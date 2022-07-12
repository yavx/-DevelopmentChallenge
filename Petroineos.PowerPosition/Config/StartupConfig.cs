using Microsoft.Extensions.Configuration;
using Petroineos.PowerPosition.Config;

namespace Petroineos.PowerPosition.Config
{
    public static class StartupConfig
    {
        public static IConfigurationSection GetLoggerConfiguration(this IConfiguration config)
        {
            return config.GetSection("Logging");
        }

        public static T Get<T>(this IConfiguration config, string name)
        {
            return config
                 .GetSection(name)
                 .Get<T>();
        }
    }
}
