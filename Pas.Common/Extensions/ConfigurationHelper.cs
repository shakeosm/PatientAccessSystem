using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Pas.Common.Extensions
{
    public static class ConfigurationHelper
    {
        public static IConfiguration ConfigureEnvironment(string environment, string basePath)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static T GetConfig<T>(this IServiceCollection services)
            where T : class, new()
        {
            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetRequiredService<IOptions<T>>().Value;
            return config;
        }

        public static T GetConfig<T>(this ServiceProvider serviceProvider)
            where T : class, new()
        {
            var config = serviceProvider.GetRequiredService<IOptions<T>>().Value;
            return config;
        }
    }
}