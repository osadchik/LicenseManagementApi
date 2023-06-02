using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Common.Interfaces;
using Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using UserIntegrationLambda.Builders;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]
namespace UserIntegrationLambda;

/// <summary>
/// Lambda function class.
/// </summary>
[ExcludeFromCodeCoverage]
public class Function
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        var services = new ServiceCollection();
        _serviceProvider = new ServiceProviderBuilder().Build(services);
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(JObject input, ILambdaContext context)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(context);

        var logger = _serviceProvider.GetRequiredService<ILogger<Function>>();

        try
        { 
            var lambdaContextAccessor = _serviceProvider.GetRequiredService<LambdaContextAccessor>();
            lambdaContextAccessor.Context = context;

            logger.LogInformation("Received SQS Event: {evnt}", input.ToString());

            var sqsEventProcessingService = _serviceProvider.GetRequiredService<ISqsEventProcessingService>();

            await sqsEventProcessingService.ProcessAsync(input);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An unhandled error has occured. Check the exception details for further details.");
            throw;
        }

    }
}