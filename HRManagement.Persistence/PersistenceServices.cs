using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Persistence;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<HRManagementContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("SqlDefaultConnectionString"));
        });
    }
}
