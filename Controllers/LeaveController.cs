using EmployeePayroll.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePayroll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveService _leaveService;

        public LeaveController(LeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave(int employeeId, DateTime fromDate, DateTime toDate, string reason)
        {
            try
            {
                await _leaveService.ApplyLeaveAsync(employeeId, fromDate, toDate, reason);
                return Ok("Leave applied successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("approve/{leaveId}")]
        public async Task<IActionResult> ApproveLeave(int leaveId)
        {
            try
            {
                await _leaveService.ApproveLeaveAsync(leaveId);
                return Ok("Leave approved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reject/{leaveId}")]
        public async Task<IActionResult> RejectLeave(int leaveId)
        {
            try
            {
                await _leaveService.RejectLeaveAsync(leaveId);
                return Ok("Leave rejected successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetLeaveHistory(int employeeId)
        {
            var leaves = await _leaveService.GetLeaveHistoryAsync(employeeId);
            return Ok(leaves);
        }
    }
}
