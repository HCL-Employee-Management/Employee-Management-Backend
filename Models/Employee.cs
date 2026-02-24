namespace EmployeePayroll.API.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime JoinDate { get; set; }
        public string Status { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
    }
}
