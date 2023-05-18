using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    /// <summary>
    /// Sns configuration extension class.
    /// </summary>
    public static class SnsConfigurationExtensions
    {
        /// <summary>
        /// Configures sns in the DI container.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureSns(this IServiceCollection services)
        {
            return services
                .AddTransient<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
        } 
    }
}
