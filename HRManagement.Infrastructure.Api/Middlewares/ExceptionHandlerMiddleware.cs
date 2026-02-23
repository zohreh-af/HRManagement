using System.Diagnostics;
using System.Net.Http;
using System.Net;
using HRManagement.Shared;
using HRManagement.Shared.Dtos;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using HRManagement.Infrastructure.Api.Exceptions;
using Microsoft.Extensions.Logging;

namespace HRManagement.Infrastructure.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> logger;

    public ExceptionHandlerMiddleware(RequestDelegate next
       ,ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
       this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private Task ConvertException(HttpContext context, Exception exception)
    {
        logger.LogWarning(exception, exception.Message);

        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var result = string.Empty;

#if DEBUG
        Debugger.Break();
#endif

        switch (exception)
        {
            case InvalidFileFormatException invalidFileEx:
                httpStatusCode = HttpStatusCode.UnsupportedMediaType;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Success = false,
                    Data = null,
                    Message = new[]
                    {
                        $"{TextResources.APP_StringKeys_Validation_File_Format}, {invalidFileEx.Message}"
                    }
                });
                break;

            default:
                httpStatusCode = HttpStatusCode.InternalServerError;

                result = JsonConvert.SerializeObject(new ApiResponse()
                {
                    Success = false,
                    Data = null,
                    Message = new[]
                    {
                $"{TextResources.APP_StringKeys_Error_Unexpected}, {exception.Message}"
            }
                });
                break;
        }

        result = JsonConvert.SerializeObject(new ApiResponse()
        {
            Success = false,
            Message = [exception.Message],
            Data = null
        });

        context.Response.StatusCode = (int)httpStatusCode;

        return context.Response.WriteAsync(result);
    }
}