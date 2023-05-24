using Common.Extensions;
using Common.Interfaces;
using Common.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UserIntegrationLambda.Extensions;
using UserIntegrationLambda.InputProcessStrategies;
using UserIntegrationLambda.Interfaces;
using UserIntegrationLambda.Services;

namespace UserIntegrationLambda.Builders
{
    /// <summary>
    /// Provides a creation service of <see cref="IServiceProvider"/> instance.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceProviderBuilder
    {
        private readonly IConfiguration _configuration;

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

            serviceCollection.ConfigureLogging();

            serviceCollection.ConfigureDynamoDB(_configuration);
            serviceCollection.ConfigureCircuitBreakerServices(_configuration);

            AddServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static void AddServices(IServiceCollection services)
        {
            var lambdaContextAccessor = new LambdaContextAccessor();
            services.AddSingleton<ILambdaContextAccessor>(lambdaContextAccessor);
            services.AddSingleton(lambdaContextAccessor);

            services.AddScoped<ISqsEventProcessingService, SqsEventProcessingService>();
            services.AddScoped<IDataHandlerStrategySelector, DataHandlerStrategySelector>();
            services.AddScoped<ISqsRecordProcessingService, SqsRecordProcessingService>();
            services.AddScoped<IUserIntegrationHandler, UserIntegrationHandler>();

            services
                .AddScoped<IDataHandlerStrategy, UserIntegrationHandlerStrategy>()
                .AddScoped<IDataHandlerStrategy, CircuitBreakerMessageHandlerStrategy>();
        }
    }
}
