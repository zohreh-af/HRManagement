using Abstraction;
using HRManagement.Application.Web;
using HRManagement.Identity.Client; 
using HRManagement.Infrastructure;
using HRManagement.Presentation;
using Implementation.Mediator;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using System.Net;
namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        var baseApiUri = configuration["Api:BaseUrl"];
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddScoped<IMediator, Mediator>();
        services.AddMudServices();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        services.AddIdentityClientServices();
        services.AddApplicationWebServices();
        services.AddInfrastructureWebServices(configuration);
        services.AddPresentationServices();
        services.AddScoped<HttpClientHandler>();

        services.AddHttpClient("HRApi")
                .ConfigureHttpClient((services, client) =>
                {
                    var config = services.GetRequiredService<IConfiguration>();
                    var baseUri = baseApiUri;
                    client.BaseAddress = new Uri(baseUri!);
                })
                .ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();
        services.AddAuthorization();

    }
}