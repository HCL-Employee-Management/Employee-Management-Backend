using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Models;

namespace EmployeePayroll.API.Services.Interfaces
{
    public interface IPayrollService
    {
        Task<Payroll> GeneratePayroll(PayrollDTO dto);
        Task<IEnumerable<Payroll>> GetPayrollByEmployee(int employeeId);
        decimal CalculateAttendancePercentage(int employeeId, int month, int year);
    }
}