using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MyApiV8.Extensions;

namespace MyApiV8.Configuration;

public static class ApiConfig
{
    public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true; //It is used to avoid returning the ModelState validation error before hitting the controller
            });

        builder.Services
            .AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //When there is no version 1, 2 or 3... it will assume the default version
                options.DefaultApiVersion = new ApiVersion(1, 0); //This is the default version 
                options.ReportApiVersions = true; //It aims to alert you that there is a more updated version of the API
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true; //If it is not specified which version you want to start the API with, it plays in the first version
            });

        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddMemoryCache();

        builder.Services.AddJwksManager().UseJwtValidation();

        builder.Services.AddLogging();

        return builder;
    }

    public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors("Development");
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseCors("Production");
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.UseStaticFiles();
    }
}
