using FluentValidation;
using MyApiV8.Application.Validations.FluentValidation;
using MyApiV8.Domain.Validations.FluentValidation;

namespace MyApiV8.Configuration;

public static class FluentValidationConfig
{
    public static WebApplicationBuilder AddFluentValidationConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<LoginUserValidation>();
        builder.Services.AddValidatorsFromAssemblyContaining<SupplierValidation>();

        return builder;
    }
}
