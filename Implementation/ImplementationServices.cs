using Abstraction;
using Implementation.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Implementation;

public static class ImplementationServices
{
    public static void AddImplementationServices(this IServiceCollection service)
    {

        service.AddScoped<IMediator, Mediator>();
    }
}