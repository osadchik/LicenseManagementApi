using System.Runtime.Serialization;

namespace Common.Exceptions
{
    /// <summary>
    /// Represents an error during license retrieving operation.
    /// </summary>
    [Serializable]
    public class LicenseNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LicenseNotFoundException"/> class.
        /// </summary>
        public LicenseNotFoundException() : this("License is not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LicenseNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public LicenseNotFoundException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LicenseNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public LicenseNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LicenseNotFoundException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected LicenseNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}