using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions
{
    public static class DatabaseConfigurationExtensions
    {
        public static IServiceCollection ConfigureDynamoDB(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDefaultAWSOptions(configuration.GetAWSOptions())
                .AddAWSService<IAmazonDynamoDB>()
                .AddTransient<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
