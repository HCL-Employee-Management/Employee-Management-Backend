using EmployeePayroll.API.DTOs;

namespace EmployeePayroll.API.Services
{
    public interface IPayrollService
    {
        Task<List<PayrollDto>> GetPayrollAsync(string month, int year);
        Task ApplyBonusToAllAsync(string month, int year, decimal bonus);
        Task AddBonusAsync(int payrollId, decimal bonus);
        Task PayAsync(int payrollId);
        Task PayAllAsync(string month, int year);
        Task GeneratePayrollAsync(string month, int year);  
    }
}