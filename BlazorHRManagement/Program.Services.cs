using HRManagement.Application;
using HRManagement.Persistence;
using MudBlazor.Services;

namespace BlazorHRManagement;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {

        services.AddMudServices();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddApplicationApiServices();
        services.AddApplicationWebServices();
        services.AddUserMapperServices();
        services.AddPersistenceServices(configuration);
    }
}
