using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents an error during product retrieving operation.
    /// </summary>
    [Serializable]
    public class ProductNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ProductNotFoundException"/> class.
        /// </summary>
        public ProductNotFoundException() : this("Product is not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProductNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ProductNotFoundException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProductNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ProductNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProductNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected ProductNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}