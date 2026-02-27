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
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = context.Users
                .FirstOrDefault(u =>
                    u.Email == request.Email &&
                    u.PasswordHash == request.Password);

            if (user == null)
                return BadRequest(new { message = "Invalid email or password" });

            return Ok(new
            {
                employeeId = user.EmployeeId,  // now nullable
                role = user.Role
            });
        }
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var today = DateTime.Today;

            var employees = await context.Employees.ToListAsync();

            var attendance = await context.Attendances
                .Where(a => a.Date == today)
                .ToListAsync();

            var result = employees.Select(emp =>
            {
                var todayRecord = attendance
                    .FirstOrDefault(a => a.EmployeeId == emp.EmployeeId);

                return new
                {
                    employeeName = emp.FirstName + " " + emp.LastName,
                    loginTime = todayRecord?.LoginTime,
                    logoutTime = todayRecord?.LogoutTime
                };
            });

            return Ok(result);
        }
    }
}
