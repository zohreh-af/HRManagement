namespace BlazorHRManagement.Web;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureServices(builder.Configuration);


        var app = builder.Build();
        app.Configure(builder.Configuration);
        app.Run();
    }
}