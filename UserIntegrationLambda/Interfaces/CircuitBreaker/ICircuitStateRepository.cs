using Resiliency.CircuitBreaker;

namespace UserIntegrationLambda.Interfaces.CircuitBreaker
{
    /// <summary>
    /// Interface of Circuit Breaker State datastore.
    /// </summary>
    public interface ICircuitStateRepository
    {
        /// <summary>
        /// Gets the state of circuit breaker.
        /// </summary>
        /// <returns>Task.</returns>
        Task<CircuitState> GetAsync();

        /// <summary>
        /// Saves an item to the datastore.
        /// </summary>
        /// <param name="circuitState">New state of the circuit breaker.</param>
        /// <returns></returns>
        Task SaveAsync(CircuitState circuitState);
    }
}
