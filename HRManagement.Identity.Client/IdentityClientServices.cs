using HRManagement.Application.Web.Contracts.Identity;
using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace HRManagement.Identity.Client;

public static class IdentityClientServices
{
    public static void AddIdentityClientServices(this IServiceCollection services)
    {
        services.AddAuthenticationCore();
        services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

        services.AddScoped(sp => (AppAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

        services.AddScoped<HRManagement.Application.Web.Contracts.Identity.IAuthenticationService, HRManagement.Identity.Client.Services.AuthenticationService>();

    }
}