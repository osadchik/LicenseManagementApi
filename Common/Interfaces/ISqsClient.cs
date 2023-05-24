using Amazon.SQS.Model;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Common.Interfaces
{
    /// <summary>
    /// Interface of service to work with AWS SQS queues.
    /// </summary>
    public interface ISqsClient
    {
        /// <summary>
        /// Adds message to the SQS queue.
        /// </summary>
        /// <param name="message"><see cref="SQSMessage"/></param>
        /// <param name="queueUrl">Queue url.</param>
        /// <returns>Task.</returns>
        Task EnqueueAsync(SQSMessage message, Uri queueUrl);

        /// <summary>
        /// Takes last message from the SQS queue.
        /// </summary>
        /// <param name="queueUrl">Queue url.</param>
        /// <returns><see cref="Message"/></returns>
        Task<Message?> DequeueAsync(Uri queueUrl);

        /// <summary>
        /// Deletes message from the SQS queue.
        /// </summary>
        /// <param name="receiptHandle">Receipt handle.</param>
        /// <param name="queueUrl">Queue url.</param>
        /// <returns>Task.</returns>
        Task DeleteMessageAsync(string receiptHandle, Uri queueUrl);
    }
}