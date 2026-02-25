
using Microsoft.EntityFrameworkCore;
using EmployeePayroll.API.Data;
using EmployeePayroll.API.Models;

namespace EmployeePayroll.API.Services
{
    public class LeaveService
    {
        private readonly ApplicationDbContext _context;

        public LeaveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ApplyLeaveAsync(int employeeId, DateTime fromDate, DateTime toDate, string reason)
        {
            var employeeExists = await _context.Employees
                .AnyAsync(e => e.EmployeeId == employeeId);

            if (!employeeExists)
                throw new Exception("Employee not found.");

            var leave = new Leave
            {
                EmployeeId = employeeId,
                FromDate = fromDate,
                ToDate = toDate,
                Reason = reason,
                Status = "Pending"
            };

            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
        }
        public async Task ApproveLeaveAsync(int leaveId)
        {
            var leave = await _context.Leaves
                .FirstOrDefaultAsync(l => l.LeaveId == leaveId);

            if (leave == null)
                throw new Exception("Leave request not found.");

            if (leave.Status != "Pending")
                throw new Exception("Leave already processed.");

            leave.Status = "Approved";

            await _context.SaveChangesAsync();
        }
        public async Task RejectLeaveAsync(int leaveId)
        {
            var leave = await _context.Leaves
                .FirstOrDefaultAsync(l => l.LeaveId == leaveId);

            if (leave == null)
                throw new Exception("Leave request not found.");

            if (leave.Status != "Pending")
                throw new Exception("Leave already processed.");

            leave.Status = "Rejected";

            await _context.SaveChangesAsync();
        }
        public async Task<List<Leave>> GetLeaveHistoryAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.FromDate)
                .ToListAsync();
        }
    }
}
