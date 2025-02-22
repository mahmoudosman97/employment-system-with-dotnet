namespace EmploymentSystem.Domain
{
    public class Application
{
    public int Id { get; set; }
    public int VacancyId { get; set; }
    public Vacancy? Vacancy { get; set; }
    public int ApplicantId { get; set; }
    public User? Applicant { get; set; } 
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}

}
