using EmployeePayroll.API.Data;
using EmployeePayroll.API.DTOs;
using EmployeePayroll.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeePayroll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        // ✅ GET ALL EMPLOYEES
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // ✅ GET EMPLOYEE BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        // ✅ CREATE EMPLOYEE
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(RegisterEmployeeDTO dto)
        {
            // Email duplicate check
            if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
                return BadRequest("Email already exists");

            var employee = new Employee
            {
                FirstName = dto.FirstName!,
                LastName = dto.LastName!,
                Email = dto.Email!,
                Phone = dto.Phone!,
                Department = dto.Department!,
                Role = dto.Role!,
                BasicSalary = dto.BasicSalary,
                JoinDate = DateTime.Now,
                Status = "Active",
                Attendances = new List<Attendance>(),
                Leaves = new List<Leave>(),
                Payrolls = new List<Payroll>()
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // ✅ UPDATE EMPLOYEE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, RegisterEmployeeDTO dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            employee.FirstName = dto.FirstName!;
            employee.LastName = dto.LastName!;
            employee.Email = dto.Email!;
            employee.Phone = dto.Phone!;
            employee.Department = dto.Department!;
            employee.Role = dto.Role!;
            employee.BasicSalary = dto.BasicSalary;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // ✅ DELETE EMPLOYEE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok("Employee deleted successfully");
        }
    }
}