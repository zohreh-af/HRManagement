using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRManagement.Application;

public static class ApplicationWebServices
{
    public static void AddApplicationWebServices(this IServiceCollection service)
    {
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
