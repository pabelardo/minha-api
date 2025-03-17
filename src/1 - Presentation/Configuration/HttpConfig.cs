using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Interfaces.Services.External;
using MyApiV8.Infra.CrossCutting.Services;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System.Net;

namespace MyApiV8.Configuration;

public static class HttpConfig
{
    private static readonly ILogger<Program> _logger = ConfigurationHelper.GetService<ILogger<Program>>();

    public static WebApplicationBuilder AddHttpConfig(this WebApplicationBuilder builder)
    {
        //HttpClient registration with Polly combining retry, timeout and circuit breaker
        builder.Services.AddHttpClient<IHttpClientService, HttpClientService>()
            .ConfigureHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30); // Set Timeout
            })
            .AddPolicyHandler(GetCombinedPolicies());

        return builder;
    }


    // Set combined policies
    private static AsyncPolicyWrap<HttpResponseMessage> GetCombinedPolicies() =>
        Policy.WrapAsync(GetRetryPolicy(), GetTimeoutPolicy(), GetCircuitBreakerPolicy()); // Combinar Retry, Timeout e Circuit Breaker usando Polly.Wrap


    // Retry Policy (retry the request in case of temporary failures)
    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(
            3, 
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                var exceptionMessage = outcome?.Exception?.Message ?? "Unknown error";
                _logger.LogWarning("Attempt {RetryCount} failed with the following error message: {ExceptionMessage}. Retentando em {TimespanSeconds} segundos.", retryCount, exceptionMessage, timespan.TotalSeconds);
            });

    // Timeout Policy (set maximum waiting time for each request)
    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogWarning("Circuit open for {TimespanSeconds} seconds due to {ExceptionMessage}", timespan.TotalSeconds, outcome.Exception.Message);
                },
                onReset: () => _logger.LogInformation("Closed circuit. Resuming operations."),
                onHalfOpen: () => _logger.LogInformation("Circuit in test status (half-open)."));

    // Circuit Breaker Policy (to handle repeated failures)
    private static AsyncTimeoutPolicy<HttpResponseMessage> GetTimeoutPolicy() =>
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30), TimeoutStrategy.Pessimistic);
}
