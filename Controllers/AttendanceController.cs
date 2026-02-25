using EmployeePayroll.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePayroll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _attendanceService;

        public AttendanceController(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("login/{employeeId}")]
        public async Task<IActionResult> MarkLogin(int employeeId)
        {
            try
            {
                await _attendanceService.MarkLoginAsync(employeeId);
                return Ok("Login attendance marked successfully.");
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
                return Ok("Logout marked successfully.");
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
    }
}
