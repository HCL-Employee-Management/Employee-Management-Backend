using EmployeePayroll.API.Services;
using EmployeePayroll.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PayslipController : ControllerBase
{
    private readonly IPayslipService _service;

    public PayslipController(IPayslipService service)
    {
        _service = service;
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeePayslip(int employeeId)
    {
        var result = await _service.GetEmployeePayslipsAsync(employeeId);
        return Ok(result);
    }
}