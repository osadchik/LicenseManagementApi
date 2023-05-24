using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
        public Task ProcessMessageAsync(SQSMessage sqsMessage)
        {
            if (sqsMessage is null) throw new ArgumentNullException(nameof(sqsMessage));

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public async Task<JObject> ProcessSqsRecordsAsync(SQSEvent sqsEvent)
        {
            foreach (SQSMessage message in sqsEvent.Records)
            {
                _logger.LogDebug("Started processing SQSMessage {MessageId}", message.MessageId);
                await _circuitBreakingService.ExecuteAsync(message, ProcessMessageAsync);
                _logger.LogInformation("Processed SQSMessage {MessageId}", message.MessageId);
            }

            return null;
        }
    }
}