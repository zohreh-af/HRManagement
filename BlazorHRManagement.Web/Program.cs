namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void Main(string[] args)
    {
		try
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.ConfigureServices(builder.Configuration);

			var app = builder.Build();
			app.Configure(builder.Configuration);
			app.Run();
		}
		catch (Exception ex )
		{
#if DEBUG
			Console.WriteLine($"Startup failed: {ex}"); 
#endif
			throw;
        }
    }
}