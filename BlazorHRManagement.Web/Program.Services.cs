using HRManagement.Application.Api;
using HRManagement.Application.Web;
using HRManagement.Infrastructure;
using HRManagement.Persistence;
using Implementation.Mediator;
using MudBlazor.Services;
using System.Net;


namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        var baseApiUri = "https://localhost:7072";
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = false,
        //                ValidateAudience = false,
        //                ValidateLifetime = true,
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere"))
        //            };
        //        });

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

        services.AddApplicationApiServices();
        services.AddApplicationWebServices();
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureWebServices(configuration);

        services.AddScoped<HttpClientHandler>();

        services.AddHttpClient("HRApi")
                .ConfigureHttpClient((services, client) =>
                {
                    var config = services.GetRequiredService<IConfiguration>();
                    var baseUri = baseApiUri;
                    client.BaseAddress = new Uri(baseUri!);
                })
                .ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();


    }
}