﻿using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// Processes circuit breaker maintenance messages.
    /// </summary>
    internal class CircuitBreakerMessageHandlerStrategy : IDataHandlerStrategy
    {
        /// <inheritdoc/>
        public bool IsSuitable(JObject input)
        {
            return input.ContainsKey("IsMaintenance") && input.Value<bool>("IsMaintenance");
        }

        /// <inheritdoc/>
        public Task ProcessInputAsync(JObject input)
        {
            throw new NotImplementedException();
        }
    }
}
