using EmployeePayroll.API.Data;
using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PayslipService : IPayslipService
{
    private readonly ApplicationDbContext _context;

    public PayslipService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PayrollDto>> GetEmployeePayslipsAsync(int employeeId)
    {
        var payrolls = await _context.Payrolls
            .Include(p => p.Employee)
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();

        return payrolls.Select(p => new PayrollDto
        {
            PayrollId = p.PayrollId,
            EmployeeId = p.EmployeeId,
            EmployeeName = p.Employee.FirstName + " " + p.Employee.LastName,
            Month = p.Month,
            Year = p.Year,
            BasicSalary = p.BasicSalary,
            LeaveDays = p.LeaveDays,
            Deduction = p.Deduction,
            Bonus = p.Bonus,
            NetSalary = p.NetSalary,
            Status = p.Status,
            Department = p.Employee.Department,
            Email = p.Employee.Email
        }).ToList();
    }
}