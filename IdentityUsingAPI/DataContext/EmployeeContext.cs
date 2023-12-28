using IdentityUsingAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityUsingAPI.DataContext
{
    public class EmployeeContext : IdentityDbContext
    {
        public EmployeeContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Employee> Employees { get; set; }
    }
}
