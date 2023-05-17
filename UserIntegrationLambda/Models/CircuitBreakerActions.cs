namespace UserIntegrationLambda.Models
{
    /// <summary>
    /// Represents Circuit Breaker possible change actions.
    /// </summary>
    internal enum CircuitBreakerActions
    {
        Open,
        Close,
        CircuitBreakerTrial,
        PermanentlyClose,
        ReturnState,
        Retry
    }
}
