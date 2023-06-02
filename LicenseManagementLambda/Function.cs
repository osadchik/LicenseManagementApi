using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Amazon.Lambda.SQSEvents;
using Common.Interfaces;
using LicenseManagementLambda.Builders;
using Newtonsoft.Json.Linq;

namespace LicenseManagementLambda
{
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
            //_serviceProvider = new ServiceProviderBuilder().Build(services);
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(JObject input, ILambdaContext lambdaContext)
        {
            var sqsEvent = input.ToObject<SQSEvent>();
            var request = input.ToObject<APIGatewayProxyRequest>();

            if (sqsEvent.Records is not null)
            {
                var service = _serviceProvider.GetRequiredService<ISqsEventProcessingService>();

                await service.ProcessAsync(input);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                };
            }
            if (request.Resource is not null)
            {
                LambdaEntryPoint lambdaEntryPoint = new();
                return await lambdaEntryPoint.FunctionHandlerAsync(request, lambdaContext);
            }
            else
            {
                throw new ArgumentException("Input type is unknown and can't be processed.");
            }
        }
    }
}
