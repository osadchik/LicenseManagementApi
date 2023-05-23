using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UserIntegrationLambda.InputProcessStrategies;
using UserIntegrationLambda.Interfaces;
using UserIntegrationLambda.Options;
using UserIntegrationLambda.Services;

namespace UserIntegrationLambda.Builders
{
    /// <summary>
    /// Provides a creation service of <see cref="IServiceProvider"/> instance.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceProviderBuilder
    {
        private IConfiguration _configuration;

        public ServiceProviderBuilder()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Creates a <see cref="IServiceProvider"/> containing services provided for solution.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns><see cref="ServiceProvider"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceProvider Build(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            var environmentConfiguration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            serviceCollection.ConfigureLogging();

            AddServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ISqsEventProcessingService, SqsEventProcessingService>();
            services.AddScoped<IDataHandlerStrategySelector, DataHandlerStrategySelector>();
            services.AddScoped<ISqsRecordProcessingService, SqsRecordProcessingService>();

            services
                .AddScoped<IDataHandlerStrategy, UserIntegrationHandlerStrategy>()
                .AddScoped<IDataHandlerStrategy, CircuitBreakerMessageHandlerStrategy>();
        }
    }
}
