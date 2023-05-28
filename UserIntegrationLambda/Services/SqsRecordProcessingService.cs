using Amazon.Lambda.SQSEvents;
using Common.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserIntegrationLambda.Interfaces;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace UserIntegrationLambda.Services
{
    /// <summary>
    /// SQS record processing service.
    /// </summary>
    public class SqsRecordProcessingService : ISqsRecordProcessingService
    {
        private readonly ICircuitBreakingService _circuitBreakingService;
        private readonly IUserIntegrationHandler _userIntegrationHandler;
        private readonly ILogger<SqsRecordProcessingService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SqsRecordProcessingService"/> class.
        /// </summary>
        /// <param name="circuitBreakingService"></param>
        /// <param name="userIntegrationHandler"></param>
        /// <param name="logger">Logger instance.</param>
        public SqsRecordProcessingService(ICircuitBreakingService circuitBreakingService, IUserIntegrationHandler userIntegrationHandler, ILogger<SqsRecordProcessingService> logger)
        {
            _circuitBreakingService = circuitBreakingService;
            _userIntegrationHandler = userIntegrationHandler;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ProcessMessageAsync(SQSMessage sqsMessage)
        {
            if (sqsMessage is null) throw new ArgumentNullException(nameof(sqsMessage));

            BaseMessage<UserDto>? messageBody = JsonConvert.DeserializeObject<BaseMessage<UserDto>>(sqsMessage.Body);
            _logger.LogDebug("Deserialized SQS message body into: {@messageBody}", messageBody);

            if (messageBody is null)
            {
                throw new ArgumentNullException("SQS Message body is empty", nameof(sqsMessage));
            }

            if (string.IsNullOrEmpty(messageBody.Action.ToString()))
            {
                throw new ArgumentException("Action is not defined", nameof(sqsMessage));
            }

            _logger.LogDebug("Executing {action} for User ID: {uuid}", messageBody.Action, messageBody.EntityId);
            Dictionary<string, Func<Task>> defaultActions = LoadDefaultActions(messageBody);

            if (!defaultActions.ContainsKey(messageBody.Action.ToString()))
            {
                string supportedActions = string.Join(',', defaultActions.Select(a => a.Key));
                _logger.LogWarning("Action {action} is not supported. See suported actions: [{supportedActions}]", messageBody.Action, supportedActions);
                return;
            }

            Func<Task> function = defaultActions[messageBody.Action.ToString()];
            await function();

            _logger.LogInformation("{action} successfully executed for user: {uuid}", messageBody.Action, messageBody.EntityId);
        }

        /// <inheritdoc/>
        public async Task ProcessSqsRecordsAsync(SQSEvent sqsEvent)
        {
            foreach (SQSMessage message in sqsEvent.Records)
            {
                _logger.LogDebug("Started processing SQSMessage {MessageId}", message.MessageId);
                await _circuitBreakingService.ExecuteAsync(message, ProcessMessageAsync);
                _logger.LogInformation("Processed SQSMessage {MessageId}", message.MessageId);
            }
        }

        private Dictionary<string, Func<Task>> LoadDefaultActions(BaseMessage<UserDto> message)
        {
            var defaultActions = new Dictionary<string, Func<Task>>()
            {
                { "Create", () => _userIntegrationHandler.CreateUser(message.Content) },
                { "Update", () => _userIntegrationHandler.UpdateUser(message.Content) },
                { "Delete", () => _userIntegrationHandler.DeleteUser(Guid.Parse(message.EntityId)) },
            };

            return defaultActions;
        }
    }
}