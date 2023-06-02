using Amazon.Lambda.SQSEvents;
using Common.Constants;
using Common.Entities;
using Common.Interfaces;
using LicenseManagementLambda.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace LicenseManagementLambda.Services
{
    /// <summary>
    /// SQS Event processor.
    /// </summary>
    internal class SqsEventProcessingService : ISqsEventProcessingService
    {
        private readonly IProductEntitlementManagementService _entitlementService;
        private readonly ILogger<SqsEventProcessingService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SqsEventProcessingService"/> class.
        /// </summary>
        /// <param name="strategySelector"><see cref="IDataHandlerStrategySelector"/></param>
        /// <param name="logger">Logger instance.</param>
        public SqsEventProcessingService(IProductEntitlementManagementService entitlementService, ILogger<SqsEventProcessingService> logger)
        {
            _entitlementService = entitlementService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task ProcessAsync(JObject input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return ProcessInternalAsync(input);
        }

        private async Task ProcessInternalAsync(JObject input)
        {
            _logger.LogDebug("SQS message processing started. Message: {message}", input.ToString());
            var sqsEvent = input.ToObject<SQSEvent>();

            foreach (SQSMessage message in sqsEvent.Records)
            {
                _logger.LogDebug("Started processing SQSMessage {MessageId}", message.MessageId);
                var body = JObject.Parse(message.Body);
                var entityType = body["EntityType"].ToString();
                _logger.LogDebug("Entity type is {type}", entityType);

                switch (entityType)
                {
                    case EntityTypes.User:
                        BaseMessage<UserDto> userMessage = JsonConvert.DeserializeObject<BaseMessage<UserDto>>(message.Body);
                        _entitlementService.UpdateUserDetails(userMessage);
                        break;
                    case EntityTypes.Product:
                        BaseMessage<ProductDto> productMessage = JsonConvert.DeserializeObject<BaseMessage<ProductDto>>(message.Body);
                        _entitlementService.UpdateProductDetails(productMessage);
                        break;
                }

                _logger.LogInformation("Processed SQSMessage {MessageId}", message.MessageId);
            }
        }
    }
}
