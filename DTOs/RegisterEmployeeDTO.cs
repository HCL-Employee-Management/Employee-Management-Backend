namespace EmployeePayroll.API.DTOs
{
    public class RegisterEmployeeDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public decimal BasicSalary { get; set; }
    }
}
