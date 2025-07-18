using BlazorHRManagement.Application;
using HRManagement.Application;
using HRManagement.Persistence;
using Implementation.Mediator;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Reflection;

namespace BlazorHRManagement;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        var baseUri = configuration["Api:BaseUrl"];

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IMediator, Mediator>();
        services.AddMudServices();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddApplicationApiServices();
        services.AddApplicationWebServices();
        services.AddPersistenceServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
            {
                policy.WithOrigins("https://localhost:7079", ("http://localhost:7079"))  //"https://client2.com"
                      .WithMethods("Get", "Post")
                      .AllowAnyHeader();
            });
        });

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
