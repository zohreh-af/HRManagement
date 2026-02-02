using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HRManagement.Persistence.Contexts;

public class HRManagementContextFactory : IDesignTimeDbContextFactory<HRManagementContext>
{
    public HRManagementContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HRManagementContext>();

        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HRManagement;Trusted_Connection=True;TrustServerCertificate=True");

        return new HRManagementContext(optionsBuilder.Options);
    }
}