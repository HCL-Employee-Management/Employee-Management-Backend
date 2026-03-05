using EmployeePayroll.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeePayroll.API.Services
{
    public class AutoLogoutService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoLogoutService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAutoLogout();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckAutoLogout()
        {
            var now = DateTime.Now;

            // Run near midnight
            if (now.Hour == 23 && now.Minute == 59)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var today = DateTime.Today;

                var records = await context.Attendances
                    .Where(a =>
                        a.Date == today &&
                        a.LoginTime != null &&
                        a.LogoutTime == null)
                    .ToListAsync();

                foreach (var attendance in records)
                {
                    attendance.LogoutTime = new DateTime(
                        today.Year,
                        today.Month,
                        today.Day,
                        23,
                        59,
                        0);

                    var totalHours =
                        (attendance.LogoutTime.Value - attendance.LoginTime.Value)
                        .TotalHours;

                    attendance.TotalHours = (decimal)totalHours;

                    if (totalHours >= 8)
                        attendance.Status = "Present";
                    else
                        attendance.Status = "Half Day";
                }

                await context.SaveChangesAsync();
            }
        }
    }
}