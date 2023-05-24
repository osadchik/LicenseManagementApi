using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents an error during user retrieving operation.
    /// </summary>
    [Serializable]
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UserNotFoundException"/> class.
        /// </summary>
        public UserNotFoundException() : this("User is not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UserNotFoundException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="UserNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}