using Abstraction;
using HRManagement.Application.Web;
using HRManagement.Identity.Client; 
using HRManagement.Infrastructure;
using HRManagement.Presentation;
using Implementation.Mediator;
using MudBlazor.Services;

namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services
    , IConfiguration configuration)
    {
        var baseApiUri = configuration["Api:BaseUrl"];
        services.AddRazorPages();
        services.AddRazorComponents()
               .AddInteractiveServerComponents();

        services.AddServerSideBlazor();

        services.AddDistributedMemoryCache();
       
        services.AddScoped<IMediator, Mediator>();
        services.AddMudServices();


        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        services.AddIdentityClientServices();
        services.AddApplicationWebServices();
        services.AddInfrastructureWebServices(configuration);
        services.AddPresentationServices();


        services.AddAntiforgery(options =>
        {
            options.FormFieldName = "__RequestVerificationToken"; // hidden input name
            options.HeaderName = "RequestVerificationToken";   // header for AJAX
                                                               // options.Cookie.Name = "XSRF-TOKEN";                 // optional custom cookie
        });

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