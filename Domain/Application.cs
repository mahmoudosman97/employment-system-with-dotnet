namespace EmploymentSystem.Domain
{
    public class Application
{
    public int Id { get; set; }
    public int VacancyId { get; set; }
    public Vacancy? Vacancy { get; set; } // جعلها nullable
    public int ApplicantId { get; set; }
    public User? Applicant { get; set; } // جعلها nullable
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}

}
