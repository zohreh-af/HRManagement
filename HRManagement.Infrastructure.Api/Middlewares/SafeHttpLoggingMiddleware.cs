using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

public sealed class SafeHttpLoggingMiddleware
{
    private readonly RequestDelegate _next;

    // Allow only safe, small, text-like bodies
    private static readonly string[] AllowedContentTypes =
    {
        "application/json",
        "application/problem+json",
        "application/x-www-form-urlencoded",
        "text/plain",
        "text/json"
    };

    // Common sensitive keys to mask
    private static readonly string[] SensitiveKeys =
    {
        "password","pwd","pass","token","access_token","refresh_token",
        "authorization","api_key","apikey","secret","client_secret",
        "creditcard","card","cvv","ssn","cookie","set-cookie"
    };

    private const int MaxBodyBytes = 4 * 1024; // limit to 4KB

    public SafeHttpLoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        // Request body (safe capture)
        string? reqBody = await ReadRequestBodySafe(context);

        // Swap response body to capture it
        var original = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        var sw = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();

            // Capture response safely
            string? respBody = await ReadResponseBodySafe(context, buffer);

            // Copy response back to client
            buffer.Position = 0;
            await buffer.CopyToAsync(original);
            context.Response.Body = original;

            var status = context.Response.StatusCode;
            bool includeBodies = status >= 400; // only on errors by default

            var log = Serilog.Log.ForContext("Method", context.Request.Method)
                                 .ForContext("Path", context.Request.Path.Value)
                                 .ForContext("StatusCode", status)
                                 .ForContext("ElapsedMs", sw.ElapsedMilliseconds)
                                 .ForContext("ClientIp", context.Connection.RemoteIpAddress?.ToString())
                                 .ForContext("UserAgent", context.Request.Headers.UserAgent.ToString());

            if (includeBodies)
            {
                log = log.ForContext("RequestBody", reqBody)
                         .ForContext("ResponseBody", respBody)
                         .ForContext("ResponseContentType", context.Response.ContentType);
            }

            if (status >= 500)
                log.Error("HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs} ms", context.Request.Method, context.Request.Path, status, sw.ElapsedMilliseconds);
            else if (status >= 400)
                log.Warning("HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs} ms", context.Request.Method, context.Request.Path, status, sw.ElapsedMilliseconds);
            else
                log.Information("HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs} ms", context.Request.Method, context.Request.Path, status, sw.ElapsedMilliseconds);
        }
    }

    private static bool IsAllowedContentType(string? contentType) =>
        !string.IsNullOrWhiteSpace(contentType) &&
        AllowedContentTypes.Any(t => contentType!.StartsWith(t, StringComparison.OrdinalIgnoreCase));

    private static async Task<string?> ReadRequestBodySafe(HttpContext ctx)
    {
        var req = ctx.Request;

        if (!IsAllowedContentType(req.ContentType)) return null;
        if ((req.ContentType ?? "").StartsWith("multipart/", StringComparison.OrdinalIgnoreCase)) return null;

        req.EnableBuffering();

        using var reader = new StreamReader(req.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        char[] buf = new char[MaxBodyBytes];
        int read = await reader.ReadBlockAsync(buf, 0, MaxBodyBytes);
        req.Body.Position = 0;

        var raw = new string(buf, 0, read);
        return RedactBody(raw, req.ContentType);
    }

    private static async Task<string?> ReadResponseBodySafe(HttpContext ctx, MemoryStream mem)
    {
        var contentType = ctx.Response.ContentType;
        if (!IsAllowedContentType(contentType)) return null;

        mem.Position = 0;
        using var reader = new StreamReader(mem, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        char[] buf = new char[MaxBodyBytes];
        int read = await reader.ReadBlockAsync(buf, 0, MaxBodyBytes);
        var raw = new string(buf, 0, read);

        return RedactBody(raw, contentType);
    }

    private static string RedactBody(string raw, string? contentType)
    {
        if (string.IsNullOrWhiteSpace(raw)) return raw;

        // JSON redaction by key
        if (!string.IsNullOrWhiteSpace(contentType) &&
            contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var redacted = RedactJson(doc.RootElement);
                return JsonSerializer.Serialize(redacted);
            }
            catch
            {
                // fall through to simple masking if not valid JSON
            }
        }

        // Simple masking for text/form
        foreach (var key in SensitiveKeys)
        {
            raw = Regex.Replace(
                raw,
                $@"({key}\s*[:=]\s*)([^&\s,]+)",
                "$1***REDACTED***",
                RegexOptions.IgnoreCase);
        }

        return raw;
    }

    private static object? RedactJson(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(
                    p => p.Name,
                    p => SensitiveKeys.Contains(p.Name, StringComparer.OrdinalIgnoreCase)
                            ? "***REDACTED***"
                            : RedactJson(p.Value)),
            JsonValueKind.Array => element.EnumerateArray().Select(RedactJson).ToList(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var i) ? i : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null
        };
    }
}
