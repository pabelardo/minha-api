using Microsoft.Extensions.Options;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Application.Services;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Interfaces.Services;
using MyApiV8.Domain.Interfaces.Services.External;
using MyApiV8.Domain.Models;
using MyApiV8.Domain.Notifications;
using MyApiV8.Domain.Services;
using MyApiV8.Extensions;
using MyApiV8.Infra.CrossCutting.Services.External;
using MyApiV8.Infra.Data.Context;
using MyApiV8.Infra.Data.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace MyApiV8.Configuration;

public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder ResolveDependencies(this WebApplicationBuilder builder)
    {
        #region Repositories

        builder.Services.AddScoped<MyDbContext>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();

        #endregion

        #region Services

        builder.Services.AddScoped<ISupplierService, SupplierService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IViaCepService, ViaCepService>();

        #endregion

        #region Application

        builder.Services.AddScoped<ISupplierAppService, SupplierAppService>();
        builder.Services.AddScoped<IProductAppService, ProductAppService>();
        builder.Services.AddScoped<IViaCepAppService, ViaCepAppService>();

        #endregion

        builder.Services.AddScoped<IApplicationResponse, ApplicationResponse>();
        builder.Services.AddScoped<INotifier, Notifier>();
        builder.Services.AddScoped<INotification, Notification>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<IUser, AspNetUser>();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        ConfigurationHelper.InitServiceProvider(builder.Services.BuildServiceProvider());

        return builder;
    }
}
