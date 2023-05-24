using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents lambda configuration validation failure.
    /// </summary>
    [Serializable]
    public class EventSourceMappingStateTransitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EventSourceMappingStateTransitionException"/> class.
        /// </summary>
        public EventSourceMappingStateTransitionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EventSourceMappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public EventSourceMappingStateTransitionException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EventSourceMappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EventSourceMappingStateTransitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EventSourceMappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected EventSourceMappingStateTransitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}