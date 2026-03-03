using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePayroll.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payrolls",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payrolls");
        }
    }
}
