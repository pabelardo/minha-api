using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Interfaces.Models;
using System.Net;
using System.Text.Json;

namespace MyApiV8.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApplicationResponse _applicationResponse;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext, IApplicationResponse applicationResponse)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, applicationResponse);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, IApplicationResponse applicationResponse)
    {
        //exception.Ship(context);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        string innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
        string stackTrace = ex.StackTrace ?? "";

        var appResponse = await applicationResponse.CreateApplicationResponse(
            false,
            (int)StatusCodeEnum.InternalServerError,
            $"Ops :( ... Something went wrong.",
            [ex.Message],
            null,
            null,
            innerException,
            stackTrace);

        await context.Response.WriteAsync(JsonSerializer.Serialize(appResponse));
    }
}
