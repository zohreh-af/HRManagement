
using HRManagement.Infrastructure.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace HRManagement.Infrastructure.Api;

public static class InfrastructureApiMiddlewares
{
    public static IApplicationBuilder UseInfrastructureApiMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseMiddleware<SafeHttpLoggingMiddleware>();

        return app;
    }
}