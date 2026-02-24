namespace EmployeePayroll.API.DTOs
{
    public class PayrollDTO
    {
        public int EmployeeId { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal Bonus { get; set; }
    }
}
