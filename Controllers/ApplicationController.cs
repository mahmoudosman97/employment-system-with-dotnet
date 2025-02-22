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

    
    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] Application application)
    {
        var existingApplication = await _context.Applications
            .FirstOrDefaultAsync(a => a.ApplicantId == application.ApplicantId &&
                                      a.AppliedAt.Date == DateTime.UtcNow.Date);

        if (existingApplication != null)
            return BadRequest(new { message = "You can only apply for one vacancy per day!" });

        var vacancy = await _context.Vacancies.FindAsync(application.VacancyId);
        if (vacancy == null)
            return NotFound(new { message = "Vacancy not found!" });

        int applicationsCount = await _context.Applications
            .CountAsync(a => a.VacancyId == application.VacancyId);

        if (applicationsCount >= vacancy.MaxApplications)
            return BadRequest(new { message = "Maximum applications limit reached for this vacancy!" });

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Application submitted successfully!" });
    }

    
    [HttpGet("search")]
    public async Task<IActionResult> SearchVacancies([FromQuery] string title)
    {
        var vacancies = await _context.Vacancies
            .Where(v => v.Title.Contains(title))
            .ToListAsync();

        return Ok(vacancies);
    }

    
   [HttpGet("applicant/{applicantId}")]
public async Task<IActionResult> GetApplicationsByApplicant(int applicantId)
{
    var applications = await _context.Applications
        .Where(a => a.ApplicantId == applicantId)
        .Include(a => a.Vacancy)         
            .ThenInclude(v => v.Employer) 
        .Include(a => a.Applicant)        
        .ToListAsync();

    if (!applications.Any())
        return NotFound(new { message = "No applications found for this applicant." });

    return Ok(applications);
}


    
   [HttpGet("vacancy/{vacancyId}")]
public async Task<IActionResult> GetApplicationsByVacancy(int vacancyId)
{
    var applications = await _context.Applications
        .Where(a => a.VacancyId == vacancyId)
        .Include(a => a.Vacancy) 
        .Include(a => a.Applicant) 
        .ToListAsync();

    if (!applications.Any())
        return NotFound(new { message = "No applicants found for this vacancy." });

    return Ok(applications);
}

}
