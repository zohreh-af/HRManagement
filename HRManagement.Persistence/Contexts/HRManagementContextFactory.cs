using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HRManagement.Persistence.Contexts;

public class HRManagementContextFactory : IDesignTimeDbContextFactory<HRManagementContext>
{
    public HRManagementContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<HRManagementContext>();
        var connectionString = configuration.GetConnectionString("SqlDefaultConnectionString");

        builder.UseSqlServer(connectionString);

        return new HRManagementContext(builder.Options);
    }
}