namespace EmploymentSystem.Domain
{
    public class Vacancy
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxApplications { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int EmployerId { get; set; }
        public User Employer { get; set; } = null!;
    }
}
