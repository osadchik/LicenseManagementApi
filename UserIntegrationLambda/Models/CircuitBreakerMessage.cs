namespace UserIntegrationLambda.Models
{
    /// <summary>
    /// Model used to describe Circuit Breaker maintenance message.
    /// </summary>
    internal class CircuitBreakerMessage
    {
        /// <summary>
        /// Gets or sets the flag if this the message is maintenance.
        /// </summary>
        public bool IsMaintenance { get; set; }

        /// <summary>
        /// Gets or sets the message action.
        /// </summary>
        public CircuitBreakerActions Action { get; set; }

        /// <summary>
        /// Gets or sets the Circuit Breaker timeout in minutes.
        /// </summary>
        public TimeSpan? Timeout { get; set; }
    }
}
