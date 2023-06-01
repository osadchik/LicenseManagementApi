using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents lambda configuration validation failure.
    /// </summary>
    [Serializable]
    public class MappingStateTransitionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MappingStateTransitionException"/> class.
        /// </summary>
        public MappingStateTransitionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MappingStateTransitionException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public MappingStateTransitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MappingStateTransitionException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected MappingStateTransitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}