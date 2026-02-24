using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace EmployeePayroll.API.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
    }
}
