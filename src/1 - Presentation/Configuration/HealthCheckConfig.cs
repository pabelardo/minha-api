using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApiV8.Domain.Configuration;
using MyApiV8.Extensions;

namespace MyApiV8.Configuration;

public static class HealthCheckConfig
{
    public static void AddHealthCheckConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(name: "Sql Server", connectionString: ConfigurationHelper.GetDefaultConnectionString(),
                tags: ["db", "data"], failureStatus: HealthStatus.Unhealthy)
            .AddCheck<HealthCheckApiExtensions>("API Health Check", tags: ["apihc"])
            .AddGCInfoCheck("GCInfo", HealthStatus.Unhealthy, ["gc"]);

        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("HealthCheck API", "/hc");
        })
        .AddInMemoryStorage();
    }

    public static void AddHealthCheckConfig(this WebApplication app)
    {
        app.MapHealthChecks("/hc", new()
        {
            //Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecksUI(options =>
        {
            options.UIPath = "/hc-ui";
            options.ApiPath = "/hc-ui-api";
            options.ResourcesPath = "/hc-ui-resources";
            options.AddCustomStylesheet("wwwroot/css/hc/hcStyle.css");
        });
    }
}
