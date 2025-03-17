using MyApiV8.Domain.Configuration;

namespace MyApiV8.Configuration;

public static class EnvironmentConfig
{
    public static WebApplicationBuilder AddEnvironmentConfig(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        var config = configuration.Build();

        ConfigurationHelper.Init(config);

        ConfigurationHelper.SetEnvironment(config);

        return builder;
    }
}
