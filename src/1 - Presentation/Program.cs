using Asp.Versioning.ApiExplorer;
using MyApiV8.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddEnvironmentConfig()
    .AddDbContextConfig()
    .AddIdentityConfig()
    .AddAutoMapperConfig()
    .AddApiConfig()
    .AddCorsConfig()
    .AddSwaggerConfig()
    .AddFluentValidationConfig()
    .ResolveDependencies()
    .AddHttpConfig()
    .AddHealthCheckConfig();

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseApiConfig(builder.Environment);

app.UseSwaggerConfig(apiVersionDescriptionProvider, builder.Environment);

app.AddHealthCheckConfig();

app.Run();
