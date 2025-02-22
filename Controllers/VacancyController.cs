using EmploymentSystem.Persistence;
using EmploymentSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EmploymentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly string CacheKey = "Vacancies";

        public VacancyController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // ✅ 1️⃣ استرجاع جميع الوظائف مع الكاش
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetAllVacancies()
        {
            if (!_cache.TryGetValue(CacheKey, out List<Vacancy>? vacancies))
            {
                vacancies = await _context.Vacancies.Include(v => v.Employer).ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // التخزين لمدة 5 دقائق

                _cache.Set(CacheKey, vacancies, cacheOptions);
            }

            return Ok(vacancies);
        }

        // ✅ 2️⃣ استرجاع وظيفة واحدة مع الكاش
        [HttpGet("{id}")]
        public async Task<ActionResult<Vacancy>> GetVacancyById(int id)
        {
            string cacheKey = $"Vacancy_{id}";

            if (!_cache.TryGetValue(cacheKey, out Vacancy? vacancy))
            {
                vacancy = await _context.Vacancies.Include(v => v.Employer).FirstOrDefaultAsync(v => v.Id == id);
                
                if (vacancy == null)
                    return NotFound(new { message = "Vacancy not found!" });

                _cache.Set(cacheKey, vacancy, TimeSpan.FromMinutes(5));
            }

            return Ok(vacancy);
        }

        // ✅ 3️⃣ تغيير حالة الوظيفة مع مسح الكاش
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ToggleVacancyStatus(int id, [FromBody] bool isActive)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            vacancy.IsActive = isActive;
            await _context.SaveChangesAsync();

            _cache.Remove(CacheKey);
            _cache.Remove($"Vacancy_{id}");

            return Ok(new { message = $"Vacancy {(isActive ? "activated" : "deactivated")} successfully!" });
        }

        // ✅ 4️⃣ استرجاع الوظائف المؤرشفة بدون كاش
        [HttpGet("archived")]
        public IActionResult GetArchivedVacancies()
        {
            var archivedVacancies = _context.Vacancies.Where(v => v.IsArchived).ToList();
            return Ok(archivedVacancies);
        }

        // ✅ 5️⃣ إضافة وظيفة جديدة مع مسح الكاش
        [HttpPost]
        public async Task<ActionResult<Vacancy>> CreateVacancy([FromBody] Vacancy vacancy)
        {
            if (vacancy == null || vacancy.EmployerId <= 0)
                return BadRequest(new { message = "EmployerId is required and must be valid!" });

            var employer = await _context.Users.FirstOrDefaultAsync(u => u.Id == vacancy.EmployerId);
            if (employer == null)
                return BadRequest(new { message = "Invalid EmployerId!" });

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            _cache.Remove(CacheKey);

            return CreatedAtAction(nameof(GetVacancyById), new { id = vacancy.Id }, vacancy);
        }

        // ✅ 6️⃣ تحديث وظيفة مع تحديث الكاش
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, [FromBody] Vacancy vacancy)
        {
            if (id != vacancy.Id)
                return BadRequest(new { message = "ID mismatch!" });

            var existingVacancy = await _context.Vacancies.Include(v => v.Employer).FirstOrDefaultAsync(v => v.Id == id);
            if (existingVacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            existingVacancy.Title = vacancy.Title;
            existingVacancy.Description = vacancy.Description;
            existingVacancy.ExpiryDate = vacancy.ExpiryDate;
            existingVacancy.MaxApplications = vacancy.MaxApplications;
            existingVacancy.IsActive = vacancy.IsActive;
            existingVacancy.IsArchived = vacancy.IsArchived;
            existingVacancy.EmployerId = vacancy.EmployerId;

            await _context.SaveChangesAsync();

            _cache.Remove(CacheKey);
            _cache.Remove($"Vacancy_{id}");

            return Ok(existingVacancy);
        }

        // ✅ 7️⃣ حذف وظيفة مع مسح الكاش
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();

            _cache.Remove(CacheKey);
            _cache.Remove($"Vacancy_{id}");

            return Ok(new { message = "Vacancy deleted successfully!" });
        }
    }
}
