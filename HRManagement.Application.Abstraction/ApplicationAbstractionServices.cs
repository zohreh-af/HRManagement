using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HRManagement.Application;

public static class ApplicationAbstractionServices
{
    public static void AddApplicationAbstractionServices(this IServiceCollection service)
    {
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}