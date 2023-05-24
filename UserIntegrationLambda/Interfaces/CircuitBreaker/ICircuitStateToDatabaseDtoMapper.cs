using Resiliency.CircuitBreaker;
using UserIntegrationLambda.Models;

namespace UserIntegrationLambda.Interfaces.CircuitBreaker
{
    /// <summary>
    /// Interface for circuit state database DTO mapper.
    /// </summary>
    public interface ICircuitStateToDatabaseDtoMapper : ICircuitStateVisitor
    {
        /// <summary>
        /// Returns circuit state DTO.
        /// </summary>
        CircuitStateDatabaseDto? CircuitStateDatabaseDto { get; }
    }
}