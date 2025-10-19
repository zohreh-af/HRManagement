using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Exceptions;
using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HRManagement.Infrastructure.Api;

public static class InfrastructureApiServices
{
    public static void AddInfrastructureApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Swagger setting 
        #region Swagger

        services.AddSwaggerGen(o =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Auth",
                Description = "Place JWT token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            []
        }
    };

            o.AddSecurityRequirement(securityRequirement);
        });
        #endregion
        #region Serilog

        string PersianFileDate()
        {
            var pc = new PersianCalendar();
            var now = DateTime.Now;
            return $"{pc.GetYear(now):0000}-{pc.GetMonth(now):00}-{pc.GetDayOfMonth(now):00}";
        }

        var connectionString = configuration.GetConnectionString("SqlDefaultConnectionString");

        var seqUrl = configuration["Logging:Serilog:SeqUrl"];

        var rollingInterval = configuration["Logging:File:rollingInterval"];

        var rollOnFileSizeLimit = configuration["Logging:File:rollOnFileSizeLimit"];

        var logRoot = configuration["Logging:File:Path"];
        if (string.IsNullOrWhiteSpace(logRoot))
        {
            // Fallback to app-local Logs folder if not provided
            logRoot = Path.Combine(AppContext.BaseDirectory, "Logs");
        }
        var exceptionsDir = Path.Combine(logRoot, "Exceptions");
        var infoDir = Path.Combine(logRoot, "Information");
        Directory.CreateDirectory(exceptionsDir);
        Directory.CreateDirectory(infoDir);

        // Optional: file size limit in MiB (defaults to 100 MiB if not set/invalid)
        long fileSizeLimitBytes = 100L * 1024 * 1024;
        if (int.TryParse(configuration["Logging:File:MaxFileSize"], out var maxMiB) && maxMiB > 0)
            fileSizeLimitBytes = (long)maxMiB * 1024 * 1024;


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

#if DEBUG
                // Console (debug only) - JSON compact, one event per line (no rollingInterval here)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
#endif
                // === LOGIN EVENTS TABLE ===
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("LogType") && e.Properties["LogType"].ToString() == "\"Login\"")
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            AutoCreateSqlTable = true,
                            TableName = "UserLoginLog"
                        },
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        columnOptions: new ColumnOptions
                        {
                            AdditionalColumns = new Collection<SqlColumn>
                            {
                                new SqlColumn("UserId",   System.Data.SqlDbType.NVarChar,dataLength : 128),
                                new SqlColumn("UserName", System.Data.SqlDbType.NVarChar,dataLength: 256),
                                new SqlColumn("IP",       System.Data.SqlDbType.NVarChar,dataLength: 64),
                                new SqlColumn("Success",  System.Data.SqlDbType.Bit)
                            }
                        }))


                // === Sub-loggers ===

                // Human-readable exceptions (Error+), separate file, daily roll
                .WriteTo.Logger(lc => lc
                    .MinimumLevel.Error()
                    .WriteTo.File(
                        path: Path.Combine(exceptionsDir, $"Log-{PersianFileDate()}.log"),
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: fileSizeLimitBytes,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: WarningOutputTemplate,
                        shared: true))

                // Information-only stream to its own JSON file (neat NDJSON)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        path: Path.Combine(infoDir, $"Log-{PersianFileDate()}.log"),
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: fileSizeLimitBytes,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: InformationOutputTemplate,
                        shared: true));

        // Only add Seq if configured
        if (!string.IsNullOrWhiteSpace(seqUrl))
        {
            logger = logger.WriteTo.Seq(seqUrl);
        }

        Log.Logger = logger.CreateLogger();

        Log.Information("Serilog initialized. Logs root: {LogRoot}", logRoot);

        #endregion
    }
}