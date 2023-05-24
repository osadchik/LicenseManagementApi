using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Amazon.SQS.Model;
using Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Common.Services
{
    /// <summary>
    /// Service to work with Amazon SQS queues.
    /// </summary>
    public class SqsClient : ISqsClient
    {
        private readonly IAmazonSQS _sqs;
        private readonly ILogger<SqsClient> _logger;

        public SqsClient(IAmazonSQS sqs, ILogger<SqsClient> logger)
        {
            _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task EnqueueAsync(SQSEvent.SQSMessage message, Uri queueUrl)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            await _sqs.SendMessageAsync(new SendMessageRequest { QueueUrl = queueUrl.AbsoluteUri, MessageBody = message.Body });
            _logger.LogInformation("Message {messageId} added to {queueUrl}", message.MessageId, queueUrl);
        }

        /// <inheritdoc/>
        public async Task<Message?> DequeueAsync(Uri queueUrl)
        {
            _logger.LogDebug("Trying to get message from the queue: {queueUrl}", queueUrl);
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                AttributeNames = { "All" },
                MaxNumberOfMessages = 1,
                QueueUrl = queueUrl.AbsoluteUri,
                VisibilityTimeout = 15,
                WaitTimeSeconds = 0
            };

            ReceiveMessageResponse receiveMessageResponse = await _sqs.ReceiveMessageAsync(receiveMessageRequest);
            _logger.LogDebug("Received message response from the SQS queue: {@response}", receiveMessageResponse);

            return receiveMessageResponse.Messages.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task DeleteMessageAsync(string receiptHandle, Uri queueUrl)
        {
            await _sqs.DeleteMessageAsync(new DeleteMessageRequest { QueueUrl = queueUrl.AbsoluteUri, ReceiptHandle = receiptHandle });
            _logger.LogInformation("Message {receiptHandle} deleted from the {queueUrl}", receiptHandle, queueUrl);
        }
    }
}
