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

        public Employee Employee { get; set; }
    }
}
