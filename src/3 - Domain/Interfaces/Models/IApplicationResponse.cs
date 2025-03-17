using MyApiV8.Domain.Models;

namespace MyApiV8.Domain.Interfaces.Models;

public interface IApplicationResponse
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string Message { get; set; }
    string InnerException { get; set; }
    string StackTrace { get; set; }
    IEnumerable<string> Errors { get; set; }
    object Data { get; set; }
    string Base64 { get; set; }

    Task<IApplicationResponse> Ok(object data = null, string message = null, string base64 = null);
    Task<IApplicationResponse> BadRequest(IEnumerable<string> errorMessages = null, string message = null);
    Task<IApplicationResponse> Unauthorized(string errorMessage = null);
    Task<IApplicationResponse> NotFound(IEnumerable<string> errorMessages = null, string message = null);
    Task<IApplicationResponse> InternalServerError(Exception ex, string message = null);
    Task<IApplicationResponse> CustomApplicationResponse(
        bool valid,
        int statusCode,
        string message = null,
        IEnumerable<string> errorMessages = null,
        object data = null,
        string base64 = null);

    Task<ApplicationResponse> CreateApplicationResponse(
        bool valid,
        int statusCode,
        string message = null,
        IEnumerable<string> errorMessages = null,
        object data = null,
        string base64 = null,
        string innerException = null,
        string stackTrace = null);
}
