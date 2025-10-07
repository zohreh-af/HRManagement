using HRManagement.Identity.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagement.Identity.Client;

public static class IdentityClientServices
{
    public static void AddIdentityClientServices(this IServiceCollection services)
    {
        
        services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

        services.AddScoped(sp => (AppAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

        services.AddScoped<HRManagement.Application.Web.Contracts.Identity.IAuthenticationService, HRManagement.Identity.Client.Services.AuthenticationService>();
        services.AddAuthenticationCore();
        //services.AddScoped<AppAuthenticationStateProvider>;
        //  services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        //.AddCookie(options =>
        //{
        //    options.LoginPath = "/account/login";
        //    options.AccessDeniedPath = "/forbidden";
        //    options.ExpireTimeSpan = TimeSpan.FromHours(10);
        //    options.SlidingExpiration = true;
        //    // options.Cookie.Name = ".HRM.Auth";
        //    // options.Cookie.SameSite = SameSiteMode.Lax;
        //    // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //    // HttpOnly is true by default -> JS cannot read the token (good).
        //});
        //    services.AddAuthorizationCore();
        //    // IMPORTANT for Blazor Server
        services.AddHttpContextAccessor();

        //    // If you want to keep using <CascadingAuthenticationState/AuthorizeRouteView>
        // services.AddAuthenticationCore(); // Blazor auth abstractions
        // Use the built-in server provider; a custom one isn’t required.

    }
}