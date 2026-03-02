using EmployeePayroll.API.Data;
using Microsoft.EntityFrameworkCore;
namespace EmployeePayroll.API.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.PasswordHash == password);

            if (user == null)
                throw new Exception("Invalid email or password");

            return new
            {
                employeeId = user.EmployeeId,
                role = user.Role
            };
        }
    }
}
