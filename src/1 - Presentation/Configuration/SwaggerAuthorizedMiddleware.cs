using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Interfaces.Models;
using System.Net;
using System.Text.Json;

namespace MyApiV8.Configuration;

public class SwaggerAuthorizedMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerAuthorizedMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, IApplicationResponse applicationResponse)
    {
        try
        {
            if (context.Request.Path.StartsWithSegments("/swagger")
                && !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                string errorMessage = "You are not authorized to access this API. Please contact your project administrator";
                await HandleResponseAsync(context, errorMessage, applicationResponse);
                return;
            }

            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, applicationResponse);
        }
    }

    private async Task HandleResponseAsync(HttpContext context, string errorMessage, IApplicationResponse applicationResponse)
    {
        context.Response.ContentType = "application/json";

        var appResponse = await applicationResponse.CreateApplicationResponse(
            false,
            context.Response.StatusCode,
            "Ops :( ... Something went wrong.",
            [errorMessage],
            null,
            null,
            null,
            null);

        await context.Response.WriteAsync(JsonSerializer.Serialize(appResponse));
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
            $"Ops :( ... Something went wrong. Details: {ex.Message}",
            null,
            null,
            null,
            innerException,
            stackTrace);

        await context.Response.WriteAsync(JsonSerializer.Serialize(appResponse));
    }
}