using HRManagement.Infrastructure.Api;
using Serilog;

namespace BlazorHRManagement.Api;

public static partial class Program
{
    public static void Configure(this WebApplication app)
    {
#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseSerilogRequestLogging();
        
        app.UseHttpsRedirection();
        app.UseCors("AllowWeb");

        app.UseMiddleware<HttpLoggingMiddleware>();

        app.UseInfrastructureApiMiddlewares();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

    }
}