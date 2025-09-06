using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Exceptions;
using System.Globalization;

namespace HRManagement.Infrastructure.Api;

public static class InfrastructureApiServices
{
    public static void AddInfrastructureApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Swagger setting 
        #region Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BlazorHRManagement API",
                Version = "v1"
            });

            // XML comments (optional but recommended)
            var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlName);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            // JWT Bearer support (if you use [Authorize])
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Description = "Enter: Bearer {your JWT token}"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

        });
        #endregion
        #region Serilog

        string PersianFileDate()
        {
            var pc = new PersianCalendar();
            var now = DateTime.Now;
            return $"{pc.GetYear(now):0000}-{pc.GetMonth(now):00}-{pc.GetDayOfMonth(now):00}";
        }

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var seqUrl = configuration["Serilog:SeqUrl"]; 

        // Output templates (used only for text sinks, not JSON-format sinks)
        var WarningOutputTemplate =
            "-------------------{Level:u3}----------------------{NewLine}" +
            "Occur Time: {JalaliTimestamp}{NewLine}" + // <-- uses the custom enricher
            "Message   : {Message:lj}{NewLine}" +
            "SourceCtx : {SourceContext}{NewLine}" +
            "RequestId : {RequestId}{NewLine}" +
            "Exception : {Exception}{NewLine}" +
            "-------------------Exception End-------------------{NewLine}";

        var InformationOutputTemplate =
            "-------------------{Level:u3}----------------------{NewLine}" +
            "Occur Time: {Timestamp:O}{NewLine}" +
            "Message   : {Message:lj}{NewLine}" +
            "-------------------Log End-------------------------{NewLine}";

        var logger =
            new LoggerConfiguration()
                .MinimumLevel.Debug()                                     // global minimum
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // quiet framework noise
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
              
                // === Primary sinks ===
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlTable = true,
                        TableName = "UserLoginLog"
                    })

                // Seq (URL, not a “connection string”)
                .WriteTo.Seq(seqUrl)

#if DEBUG
                // Console (debug only) - JSON compact, one event per line (no rollingInterval here)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
#endif

                // === Sub-loggers ===

                // Human-readable exceptions (Error+), separate file, daily roll
                .WriteTo.Logger(lc => lc
                    .MinimumLevel.Error()
                    .WriteTo.File(
                        path: $"Logs/Exceptions/Log-{PersianFileDate()}.log",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: WarningOutputTemplate,
                        shared: true))

                // Information-only stream to its own JSON file (neat NDJSON)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        formatter: new RenderedCompactJsonFormatter(),
                        path: $"Logs/Informations/Log-{PersianFileDate()}.log",
                        rollingInterval: RollingInterval.Day,
                        shared: true))

                .CreateLogger();

        Log.Logger = logger;

        #endregion
    }
}