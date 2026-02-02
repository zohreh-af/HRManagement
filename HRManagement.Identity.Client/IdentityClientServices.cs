using Blazored.LocalStorage;
using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Identity.Client;

public static class IdentityClientServices
{
    public static void AddIdentityClientServices(this IServiceCollection services)
    {
        services.AddAuthenticationCore();

        services.AddCascadingAuthenticationState();

        services.AddBlazoredLocalStorage();

        services.AddScoped<AppAuthenticationStateProvider>();

        services.AddScoped<AuthenticationStateProvider>(
            sp => sp.GetRequiredService<AppAuthenticationStateProvider>());

        services.AddHttpContextAccessor();

    }
}