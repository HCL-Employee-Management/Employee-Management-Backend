using EmployeePayroll.API.Data;
using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeePayroll.API.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly ApplicationDbContext _context;

        public PayrollService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PayrollDto>> GetPayrollAsync(string month, int year)
        {
            var payrolls = await _context.Payrolls
                .Include(p => p.Employee)
                .Where(p => p.Month == month && p.Year == year)
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
                Status = p.Status ?? "Pending"
            }).ToList();
        }

        public async Task ApplyBonusToAllAsync(string month, int year, decimal bonus)
        {
            var payrolls = await _context.Payrolls
                .Where(p => p.Month == month && p.Year == year && p.Status == "Pending")
                .ToListAsync();

            foreach (var p in payrolls)
            {
                p.Bonus += bonus;
                p.NetSalary = p.BasicSalary - p.Deduction + p.Bonus;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddBonusAsync(int payrollId, decimal bonus)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null) return;

            payroll.Bonus += bonus;
            payroll.NetSalary = payroll.BasicSalary - payroll.Deduction + payroll.Bonus;

            await _context.SaveChangesAsync();
        }

        public async Task PayAsync(int payrollId)
        {
            var payroll = await _context.Payrolls.FindAsync(payrollId);
            if (payroll == null) return;

            payroll.Status = "Paid";
            await _context.SaveChangesAsync();
        }

        public async Task PayAllAsync(string month, int year)
        {
            var payrolls = await _context.Payrolls
                .Where(p => p.Month == month && p.Year == year)
                .ToListAsync();

            foreach (var p in payrolls)
            {
                p.Status = "Paid";
            }

            await _context.SaveChangesAsync();
        }

        public async Task GeneratePayrollAsync(string month, int year)
        {
            var employees = await _context.Employees.ToListAsync();

            int monthNumber = DateTime.ParseExact(month, "MMMM",
                System.Globalization.CultureInfo.InvariantCulture).Month;

            foreach (var emp in employees)
            {
                bool exists = await _context.Payrolls
                    .AnyAsync(p => p.EmployeeId == emp.EmployeeId
                                && p.Month == month
                                && p.Year == year);

                if (!exists)
                {
                    // ✅ Get approved leaves for this employee in this month
                    var leaveDays = await _context.Leaves
                        .Where(l => l.EmployeeId == emp.EmployeeId
                                 && l.Status == "Approved"
                                 && l.FromDate.Month == monthNumber
                                 && l.FromDate.Year == year)
                        .SumAsync(l => EF.Functions.DateDiffDay(l.FromDate, l.ToDate) + 1);

                    // ✅ Per day salary calculation
                    decimal perDaySalary = emp.BasicSalary / 30;
                    decimal deduction = perDaySalary * leaveDays;

                    var payroll = new Payroll
                    {
                        EmployeeId = emp.EmployeeId,
                        Month = month,
                        Year = year,
                        BasicSalary = emp.BasicSalary,
                        LeaveDays = leaveDays,
                        Deduction = deduction,
                        Bonus = 0,
                        NetSalary = emp.BasicSalary - deduction,
                        Status = "Pending"
                    };

                    _context.Payrolls.Add(payroll);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}