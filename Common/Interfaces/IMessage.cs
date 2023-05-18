using Common.Entities;

namespace Common.Interfaces
{
    /// <summary>
    /// Represents a message in SNS or SQS.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the Id of the entity message refers to.
        /// </summary>
        public string EntityId { get; }

        /// <summary>
        /// Gets the action being performed of the Entity.
        /// </summary>
        public ProcessAction Action { get; }

        /// <summary>
        /// Gets the body of the message.
        /// </summary>
        public object Content { get; }

        /// <summary>
        /// Gets or sets the datetime that the message was sent.
        /// </summary>
        public DateTimeOffset MessageSentOn { get; set; }
    }

    /// <summary>
    /// Represents a strongly typed instance of a message.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    public interface IMessage<T> : IMessage
    {
        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        new T Content { get; set; }
    }
}
