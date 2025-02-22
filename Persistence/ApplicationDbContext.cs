using Microsoft.EntityFrameworkCore;
using EmploymentSystem.Domain;

namespace EmploymentSystem.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<Vacancy>()
                .HasOne(v => v.Employer)
                .WithMany()
                .HasForeignKey(v => v.EmployerId)
                .OnDelete(DeleteBehavior.Restrict); 

          
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Vacancy)
                .WithMany()
                .HasForeignKey(a => a.VacancyId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Applicant)
                .WithMany()
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
