using Blazored.LocalStorage;
using HRManagement.Infrastructure.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Infrastructure;

public static class InfrastructureWebServices
{
    public static void AddInfrastructureWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApiHandler>();
    }
}