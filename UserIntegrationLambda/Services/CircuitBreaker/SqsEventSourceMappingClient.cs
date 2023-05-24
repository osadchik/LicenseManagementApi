using Amazon.Lambda;
using Amazon.Lambda.Model;
using Common.Entities;
using Common.Interfaces;
using Common.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net;

namespace UserIntegrationLambda.Services.CircuitBreaker
{
    /// <summary>
    /// SQS event source mapping switcher (on/off).
    /// </summary>
    public class SqsEventSourceMappingClient : IEventSourceMappingClient
    {
        private readonly IAmazonLambda _amazonLambdaClient;
        private readonly ILambdaContextAccessor _lambdaContextAccessor;
        private readonly ILogger<SqsEventSourceMappingClient> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SqsEventSourceMappingClient"/> class.
        /// </summary>
        /// <param name="amazonLambdaClient"><see cref="IAmazonLambda"/></param>
        /// <param name="lambdaContextAccessor"><see cref="ILambdaContextAccessor"/></param>
        /// <param name="logger">Logger instance.</param>
        public SqsEventSourceMappingClient(IAmazonLambda amazonLambdaClient, ILambdaContextAccessor lambdaContextAccessor, ILogger<SqsEventSourceMappingClient> logger)
        {
            _amazonLambdaClient = amazonLambdaClient;
            _lambdaContextAccessor = lambdaContextAccessor;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task DisableEventSourceMappingAsync()
        {
            _logger.LogDebug("Trying to disable EventSourceMapping for {FunctionName} function", _lambdaContextAccessor.Context.FunctionName);
            EventSourceMappingConfiguration currentState = await GetMappingConfiguration(_lambdaContextAccessor.Context.FunctionName);

            if (currentState.State == AwsEventSourceMappingState.Enabling)
            {
                throw new EventSourceMappingStateTransitionException($"EventSourceMapping can't be disabled as it's in {currentState.State} state.");
            }

            if (currentState.State is not (AwsEventSourceMappingState.Disabling or AwsEventSourceMappingState.Disabled))
            {
                await SwitchEventSourceMappingAsync(currentState, false);
            }
        }

        /// <inheritdoc/>
        public async Task EnableEventSourceMappingAsync()
        {
            _logger.LogDebug("Trying to enable EventSourceMapping for {FunctionName} function", _lambdaContextAccessor.Context.FunctionName);
            EventSourceMappingConfiguration currentState = await GetMappingConfiguration(_lambdaContextAccessor.Context.FunctionName);

            if (currentState.State == AwsEventSourceMappingState.Disabling)
            {
                throw new EventSourceMappingStateTransitionException($"EventSourceMapping can't be enabled as it's in {currentState.State} state.");
            }

            if (currentState.State is not (AwsEventSourceMappingState.Enabling or AwsEventSourceMappingState.Enabled))
            {
                await SwitchEventSourceMappingAsync(currentState, true);
            }
        }

        private async Task SwitchEventSourceMappingAsync(EventSourceMappingConfiguration currentState, bool enable)
        {
            if (currentState == null) throw new ArgumentNullException(nameof(currentState));

            UpdateEventSourceMappingResponse updateResponse =
                await _amazonLambdaClient.UpdateEventSourceMappingAsync(new UpdateEventSourceMappingRequest
                {
                    UUID = currentState.UUID,
                    Enabled = enable,
                });

            string targetState = enable ? "enable" : "disable";
            if (updateResponse.HttpStatusCode == HttpStatusCode.Accepted)
            {
                _logger.LogInformation("Request to {targetState} {arn} EventSourceMapping is accepted", targetState, currentState.EventSourceArn);
            }
            else
            {
                throw new EventSourceMappingStateTransitionException($"Request to {targetState} {currentState.EventSourceArn} EventSourceMapping responded with {updateResponse.HttpStatusCode}");
            }
        }

        private async Task<EventSourceMappingConfiguration> GetMappingConfiguration(string functionName)
        {
            ListEventSourceMappingsResponse listEventSourceMappings =
                await _amazonLambdaClient.ListEventSourceMappingsAsync(new ListEventSourceMappingsRequest { FunctionName = functionName });
            EventSourceMappingConfiguration? mappingConfiguration =
                listEventSourceMappings.EventSourceMappings.FirstOrDefault(m => SqsEventSourceArn.IsMatch(m.EventSourceArn));
            return mappingConfiguration ?? throw new EventSourceMappingStateTransitionException($"Failed to find SQS EventSourceMapping for {functionName}");
        }
    }
}
