using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApiV8.Configuration;
using MyApiV8.Domain.Models;

namespace MyApiV8.Extensions;

public static class GCInfoHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddGCInfoCheck(
            this IHealthChecksBuilder builder,
            string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            long? thresholdInBytes = null)
    {
        // Registra uma verificação do tipo GCInfo.
        builder.AddCheck<GCInfoHealthCheckConfig>(name, failureStatus ?? HealthStatus.Degraded, tags);

        // Configure as opções nomeadas para passar o limite para a verificação.
        if (thresholdInBytes.HasValue)
        {
            builder.Services.Configure<GCInfoOptions>(name, options =>
            {
                options.Threshold = thresholdInBytes.Value;
            });
        }

        return builder;
    }
}
