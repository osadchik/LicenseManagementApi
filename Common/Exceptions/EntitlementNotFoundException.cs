using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents an error during user retrieving operation.
    /// </summary>
    [Serializable]
    public class EntitlementNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EntitlementNotFoundException"/> class.
        /// </summary>
        public EntitlementNotFoundException() : this("Product Entitlement is not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EntitlementNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public EntitlementNotFoundException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EntitlementNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntitlementNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EntitlementNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected EntitlementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}