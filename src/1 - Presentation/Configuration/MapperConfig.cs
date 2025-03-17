namespace MyApiV8.Configuration;

public static class MapperConfig
{
    public static WebApplicationBuilder AddAutoMapperConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return builder;
    }
}
