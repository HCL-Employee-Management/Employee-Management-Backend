using EmployeePayroll.API.Data;

namespace EmployeePayroll.API.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Department { get; set; }
        public required string Role { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime JoinDate { get; set; }
        public required string Status { get; set; }

        public required ICollection<Attendance>? Attendances { get; set; }
        public required ICollection<Leave>? Leaves { get; set; }
        public required ICollection<Payroll>? Payrolls { get; set; }

        //public async Task AddEmployeeAsync(Employee employee)
        //{
        //    // 1. Save Employee
        //    _context.Employees.Add(employee);
        //    await _context.SaveChangesAsync();

        //    // 2. Create Login Account Automatically
        //    var user = new User
        //    {
        //        Email = employee.Email,
        //        PasswordHash = "1234", // temporary default password
        //        Role = employee.Role,
        //        EmployeeId = employee.EmployeeId
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();
        //}
    }

}
