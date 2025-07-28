using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace HRManagement.Application.Api;

public static class ApplicationApiServices
{
    public static void AddApplicationApiServices(this IServiceCollection service)
    {
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}