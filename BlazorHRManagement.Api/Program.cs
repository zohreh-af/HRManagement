using Serilog;

namespace BlazorHRManagement.Api
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureServices(builder.Configuration);
            
            builder.Host.UseSerilog(Log.Logger);

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.Configure();

            app.Run();
        }
    }
}
