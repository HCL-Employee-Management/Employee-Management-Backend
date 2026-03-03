using EmployeePayroll.API.DTOs;

namespace EmployeePayroll.API.Services.Interfaces
{
    public interface IPayslipService
    {
        Task<List<PayrollDto>> GetEmployeePayslipsAsync(int employeeId);
    }
}
