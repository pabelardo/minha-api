using Microsoft.EntityFrameworkCore;
using MyApiV8.Infra.Data.Context;

namespace MyApiV8.Configuration;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<MyDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return builder;
    }
}
