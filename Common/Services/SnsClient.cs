﻿using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Services
{
    /// <summary>
    /// Sns message publisher service.
    /// </summary>
    public class SnsClient : ISnsClient
    {
        private readonly IAmazonSimpleNotificationService _simpleNotificationService;
        private readonly ILogger<SnsClient> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SnsClient"/> class.
        /// </summary>
        /// <param name="simpleNotificationService"><see cref="IAmazonSimpleNotificationService"/></param>
        /// <param name="logger">Logger instance.</param>
        public SnsClient(IAmazonSimpleNotificationService simpleNotificationService, ILogger<SnsClient> logger)
        {
            _simpleNotificationService = simpleNotificationService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task PublishToTopicAsync<T>(string topicArn, IMessage<T> message)
        {
            _logger.LogDebug("Started publishing the content: {@content} to the SNS topic: {topicArn}", message, topicArn);

            var content = JsonConvert.SerializeObject(message, Formatting.None);
            _logger.LogDebug("Serialized content: {content}", content);

            Dictionary<string, MessageAttributeValue> messageAttributes = new()
            {
                { "Action", new MessageAttributeValue() { StringValue = message.Action.ToString(), DataType = "String" } },
                { "EntityType", new MessageAttributeValue() { StringValue = message.EntityType.ToString(), DataType = "String" } }
            };

            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = content,
                MessageAttributes = messageAttributes
            };
            _logger.LogDebug("Created a new SNS publish request: {@request}", request);

            await _simpleNotificationService.PublishAsync(request);
            _logger.LogInformation("Successfully published message: {@content} to the SNS topic: {topicArn}", content);
        }
    }
}
