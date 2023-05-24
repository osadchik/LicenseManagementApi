using Resiliency.CircuitBreaker;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using UserIntegrationLambda.Models;

namespace UserIntegrationLambda.Services.CircuitBreaker
{
    /// <summary>
    /// Mapper class for Circuit State Database DTO.
    /// </summary>
    public class CircuitStateToDatabaseDtoMapper : ICircuitStateToDatabaseDtoMapper
    {
        /// <inheritdoc/>
        public CircuitStateDatabaseDto? CircuitStateDatabaseDto { get; private set; }

        /// <inheritdoc/>
        public void Visit(OpenState state)
        {
            CircuitStateDatabaseDto = new CircuitStateDatabaseDto
            {
                StateId = "Open",
                OpenedAt = state.OpenedAt.DateTime
            };
        }

        /// <inheritdoc/>
        public void Visit(ClosedState state)
        {
            CircuitStateDatabaseDto = new CircuitStateDatabaseDto
            {
                StateId = "Closed",
                ErrorCounter = state.ErrorCounter,
                StartedAt = state.StartedAt.DateTime
            };
        }

        /// <inheritdoc/>
        public void Visit(HalfOpenState state)
        {
            CircuitStateDatabaseDto = new CircuitStateDatabaseDto
            {
                StateId = "HalfOpen"
            };
        }

        /// <inheritdoc/>
        public void Visit(PermanentlyClosedState state)
        {
            CircuitStateDatabaseDto = new CircuitStateDatabaseDto
            {
                StateId = "PermanentlyClosed",
                StartedAt = state.StartedAt.DateTime
            };
        }
    }
}