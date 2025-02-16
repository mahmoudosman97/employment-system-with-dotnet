using EmploymentSystem.Persistence;
using EmploymentSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmploymentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VacancyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ 1️⃣ إرجاع جميع الوظائف
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vacancy>>> GetAllVacancies()
        {
            var vacancies = await _context.Vacancies.Include(v => v.Employer).ToListAsync();
            return Ok(vacancies);
        }

        // ✅ 2️⃣ إرجاع وظيفة واحدة
        [HttpGet("{id}")]
        public async Task<ActionResult<Vacancy>> GetVacancyById(int id)
        {
            var vacancy = await _context.Vacancies.Include(v => v.Employer).FirstOrDefaultAsync(v => v.Id == id);
            
            if (vacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            return Ok(vacancy);
        }

        // ✅ 3️⃣ إضافة وظيفة جديدة
        [HttpPost]
        public async Task<ActionResult<Vacancy>> CreateVacancy([FromBody] Vacancy vacancy)
        {
            if (vacancy == null || vacancy.EmployerId <= 0)
            {
                return BadRequest(new { message = "EmployerId is required and must be valid!" });
            }

            // التحقق من أن Employer موجود في قاعدة البيانات
            var employer = await _context.Users.FirstOrDefaultAsync(u => u.Id == vacancy.EmployerId);
            if (employer == null)
            {
                return BadRequest(new { message = "Invalid EmployerId!" });
            }

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVacancyById), new { id = vacancy.Id }, vacancy);
        }

        // ✅ 4️⃣ تحديث وظيفة
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, [FromBody] Vacancy vacancy)
        {
            if (id != vacancy.Id)
                return BadRequest(new { message = "ID mismatch!" });

            var existingVacancy = await _context.Vacancies.FindAsync(id);
            if (existingVacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            // تحديث القيم
            existingVacancy.Title = vacancy.Title;
            existingVacancy.Description = vacancy.Description;
            existingVacancy.ExpiryDate = vacancy.ExpiryDate;
            existingVacancy.MaxApplications = vacancy.MaxApplications;
            existingVacancy.IsActive = vacancy.IsActive;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ 5️⃣ حذف وظيفة
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound(new { message = "Vacancy not found!" });

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
