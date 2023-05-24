using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace UserIntegrationLambda.Interfaces.CircuitBreaker
{
    /// <summary>
    /// Interface of a circuit breaker service adapter.
    /// </summary>
    public interface ICircuitBreakingService
    {
        /// <summary>
        /// Processes sqs message and breaks execution circuit if necessary.
        /// </summary>
        /// <param name="sqsMessage">Incoming sqs message.</param>
        /// <param name="action">Call to the external service.</param>
        /// <returns>Task.</returns>
        Task ExecuteAsync(SQSMessage sqsMessage, Func<SQSMessage, Task> action);

        /// <summary>
        /// Changes the circuit breaker to open state.
        /// </summary>
        /// <param name="customTimeout">Overrided value for circuit breaker timeout.</param>
        /// <returns>Task.</returns>
        Task Open(TimeSpan? customTimeout);

        /// <summary>
        /// Changes the circuit breaker to closed state.
        /// </summary>
        /// <returns>Task.</returns>
        Task Close();

        /// <summary>
        /// Permanently close the circuit breaker.
        /// </summary>
        /// <returns>Task.</returns>
        Task PermanentlyClose();

        /// <summary>
        /// Changes the circuit breaker to half-open state.
        /// </summary>
        /// <returns>Task.</returns>
        Task HalfOpen();

        /// <summary>
        /// Tries execution of one message.
        /// </summary>
        /// <param name="action">Call to the external service.</param>
        /// <returns>Task.</returns>
        Task Trial(Func<SQSMessage, Task> action);
    }
}