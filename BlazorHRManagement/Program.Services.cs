using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlazorHRManagement
{
    public static partial class Program
    {
        public static void ConfigureServices(IServiceCollection services
            , IConfiguration config)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

            services.AddAuthorization();
            



        }
    }
}
