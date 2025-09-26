using System;

#if NET48
using System.Configuration; // فقط روی net48
#endif

namespace Logger;

public sealed class LoggingOptions
{
    public bool IncludeRequestBody { get; set; } = true;
    public bool IncludeResponseBody { get; set; } = true;
    public int MaxBodyBytes { get; set; } = 64 * 1024;
    public bool PrettyPrintJson { get; set; } = true;
    public string CorrelationHeaderName { get; set; } = "X-Correlation-ID";
    public string SessionUserKey { get; set; } = "UserName";
    public string UsernameHeaderFallback { get; set; } = "X-User-Name";

    public string LogDirectory { get; set; } = "logs";
    public string FileNameTemplate { get; set; } = "log-.json"; // با RollingInterval.Day
    public long FileSizeLimitBytes { get; set; } = 10 * 1024 * 1024; // 10MB
    public int? RetainedFileCountLimit { get; set; } = 7; // معادل ~7 روز وقتی Rolling = Day
    public bool RollOnFileSizeLimit { get; set; } = true;
}

public static class BindingOptions
{
    // روی net48 از web.config می‌خوانیم؛ روی Core/Net8 دست نمی‌زنیم (پیش‌فرض‌ها باقی می‌مانند یا از IConfiguration بخوان)
    public static void BindFromWebConfig(LoggingOptions opt)
    {
#if NET48
        if (bool.TryParse(ConfigurationManager.AppSettings["SadeLogging:IncludeRequestBody"], out var b1)) opt.IncludeRequestBody = b1;
        if (bool.TryParse(ConfigurationManager.AppSettings["SadeLogging:IncludeResponseBody"], out var b2)) opt.IncludeResponseBody = b2;
        if (int .TryParse(ConfigurationManager.AppSettings["SadeLogging:MaxBodyBytes"], out var i1)) opt.MaxBodyBytes = i1;
        if (bool.TryParse(ConfigurationManager.AppSettings["SadeLogging:PrettyPrintJson"], out var b3)) opt.PrettyPrintJson = b3;

        opt.LogDirectory     = ConfigurationManager.AppSettings["SadeLogging:LogDirectory"]     ?? opt.LogDirectory;
        opt.FileNameTemplate = ConfigurationManager.AppSettings["SadeLogging:FileNameTemplate"] ?? opt.FileNameTemplate;

        if (long.TryParse(ConfigurationManager.AppSettings["SadeLogging:FileSizeLimitBytes"], out var l1)) opt.FileSizeLimitBytes = l1;
        if (int .TryParse(ConfigurationManager.AppSettings["SadeLogging:RetainedFileCountLimit"], out var i2)) opt.RetainedFileCountLimit = i2;
        if (bool.TryParse(ConfigurationManager.AppSettings["SadeLogging:RollOnFileSizeLimit"], out var b4)) opt.RollOnFileSizeLimit = b4;
#endif
    }

#if !NET48
    // اختیاری: نسخه‌ی Core برای appsettings.json
    public static void BindFromConfiguration(LoggingOptions opt, Microsoft.Extensions.Configuration.IConfiguration cfg)
        => cfg.GetSection("SadeLogging").Bind(opt);
#endif
}