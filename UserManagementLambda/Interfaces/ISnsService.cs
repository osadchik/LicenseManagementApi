using Common.Interfaces;

namespace UserManagementLambda.Interfaces
{
    /// <summary>
    /// Interface of SNS message publisher service.
    /// </summary>
    public interface ISnsService
    {
        /// <summary>
        /// Publishes a message to the bus.
        /// </summary>
        /// <typeparam name="T">Type of the message payload.</typeparam>
        /// <param name="topicArn">Target topic arn.</param>
        /// <param name="message">The message to be sent.</param>
        /// <returns></returns>
        Task PublishToTopicAsync<T>(string topicArn, IMessage<T> message);
    }
}
