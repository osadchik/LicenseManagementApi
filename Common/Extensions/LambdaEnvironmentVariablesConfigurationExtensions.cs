using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    /// <summary>
    /// Extension class to work with lambda environment variables.
    /// </summary>
    public static class LambdaEnvironmentVariablesConfigurationExtensions
    {
        /// <summary>
        /// Injects lambda parameters in the DI container.
        /// </summary>
        /// <typeparam name="TOptions">Type of options being configured.</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureLambdaVariables<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class
        {
            services.Configure<TOptions>(configuration.GetSection("Parameters"));

            return services;
        }
    }
}
