namespace UserIntegrationLambda.Options
{
    /// <summary>
    /// Contains configuration parameters for circuit breaker.
    /// </summary>
    public class CircuitBreakerOptions
    {
        /// <summary>
        /// Name of circuit state Dynamo DB table.
        /// </summary>
        public string CircuitStateTableName { get; set; } = null!;

        /// <summary>
        /// Max number of errors before opening.
        /// </summary>
        public int ErrorThreshold { get; set; } = default!;

        /// <summary>
        /// Timeout value.
        /// </summary>
        public TimeSpan Timeout { get; set; } = default!;

        /// <summary>
        /// Sampling duration in minutes.
        /// </summary>
        public TimeSpan SamplingDuration { get; set; } = default!;

        /// <summary>
        /// Dead Letter Queue url.
        /// </summary>
        public Uri DeadLetterQueueUrl { get; set; } = null!;

        /// <summary>
        /// SQS source queue url.
        /// </summary>
        public Uri SourceQueueUrl { get; set; } = null!;
    }
}