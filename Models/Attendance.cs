namespace EmployeePayroll.API.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public decimal? TotalHours { get; set; }
        public string? Status { get; set; } = "Absent";
        public Employee Employee { get; set; }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }



    //public async Task AddEmployeeAsync(Employee employee)
    //    {
    //        // 1. Save Employee
    //        context.Employees.Add(employee);
    //        await context.SaveChangesAsync();

    //        // 2. Create Login Account Automatically
    //        var user = new User
    //        {
    //            Email = employee.Email,
    //            PasswordHash = "1234", // temporary default password
    //            Role = employee.Role,
    //            EmployeeId = employee.EmployeeId
    //        };

    //        context.Users.Add(user);
    //        await context.SaveChangesAsync();
    //    }
}

