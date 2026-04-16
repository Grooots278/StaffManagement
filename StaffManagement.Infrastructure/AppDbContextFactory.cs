using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StaffManagement.Infrastructure.Data;

namespace StaffManagement.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=StaffManagementDb;Username=staffuser;Password=StaffPass123!");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
