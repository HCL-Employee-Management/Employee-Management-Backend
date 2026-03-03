using EmployeePayroll.API.Services;
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

        // Get Payroll for Month
        [HttpGet]
        public async Task<IActionResult> Get(string month, int year)
        {
            var result = await _service.GetPayrollAsync(month, year);
            return Ok(result);
        }

        // Apply Bonus To All
        [HttpPost("apply-bonus-all")]
        public async Task<IActionResult> ApplyBonusToAll(string month, int year, decimal bonus)
        {
            await _service.ApplyBonusToAllAsync(month, year, bonus);
            return Ok(new { message = "Bonus Applied" });
        }

        // Add Bonus Single
        [HttpPost("add-bonus/{payrollId}")]
        public async Task<IActionResult> AddBonus(int payrollId, decimal bonus)
        {
            await _service.AddBonusAsync(payrollId, bonus);
            return Ok(new { message = "Bonus Added" });
        }

        // Pay Single
        [HttpPost("pay/{payrollId}")]
        public async Task<IActionResult> Pay(int payrollId)
        {
            await _service.PayAsync(payrollId);
            return Ok(new { message = "Paid" });
        }

        // Pay All
        [HttpPost("pay-all")]
        public async Task<IActionResult> PayAll(string month, int year)
        {
            await _service.PayAllAsync(month, year);
            return Ok(new { message = "All Paid" });
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate(string month, int year)
        {
            await _service.GeneratePayrollAsync(month, year);
            return Ok(new { message = "Payroll Generated" });
        }
    }
}