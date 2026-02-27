using EmployeePayroll.API.Data;
using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Models;
using EmployeePayroll.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EmployeePayroll.API.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly ApplicationDbContext _context;

        public PayrollService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payroll> GeneratePayroll(PayrollDTO dto)
        {
            // Get Employee
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId);

            if (employee == null)
                throw new Exception("Employee not found");

            // Convert Month string to month number safely
            int monthNumber = DateTime.ParseExact(
                dto.Month,
                "MMMM",
                CultureInfo.InvariantCulture).Month;

            // Check if payroll already exists
            var existingPayroll = await _context.Payrolls
                .FirstOrDefaultAsync(p =>
                    p.EmployeeId == dto.EmployeeId &&
                    p.Month == dto.Month &&
                    p.Year == dto.Year);

            if (existingPayroll != null)
                throw new Exception("Payroll already generated for this month");

            // Count approved leave days for that month and year
            var leaveDays = await _context.Leaves
                .Where(l => l.EmployeeId == dto.EmployeeId
                            && l.Status == "Approved"
                            && l.FromDate.Month == monthNumber
                            && l.FromDate.Year == dto.Year)
                .CountAsync();

            // Salary Calculation
            decimal perDaySalary = employee.BasicSalary / 30;
            decimal deduction = perDaySalary * leaveDays;
            decimal netSalary = employee.BasicSalary - deduction + dto.Bonus;

            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                Month = dto.Month,
                Year = dto.Year,
                BasicSalary = employee.BasicSalary,
                LeaveDays = leaveDays,
                Deduction = Math.Round(deduction, 2),
                Bonus = dto.Bonus,
                NetSalary = Math.Round(netSalary, 2)
            };

            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();

            return payroll;
        }

        public async Task<IEnumerable<Payroll>> GetPayrollByEmployee(int employeeId)
        {
            return await _context.Payrolls
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public decimal CalculateAttendancePercentage(int employeeId, int month, int year)
        {
            var totalDays = DateTime.DaysInMonth(year, month);

            if (totalDays == 0)
                return 0;

            var presentDays = _context.Attendances
                .Count(a => a.EmployeeId == employeeId
                            && a.Date.Month == month
                            && a.Date.Year == year);

            return Math.Round(((decimal)presentDays / totalDays) * 100, 2);
        }
    }
}