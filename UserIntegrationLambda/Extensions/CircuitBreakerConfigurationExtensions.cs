using Amazon.EventBridge;
using Amazon.Lambda;
using Amazon.SQS;
using Common.Interfaces;
using Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using UserIntegrationLambda.Options;
using UserIntegrationLambda.Repository;
using UserIntegrationLambda.Services.CircuitBreaker;

namespace UserIntegrationLambda.Extensions
{
    public static class CircuitBreakerConfigurationExtensions
    {
        public static IServiceCollection ConfigureCircuitBreakerServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var temp = Environment.GetEnvironmentVariable("EVENTBRIDGE_Rule_Name");
            serviceCollection.Configure<CircuitBreakerOptions>(configuration.GetSection("CircuitBreaker"));

            serviceCollection.AddAWSService<IAmazonEventBridge>();
            serviceCollection.AddAWSService<IAmazonSQS>();

            serviceCollection.AddScoped<ICircuitStateRepository, CircuitStateRepository>();
            serviceCollection.AddScoped<ICircuitStateToDatabaseDtoMapper, CircuitStateToDatabaseDtoMapper>();

            serviceCollection
                .AddScoped<IEventBridgeSchedulerService, EventBridgeSchedulerService>()
                .AddScoped<IAmazonLambda, AmazonLambdaClient>()
                .AddScoped<ISqsClient, SqsClient>()
                .AddScoped<IEventSourceMappingClient, SqsEventSourceMappingClient>();

            serviceCollection.AddScoped<ICircuitBreakingService, CircuitBreakingService>();

            return serviceCollection;
        }
    }
}
