
using HRManagement.Infrastructure.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace HRManagement.Infrastructure.Api;

public static class InfrastructureApiMiddlewares
{
    public static IApplicationBuilder UseInfrastructureApiMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseMiddleware<SafeHttpLoggingMiddleware>();
        app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = (ctx, http) =>
            {
                ctx.Set("ClientIp", http.Connection.RemoteIpAddress?.ToString());
                ctx.Set("UserName", http.User.Identity?.Name ?? "Anonymous");
                ctx.Set("RequestPath", http.Request.Path);
                ctx.Set("UserAgent", http.Request.Headers.UserAgent.ToString());
                ctx.Set("TraceIdentifier", http.TraceIdentifier);
            };
        }

        //   app.UseMiddleware<RequestLoggingMiddleware>();

        //  app.UseMiddleware<InvalidContentCheckMiddleware>();
         return app;
    }
}