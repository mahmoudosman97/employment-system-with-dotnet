using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmploymentSystem.Persistence;

namespace EmploymentSystem.Services
{
    public class ArchiveExpiredVacanciesService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ArchiveExpiredVacanciesService> _logger;

        public ArchiveExpiredVacanciesService(IServiceScopeFactory serviceScopeFactory, ILogger<ArchiveExpiredVacanciesService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var expiredVacancies = dbContext.Vacancies
                            .Where(v => v.ExpiryDate <= DateTime.UtcNow && !v.IsArchived)
                            .ToList();

                        if (expiredVacancies.Any())
                        {
                            foreach (var vacancy in expiredVacancies)
                            {
                                vacancy.IsArchived = true;
                            }

                            dbContext.SaveChanges();
                            _logger.LogInformation($"{expiredVacancies.Count} vacancies archived.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while archiving expired vacancies.");
                }

                // انتظر 24 ساعة قبل تنفيذ الأرشفة مرة أخرى (يمكنك تعديلها)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
