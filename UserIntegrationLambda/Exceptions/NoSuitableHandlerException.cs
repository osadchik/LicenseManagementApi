using System.Runtime.Serialization;

namespace UserIntegrationLambda.Exceptions
{
    /// <summary>
    /// Represents absence of suitable handler for SQS message.
    /// </summary>
    [Serializable]
    internal class NoSuitableHandlerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NoSuitableHandlerException"/> class.
        /// </summary>
        public NoSuitableHandlerException() : base("No appropriate handler has been found for the incoming message type.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoSuitableHandlerException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NoSuitableHandlerException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoSuitableHandlerException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public NoSuitableHandlerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoSuitableHandlerException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected NoSuitableHandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}