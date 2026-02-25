using EmployeePayroll.API.Models;
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
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequest request)
        {
            try
            {
                await _leaveService.ApplyLeaveAsync(
                    request.EmployeeId,
                    request.FromDate,
                    request.ToDate,
                    request.Reason);

                return Ok(new { message = "Leave applied successfully." });
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
                return Ok(new { message = "Leave approved successfully." });
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
                return Ok(new { message = "Leave rejected successfully." });
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
