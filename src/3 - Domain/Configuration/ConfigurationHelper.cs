using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyApiV8.Domain.Configuration;

public static class ConfigurationHelper
{
    private static IConfiguration _config;
    public static IServiceProvider _serviceProvider;
    private static bool _dev;
    private static bool _prod;
    private static bool _localHost;

    public static void Init(IConfiguration config) => _config = config;

    private static void SetConfig(IConfiguration config) => _config = config;

    public static void InitServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public static void SetEnvironment(IConfiguration config)
    {
        _dev = _config["Environment"] == "Development";
        _prod = _config["Environment"] == "Production";
        _localHost = _config["Environment"] == "LocalHost";
    }

    public static void SetEnvironment(bool dev, bool prod, bool localHost)
    {
        _dev = dev;
        _prod = prod;
        _localHost = localHost;
    }

    public static string GetValue(string tag) => _config.GetValue<string>(tag);

    public static string GetValueSection(string section, string tag) => _config.GetSection(section).GetValue<string>(tag);

    public static T GetSection<T>(string section) => _config.GetSection(section).Get<T>();

    public static T GetService<T>() => _serviceProvider.GetService<T>();

    public static object GetService(Type type) => _serviceProvider.GetService(type);

    public static string GetDefaultConnectionString() => _config.GetConnectionString("DefaultConnection");

    public static bool IsDevelopment() => _dev;
    public static bool IsProduction() => _prod;
    public static bool IsLocalHost() => _localHost;

    public static string GetEnvironment()
    {
        if (_dev) return "Development";
        if (_prod) return "Production";
        if (_localHost) return "LocalHost";

        return "LocalHost";
    }

    public static string GetUrlApi()
    {
        if( _dev) return _config["UrlApiDev"];
        if (_prod) return _config["UrlApiProd"];
        
        return _config["UrlApiLocalHost"];
    }
}