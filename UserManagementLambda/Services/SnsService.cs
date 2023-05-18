using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Common.Interfaces;
using Newtonsoft.Json;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Services
{
    /// <summary>
    /// Sns message publisher service.
    /// </summary>
    public class SnsService : ISnsService
    {
        private readonly IAmazonSimpleNotificationService _simpleNotificationService;
        private readonly ILogger<SnsService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SnsService"/> class.
        /// </summary>
        /// <param name="simpleNotificationService"><see cref="IAmazonSimpleNotificationService"/></param>
        /// <param name="logger">Logger instance.</param>
        public SnsService(IAmazonSimpleNotificationService simpleNotificationService, ILogger<SnsService> logger)
        {
            _simpleNotificationService = simpleNotificationService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task PublishToTopicAsync<T>(string topicArn, IMessage<T> message)
        {
            _logger.LogDebug("Trying to push the content: {@content} to the SNS topic: {topicArn}", message, topicArn);

            var content = JsonConvert.SerializeObject(message);
            _logger.LogDebug("Serialized content: {content}", content);

            var request = new PublishRequest
            {
                MessageStructure = "json",
                TopicArn = topicArn,
                Message = content
            };

            await _simpleNotificationService.PublishAsync(request);
            _logger.LogInformation("Successfully published message: {content} ", content);
        }   
    }
}
