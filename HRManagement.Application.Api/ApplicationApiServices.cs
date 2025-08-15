using Abstraction;
using HRManagement.Application.Api.Features.Account.Queries.LoginUser;
using HRManagement.Application.Api.Features;
using HRManagement.Application.Web.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace HRManagement.Application.Api;

public static class ApplicationApiServices
{
    public static void AddApplicationApiServices(this IServiceCollection service)
    {

        // رجیستر صریح هندلر CreateUser
        service.AddScoped<
         ICommandHandler<
           CreateUserCommand,
            CreateUserVm>,
          CreateUserHandler>();

        // اگر LoginUser هم داری:
        service.AddScoped<
        IQueryHandler<
        LoginUserQuery,
        LoginUserVm>,
        LoginUserHandler>();
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}