namespace EmployeePayroll.API.DTOs
{
    public class PayrollDto
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string Month { get; set; }
        public int Year { get; set; }

        public decimal BasicSalary { get; set; }
        public int LeaveDays { get; set; }
        public decimal Deduction { get; set; }
        public decimal Bonus { get; set; }
        public decimal NetSalary { get; set; }

        public string Status { get; set; }  // Pending / Paid

        public string Department { get; set; }
        public string Email { get; set; }
    }
}