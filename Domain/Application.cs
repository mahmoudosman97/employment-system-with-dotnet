namespace EmploymentSystem.Domain
{
    public class Application
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; } = null!;
        public int ApplicantId { get; set; }
        public User Applicant { get; set; } = null!;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}
