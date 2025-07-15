using HRManagement.Application;
using HRManagement.Persistence;

namespace BlazorHRManagement;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        services.AddApplicationApiServices();
        services.AddApplicationWebServices();
        services.AddUserMapperServices();
        services.AddPersistenceServices(configuration);
    }
}
