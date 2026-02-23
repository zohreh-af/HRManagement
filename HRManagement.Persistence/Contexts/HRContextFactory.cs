using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Persistence.Contexts;

public class HRContextFactory : IDesignTimeDbContextFactory<HRContext>
{
    public HRContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HRContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DB_HRManagement;Trusted_Connection=True;TrustServerCertificate=True;");

        return new HRContext(optionsBuilder.Options);
    }
}