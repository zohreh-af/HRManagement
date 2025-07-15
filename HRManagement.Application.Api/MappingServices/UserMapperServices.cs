
using HRManagement.Application.Api.Abstraction;
using HRManagement.Application.Api.Features;
using HRManagement.Application.Web.Features;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Application;
public static class UserMapperServices
{
    public static void AddUserMapperServices(this IServiceCollection service)
    {
        service.AddScoped<ICommandHandler<CreateUserCommand,CreateUserVm>,CreateUserHandler>();
    }
}