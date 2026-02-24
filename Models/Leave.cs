namespace EmployeePayroll.API.Models
{
    public class Leave
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

        public Employee Employee { get; set; }
    }
}
