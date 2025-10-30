using src.Utilities;

namespace src.Extensions;

public static class ConfigurationExtension
{
    public static ApplicationConfig LoadConfiguration(this IConfiguration source) => source.Get<ApplicationConfig>();
}