using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Interfaces.Models;
using System.Text.Json.Serialization;

namespace MyApiV8.Domain.Models;

public class ApplicationResponse : IApplicationResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("errors")]
    public IEnumerable<string> Errors { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; }

    [JsonPropertyName("file")]
    public string Base64 { get; set; }

    [JsonIgnore]
    public string InnerException { get; set; } = string.Empty;

    [JsonIgnore]
    public string StackTrace { get; set; } = string.Empty;

    public ApplicationResponse() 
    {
        Errors = [];
        Data = string.Empty;
        Base64 = null;
    }

    public ApplicationResponse(
        bool success,
        int statusCode,
        string message = null,
        IEnumerable<string> errors = null,
        object data = null,
        string base64 = null,
        string innerException = null,
        string stackTrace = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
        Data = data;
        Base64 = base64;
        InnerException = innerException;
        StackTrace = stackTrace;
    }

    public async Task<IApplicationResponse> Ok(object data = null, string message = null, string base64 = null)
    {
        Success = true;
        Message = message ?? string.Empty;
        StatusCode = (int)StatusCodeEnum.Ok;
        Data = data;
        Errors = [];
        Base64 = base64 ?? string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IApplicationResponse> BadRequest(IEnumerable<string> errorMessages = null, string message = null)
    {
        Success = false;
        Message = message ?? string.Empty;
        StatusCode = (int)StatusCodeEnum.BadRequest;
        Data = "";
        Errors = errorMessages ?? [];
        Base64 = string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IApplicationResponse> NotFound(IEnumerable<string> errorMessages = null, string message = null)
    {
        Success = false;
        Message = message ?? string.Empty;
        StatusCode = (int)StatusCodeEnum.NotFound;
        Data = "";
        Errors = errorMessages ?? ["Object not found."];
        Base64 = string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IApplicationResponse> Unauthorized(string errorMessage = null)
    {
        string message = "Não autorizado.";

        Success = false;
        Message = string.Empty;
        StatusCode = (int)StatusCodeEnum.Unauthorized;
        Data = "";
        Errors = [errorMessage ?? message];
        Base64 = string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IApplicationResponse> InternalServerError(Exception ex, string message = null)
    {
        Success = false;
        Message = message ?? string.Empty;
        StatusCode = (int)StatusCodeEnum.InternalServerError;
        Data = "";
        Errors = [ex.Message];
        InnerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
        StackTrace = ex.StackTrace ?? string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IApplicationResponse> CustomApplicationResponse(
        bool valid,
        int statusCode,
        string message = null,
        IEnumerable<string> errorMessages = null,
        object data = null,
        string base64 = null)
    {
        Success = valid;
        Message = message ?? string.Empty;
        StatusCode = statusCode;
        Errors = errorMessages;
        Data = data;
        Base64 = base64;
        
        return await Task.FromResult(this);
    }

    public Task<ApplicationResponse> CreateApplicationResponse(
        bool valid,
        int statusCode,
        string message = null,
        IEnumerable<string> errorMessages = null,
        object data = null,
        string base64 = null,
        string innerException = null,
        string stackTrace = null)
    {
        return Task.FromResult(new ApplicationResponse(valid, statusCode, message ?? string.Empty, errorMessages ?? [], data ?? string.Empty, base64 ?? string.Empty, innerException ?? string.Empty, stackTrace ?? string.Empty));
    }
}