
namespace BlazorHRManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.WithOrigins("https://localhost:7080", "http://localhost:7080")
                          .WithMethods("GET", "POST")
                          .AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowBlazorClient"); 
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
