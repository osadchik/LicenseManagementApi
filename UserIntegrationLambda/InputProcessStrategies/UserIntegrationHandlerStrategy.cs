using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// Processes user integration requests SQS messages.
    /// </summary>
    public class UserIntegrationHandlerStrategy : IDataHandlerStrategy
    {
        private readonly ISqsRecordProcessingService _sqsRecordProcessingService;
        private readonly ILogger<UserIntegrationHandlerStrategy> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UserIntegrationHandlerStrategy"/> class.
        /// </summary>
        /// <param name="sqsRecordProcessingService"></param>
        /// <param name="logger"></param>
        public UserIntegrationHandlerStrategy(ISqsRecordProcessingService sqsRecordProcessingService, ILogger<UserIntegrationHandlerStrategy> logger)
        {
            _sqsRecordProcessingService = sqsRecordProcessingService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public bool IsSuitable(JObject input)
        {
            return input.ContainsKey("Records");
        }

        /// <inheritdoc/>
        public async Task<JObject> ProcessInputAsync(JObject input)
        {
            var sqsEvent = input.ToObject<SQSEvent>();

            if (sqsEvent is null) throw new ArgumentNullException(nameof(input));
            _logger.LogDebug("Deserialized input into SQS Event: {@sqsEvent}", sqsEvent);

            return await _sqsRecordProcessingService.ProcessSqsRecordsAsync(sqsEvent);
        }
    }
}