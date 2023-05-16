using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace UserManagementLambda.Extensions
{
    public static class DatabaseConfiguration
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
