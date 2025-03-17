using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApiV8.Data;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Models;
using MyApiV8.Models;
using System.Text;

namespace MyApiV8.Configuration;

public static class IdentityConfig
{
    public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConfigurationHelper.GetDefaultConnectionString()));

        var lockoutOptions = new LockoutOptions()
        {
            AllowedForNewUsers = true,
            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(120),
            MaxFailedAccessAttempts = 5
        };

        builder.Services
            .AddIdentity<User, IdentityRole<Guid>>(options => options.Lockout = lockoutOptions)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Pegando o Token e gerando a chave encodada
        var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(JwtSettingsSection);

        var jwtSettings = JwtSettingsSection.Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer
            };
        });

        return builder;
    }
}
