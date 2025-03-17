using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MyApiV8.Domain.Models;

namespace MyApiV8.Configuration;

public class GCInfoHealthCheckConfig : IHealthCheck
{
    private readonly IOptionsMonitor<GCInfoOptions> _options;

    public GCInfoHealthCheckConfig(IOptionsMonitor<GCInfoOptions> options)
    {
        _options = options;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new())
    {
        var options = _options.Get(context.Registration.Name);

        // Este exemplo relatará o status degradado se o aplicativo estiver usando
        // mais do que a quantidade configurada de memória (1gb por padrão).
        // Além disso, incluímos algumas informações de GC nos diagnósticos relatados.
        var allocated = GC.GetTotalMemory(false);

        var data = new Dictionary<string, object>()
            {
                { "Allocated", allocated },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) },
            };

        //Relatar falha se a memória alocada for >= o limite.

        //Using context.Registration.FailureStatus significa que o desenvolvedor do aplicativo pode configurar
        // como eles querem que as falhas apareçam.
        var result = allocated >= options.Threshold ? context.Registration.FailureStatus : HealthStatus.Healthy;

        return Task.FromResult(new HealthCheckResult(
            result,
            description: "Checks if the GC is occupying >= 1gb of memory",
            data: data));
    }
}
