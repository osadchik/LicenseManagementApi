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
        /// <param name="entitlementService"><see cref="IProductEntitlementManagementService"/></param>
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

            if (sqsEvent is null) throw new ArgumentException(nameof(sqsEvent));

            foreach (SQSMessage message in sqsEvent.Records)
            {
                _logger.LogDebug("Started processing SQSMessage {MessageId}", message.MessageId);
                var body = JObject.Parse(message.Body);
                var entityType = body["EntityType"]?.ToString();

                switch (entityType)
                {
                    case EntityTypes.User:
                        BaseMessage<UserDto> userMessage = JsonConvert.DeserializeObject<BaseMessage<UserDto>>(message.Body);
                        await _entitlementService.UpdateUserDetails(userMessage);
                        break;

                    case EntityTypes.Product:
                        BaseMessage<ProductDto> productMessage = JsonConvert.DeserializeObject<BaseMessage<ProductDto>>(message.Body);
                        await _entitlementService.UpdateProductDetails(productMessage);
                        break;

                    default:
                        break;
                }

                _logger.LogInformation("Processed SQSMessage {MessageId}", message.MessageId);
            }
        }
    }
}
