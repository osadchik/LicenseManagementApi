using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json.Linq;

namespace LicenseManagementLambda
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function() { }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(JObject input, ILambdaContext lambdaContext)
        {
            var sqsEvent = input.ToObject<SQSEvent>();
            var request = input.ToObject<APIGatewayProxyRequest>();

            if (sqsEvent is not null)
            {
                // Process
            }

            if (request is not null)
            {
                LambdaEntryPoint lambdaEntryPoint = new();
                return await lambdaEntryPoint.FunctionHandlerAsync(request, lambdaContext);
            }

            throw new ArgumentException("Input type is unknown and can't be processed.");
        }
    }
}
