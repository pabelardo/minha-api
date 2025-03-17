using Microsoft.Extensions.Logging;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Services.External;
using MyApiV8.Domain.Services.Base;
using System.Text;
using System.Text.Json;

namespace MyApiV8.Infra.CrossCutting.Services;

public class HttpClientService(
    HttpClient httpClient,
    ILogger<HttpClientService> logger,
    INotifier notifier,
    INotification notification) : BaseService(notifier, notification), IHttpClientService
{
    //private readonly AsyncPolicyWrap<HttpResponseMessage> _resiliencePolicy;
    //// Configuring resilience policies
    //var retryPolicy = HttpPolicyExtensions
    //    .HandleTransientHttpError()
    //    .Or<TimeoutRejectedException>()
    //    .WaitAndRetryAsync(
    //        3, // Number of attempts
    //        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    //        (outcome, timespan, retryCount, context) =>
    //        {
    //            var exceptionMessage = outcome?.Exception?.Message ?? "Unknown error";
    //            _logger.LogWarning("Attempt {RetryCount} failed with the following error message: {ExceptionMessage}. Retentando em {TimespanSeconds} segundos.", retryCount, exceptionMessage, timespan.TotalSeconds);
    //        }
    //    );

    //var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
    //    TimeSpan.FromSeconds(30), // Timeout 30 sec
    //    TimeoutStrategy.Pessimistic);

    //var circuitBreakerPolicy = Policy<HttpResponseMessage>
    //    .Handle<HttpRequestException>()
    //    .CircuitBreakerAsync(
    //        handledEventsAllowedBeforeBreaking: 2, // Number of consecutive failures to open the circuit
    //        durationOfBreak: TimeSpan.FromSeconds(30), // Time to keep the circuit open
    //        onBreak: (outcome, timespan) =>
    //        {
    //            _logger.LogWarning("Circuit open for {TimespanSeconds} seconds due to {ExceptionMessage}", timespan.TotalSeconds, outcome.Exception.Message);
    //        },
    //        onReset: () => _logger.LogInformation("Closed circuit. Resuming operations."),
    //        onHalfOpen: () => _logger.LogInformation("Circuit in test status (half-open)."));

    //_resiliencePolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy, circuitBreakerPolicy);

    public async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string url,
        object? body = null,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        // Build URL with query parameters(query string)
        if (queryParams != null && queryParams.Count != 0)
        {
            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            url = $"{url}?{query}";
        }

        // Create the HTTP request
        var request = new HttpRequestMessage(method, url);

        // Add the request body(for POST, PUT, etc.)
        if (body != null)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            request.Content = jsonContent;
        }

        // Add custom headers (if provided)
        if (headers != null)
            foreach (var header in headers)
                request.Headers.Add(header.Key, header.Value);

        // Send the request and get the response
        logger.LogInformation("Sending [{Method}] request to {Url}.", method.Method, url);

        HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            logger.LogError("Request to [{Url}] failed with status code {StatusCode}.", url, response.StatusCode);
        else
            logger.LogInformation("Request to [{Url}] successful.", url);

        try
        {
            response.EnsureSuccessStatusCode(); // Throws exception if status is not success(2xx)
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Request to [{Url}] failed with exception.", url);
            Notify($"Request to [{url}] failed with status code {response.StatusCode}. Details: {ex.Message}");
        }

        return response;
    }

    public async Task<T?> SendAsync<T>(
        HttpMethod method,
        string url,
        object? body = null,
        Dictionary<string, string>? headers = null,
        Dictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Sending [{Method}] request to {Url}.", method.Method, url);

        var response = await SendAsync(method, url, body, headers, queryParams, cancellationToken);

        if (!response.IsSuccessStatusCode)
            logger.LogError("Request to [{Url}] failed with status code {StatusCode}.", url, response.StatusCode);

        try
        {
            response.EnsureSuccessStatusCode(); // Throws exception if status is not success(2xx)
        }
        catch (HttpRequestException ex)
        {
            Notify($"Request to [{url}] failed with status code {response.StatusCode}. Details: {ex.Message}");
        }

        logger.LogInformation("Request to [{Url}] successful.", url);

        // Read response content as string
        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        // Convert JSON to expected type
        return JsonSerializer.Deserialize<T>(jsonResponse);
    }
}
