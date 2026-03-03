namespace EmployeePayroll.API.Models
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public int LeaveDays { get; set; }
        public decimal Deduction { get; set; }
        public decimal Bonus { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; } = "Pending";
        public Employee Employee { get; set; }
    }
}
