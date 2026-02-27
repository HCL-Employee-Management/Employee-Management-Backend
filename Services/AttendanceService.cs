using EmployeePayroll.API.Models;
using Microsoft.EntityFrameworkCore;
using EmployeePayroll.API.Data;
namespace EmployeePayroll.API.Services
{
    public class AttendanceService
    {
        private readonly ApplicationDbContext context;
        public AttendanceService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task MarkLoginAsync(int employeeId)
        {
            var today = DateTime.Today;

            var existing = await context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

            if (existing != null)
                throw new Exception("Attendance already marked for today.");

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = today,
                LoginTime = DateTime.Now
            };

            context.Attendances.Add(attendance);
            await context.SaveChangesAsync();
        }
        public async Task MarkLogoutAsync(int employeeId)
        {
            var today = DateTime.Today;

            var attendance = await context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

            if (attendance == null)
                throw new Exception("Login not found for today.");

            if (attendance.LogoutTime != null)
                throw new Exception("Logout already marked.");

            attendance.LogoutTime = DateTime.Now;

           
            if (attendance.LoginTime.HasValue)
            {
                var totalHours = (attendance.LogoutTime.Value - attendance.LoginTime.Value).TotalHours;
                attendance.TotalHours = (decimal)totalHours;
            }

            await context.SaveChangesAsync();
        }
        public async Task<List<Attendance>> GetAttendanceHistoryAsync(int employeeId)
        {
            return await context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
        public async Task<double> GetAttendancePercentageAsync(int employeeId, int month, int year)
        {
            var records = await context.Attendances
                .Where(a => a.EmployeeId == employeeId
                            && a.Date.Month == month
                            && a.Date.Year == year)
                .ToListAsync();

            if (records.Count == 0)
                return 0;

            var presentDays = records.Count(a => a.LogoutTime != null);
            var totalDays = records.Count;

            double percentage = ((double)presentDays / totalDays) * 100;

            return Math.Round(percentage, 2);
        }
    }
}
