namespace MyApiV8.Configuration;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Development", corsPolicyBuilder =>
                        corsPolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());

            options.AddPolicy("Production", corsPolicyBuilder =>
                        corsPolicyBuilder
                            .WithOrigins("https://localhost:9000")
                            .WithMethods("POST")
                            .AllowAnyHeader());
        });

        return builder;
    }
}
