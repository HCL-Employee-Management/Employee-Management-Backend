using EmployeePayroll.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeePayroll.API.Data;
namespace EmployeePayroll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _attendanceService;
        private readonly ApplicationDbContext context;
        public AttendanceController(AttendanceService attendanceService, ApplicationDbContext context)
        {
            this.context = context;
            _attendanceService = attendanceService;
        }

        [HttpPost("login/{employeeId}")]
        public async Task<IActionResult> MarkLogin(int employeeId)
        {
            try
            {
                await _attendanceService.MarkLoginAsync(employeeId);
                return Ok(new { message = "Login attendance marked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("logout/{employeeId}")]
        public async Task<IActionResult> Logout(int employeeId)
        {
            try
            {
                await _attendanceService.MarkLogoutAsync(employeeId);
                return Ok(new { message = "Logout marked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetAttendanceHistory(int employeeId)
        {
            var records = await _attendanceService.GetAttendanceHistoryAsync(employeeId);
            return Ok(records);
        }
        [HttpGet("percentage")]
        public async Task<IActionResult> GetAttendancePercentage(int employeeId, int month, int year)
        {
            var percentage = await _attendanceService
                .GetAttendancePercentageAsync(employeeId, month, year);

            return Ok(new { AttendancePercentage = percentage });
        }
        
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var today = DateTime.Today;

            var employees = await context.Employees.ToListAsync();

            var attendance = await context.Attendances
                .Where(a => a.Date == today)
                .ToListAsync();

            var leaves = await context.Leaves
                .Where(l => l.FromDate <= today && l.ToDate >= today)
                .ToListAsync();

            var result = employees.Select(emp =>
            {
                var todayRecord = attendance
                    .FirstOrDefault(a => a.EmployeeId == emp.EmployeeId);

                var leaveRecord = leaves
                    .FirstOrDefault(l => l.EmployeeId == emp.EmployeeId);

                string status;

                if (leaveRecord != null)
                {
                    status = "On Leave";
                }
                else if (todayRecord == null)
                {
                    status = "Absent";
                }
                else if (todayRecord.LoginTime != null && todayRecord.LogoutTime == null)
                {
                    status = "Logged In";
                }
                else if (todayRecord.LoginTime != null && todayRecord.LogoutTime != null)
                {
                    status = "Logged Out";
                }
                else
                {
                    status = "Absent";
                }

                return new
                {
                    employeeName = emp.FirstName + " " + emp.LastName,
                    loginTime = todayRecord?.LoginTime,
                    logoutTime = todayRecord?.LogoutTime,
                    status = status
                };
            });

            return Ok(result);
        }
    }
}
