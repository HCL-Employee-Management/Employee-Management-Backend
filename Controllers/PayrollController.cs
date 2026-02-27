using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePayroll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _service;

        public PayrollController(IPayrollService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePayroll(PayrollDTO dto)
        {
            var result = await _service.GeneratePayroll(dto);
            return Ok(result);
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetEmployeePayroll(int id)
        {
            var result = await _service.GetPayrollByEmployee(id);
            return Ok(result);
        }

        [HttpGet("attendance-percentage")]
        public IActionResult GetAttendancePercentage(int employeeId, int month, int year)
        {
            var percentage = _service.CalculateAttendancePercentage(employeeId, month, year);
            return Ok(percentage);
        }
    }
}
