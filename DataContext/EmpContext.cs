using EmployeeTest.Models;
using Microsoft.EntityFrameworkCore;
namespace EmployeeTest.DataContext
{
    public class EmpContext : DbContext
    {
        public EmpContext(DbContextOptions Options):base (Options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalarys { get; set; }

    }
}
