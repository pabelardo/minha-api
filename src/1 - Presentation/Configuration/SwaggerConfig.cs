using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace MyApiV8.Configuration;

public static class SwaggerConfig
{
    public static WebApplicationBuilder AddSwaggerConfig(this WebApplicationBuilder builder)
    {
        //builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.OperationFilter<SwaggerDefaultValues>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    public static void UseSwaggerConfig(this WebApplication app, IApiVersionDescriptionProvider provider, IWebHostEnvironment environment)
    {
        if (environment.IsProduction())
            app.UseMiddleware<SwaggerAuthorizedMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                //options.RoutePrefix = string.Empty;
            });
    }
}
