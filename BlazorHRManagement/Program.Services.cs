using BlazorHRManagement.Application;
using HRManagement.Application;
using HRManagement.Persistence;
using Implementation.Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Reflection;
using System.Text;

namespace BlazorHRManagement;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        var baseUri = configuration["Api:BaseUrl"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere"))
        };
    });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IMediator, Mediator>();
        services.AddMudServices();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddApplicationApiServices();
        services.AddApplicationWebServices();
        services.AddPersistenceServices(configuration);

        services.AddScoped<HttpClientHandler>();

        services.AddHttpClient("HRApi")
            .ConfigureHttpClient((services, client) =>
            {
                var config = services.GetRequiredService<IConfiguration>();
                var baseUri = config["Api:BaseUrl"];
                client.BaseAddress = new Uri(baseUri!);
            })
            .ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();
            }
}
