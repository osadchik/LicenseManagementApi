using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    /// <summary>
    /// DynamoDB configuration extension class.
    /// </summary>
    public static class DatabaseConfigurationExtensions
    {
        /// <summary>
        /// Configures dynamoDB services in the DI container.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureDynamoDB(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDefaultAWSOptions(configuration.GetAWSOptions())
                .AddAWSService<IAmazonDynamoDB>()
                .AddTransient<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
