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

            int monthNumber = DateTime.ParseExact(
                month,
                "MMMM",
                System.Globalization.CultureInfo.InvariantCulture).Month;

            int daysInMonth = DateTime.DaysInMonth(year, monthNumber);

            int workingDays = 0;

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime date = new DateTime(year, monthNumber, i);

                if (date.DayOfWeek != DayOfWeek.Sunday)
                    workingDays++;
            }

            foreach (var emp in employees)
            {
                var payroll = await _context.Payrolls.FirstOrDefaultAsync(p =>
                    p.EmployeeId == emp.EmployeeId &&
                    p.Month == month &&
                    p.Year == year);

                // Get latest attendance
                var attendance = await _context.Attendances
                    .Where(a =>
                        a.EmployeeId == emp.EmployeeId &&
                        a.Date.Month == monthNumber &&
                        a.Date.Year == year)
                    .ToListAsync();

                int presentDays = attendance.Count(a => a.Status == "Present");

                int halfDays = attendance.Count(a => a.Status == "Half Day");

                int absentDays = workingDays - presentDays - halfDays;

                decimal perDaySalary = emp.BasicSalary / workingDays;

                decimal fullSalary = presentDays * perDaySalary;

                decimal halfSalary = halfDays * (perDaySalary * 0.5m);

                decimal deduction =
                    (absentDays * perDaySalary) +
                    (halfDays * perDaySalary * 0.5m);

                decimal netSalary = fullSalary + halfSalary;

                if (payroll == null)
                {
                    // Create new payroll
                    payroll = new Payroll
                    {
                        EmployeeId = emp.EmployeeId,
                        Month = month,
                        Year = year
                    };

                    _context.Payrolls.Add(payroll);
                }

                // Update payroll every time
                payroll.BasicSalary = emp.BasicSalary;
                payroll.LeaveDays = absentDays;
                payroll.Deduction = deduction;
                payroll.NetSalary = netSalary;

                if (string.IsNullOrEmpty(payroll.Status))
                    payroll.Status = "Pending";
            }

            await _context.SaveChangesAsync();
        }
    }
}