using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;
using UserIntegrationLambda.Interfaces.CircuitBreaker;
using UserIntegrationLambda.Models;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// Processes circuit breaker maintenance messages.
    /// </summary>
    public class CircuitBreakerMessageHandlerStrategy : IDataHandlerStrategy
    {
        private readonly ICircuitBreakingService _circuitBreakingService;
        private readonly ISqsRecordProcessingService _sqsRecordProcessingService;
        private readonly ILogger<CircuitBreakerMessageHandlerStrategy> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="CircuitBreakerMessageHandlerStrategy"/> class.
        /// </summary>
        /// <param name="circuitBreakingService"><see cref="ICircuitBreakingService"/></param>
        /// <param name="sqsRecordProcessingService"><see cref="ISqsRecordProcessingService"/></param>
        /// <param name="logger">Logger instance.</param>
        public CircuitBreakerMessageHandlerStrategy(ICircuitBreakingService circuitBreakingService, ISqsRecordProcessingService sqsRecordProcessingService, ILogger<CircuitBreakerMessageHandlerStrategy> logger)
        {
            _circuitBreakingService = circuitBreakingService;
            _sqsRecordProcessingService = sqsRecordProcessingService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public bool IsSuitable(JObject input)
        {
            return input.ContainsKey("IsMaintenance") && input.Value<bool>("IsMaintenance");
        }

        /// <inheritdoc/>
        public async Task ProcessInputAsync(JObject input)
        {
            _logger.LogDebug("Received maintenance message.");
            CircuitBreakerMessage message = input.ToObject<CircuitBreakerMessage>();

            if (message == null) throw new ArgumentNullException(nameof(input));

            switch (message.Action)
            {
                case CircuitBreakerActions.Open:
                    await _circuitBreakingService.Open(message.Timeout);
                    break;
                case CircuitBreakerActions.Close:
                    await _circuitBreakingService.Close();
                    break;
                case CircuitBreakerActions.CircuitBreakerTrial:
                    await _circuitBreakingService.Trial(_sqsRecordProcessingService.ProcessMessageAsync);
                    break;
                case CircuitBreakerActions.PermanentlyClose:
                    await _circuitBreakingService.PermanentlyClose();
                    break;
                default:
                    throw new NotSupportedException($"Action {message.Action} is not supported.");
            }
        }
    }
}
