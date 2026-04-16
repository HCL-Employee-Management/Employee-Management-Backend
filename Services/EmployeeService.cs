using EmployeePayroll.API.Data;
using EmployeePayroll.API.Models;

namespace EmployeePayroll.API.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            // 1. Save Employee
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // 2. Create Login Account Automatically
            var user = new User
            {
                Email = employee.Email,
                PasswordHash = "1234", // temporary default password
                Role = employee.Role,
                EmployeeId = employee.EmployeeId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
