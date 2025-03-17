using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Interfaces.Services.External;

namespace MyApiV8.Extensions;

public class HealthCheckApiExtensions(IHttpClientService httpClientService) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        string urlBase = ConfigurationHelper.GetValue("UrlApi");
        string urlProducts = ConfigurationHelper.GetValue("UrlTest");

        var response = await httpClientService.SendAsync(HttpMethod.Get, string.Concat(urlBase, urlProducts), cancellationToken: cancellationToken);

        if (response.IsSuccessStatusCode)
            return HealthCheckResult.Healthy("API is up and running.");

        return HealthCheckResult.Unhealthy("API is down.");
    }
}
