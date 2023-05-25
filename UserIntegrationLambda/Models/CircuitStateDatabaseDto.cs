using Amazon.DynamoDBv2.DataModel;

namespace UserIntegrationLambda.Models
{
    /// <summary>
    /// Model used to save the CircuitState into the database.
    /// </summary>
    [DynamoDBTable("LicenseManagement-CircuitBreakerState")]
    public class CircuitStateDatabaseDto
    {
        /// <summary>
        /// Entity ID value.
        /// </summary>
        [DynamoDBHashKey]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Circuit Breaker state.
        /// </summary>
        public string StateId { get; set; } = null!;

        /// <summary>
        /// Opening date.
        /// </summary>
        public DateTime? OpenedAt { get; set; }

        /// <summary>
        /// Starting date.
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Number of errors occured.
        /// </summary>
        public int? ErrorCounter { get; set; }
    }
}