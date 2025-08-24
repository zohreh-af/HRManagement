using HRManagement.Infrastructure.Api;

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

        app.UseInfrastructureApiMiddlewares();

        app.UseHttpsRedirection();

        app.UseCors("AllowBlazor");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

    }
}