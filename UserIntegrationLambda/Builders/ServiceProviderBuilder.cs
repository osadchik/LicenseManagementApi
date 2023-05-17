using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UserIntegrationLambda.Options;

namespace UserIntegrationLambda.Builders
{
    /// <summary>
    /// Provides a creation service of <see cref="IServiceProvider"/> instance.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceProviderBuilder
    {
        private IConfiguration _configuration;

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

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.ConfigureLogging();
            AddLambdaParameters(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private void AddLambdaParameters(IServiceCollection serviceCollection)
        {
            // Event Bridge Configuration
            var eventBridgeConfiguration = new EventBridgeOptions
            {
                RuleName = _configuration["EVENTBRIDGE_Rule_Name"] ?? throw new ArgumentNullException(nameof(_configuration))
            };

            serviceCollection.AddSingleton(eventBridgeConfiguration);
        }

    }
}
