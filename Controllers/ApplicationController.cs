using Microsoft.AspNetCore.Mvc;
using EmploymentSystem.Domain;
using EmploymentSystem.Persistence;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ApplicationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // تقديم طلب جديد
    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] Application application)
    {
        var existingApplication = await _context.Applications
            .FirstOrDefaultAsync(a => a.ApplicantId == application.ApplicantId &&
                                      a.AppliedAt.Date == DateTime.UtcNow.Date);

        if (existingApplication != null)
            return BadRequest(new { message = "You can only apply for one vacancy per day!" });

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Application submitted successfully!" });
    }

    // جلب جميع التقديمات
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var applications = await _context.Applications.ToListAsync();
        return Ok(applications);
    }
}
