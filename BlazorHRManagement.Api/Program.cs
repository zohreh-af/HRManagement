using Serilog;

namespace BlazorHRManagement.Api
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog(Log.Logger);

            Program.ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.Services.GetRequiredService<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider>();
            
            app.UseSerilogRequestLogging();

            app.Configure();

            app.Run();
        }
    }
}
