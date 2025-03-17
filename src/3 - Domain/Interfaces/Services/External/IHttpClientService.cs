namespace MyApiV8.Domain.Interfaces.Services.External;

public interface IHttpClientService
{
    Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string url,
        object body = null,
        Dictionary<string, string> headers = null,
        Dictionary<string, string> queryParams = null,
        CancellationToken cancellationToken = default);

    Task<T?> SendAsync<T>(
        HttpMethod method,
        string url,
        object body = null,
        Dictionary<string, string> headers = null,
        Dictionary<string, string> queryParams = null,
        CancellationToken cancellationToken = default);
}