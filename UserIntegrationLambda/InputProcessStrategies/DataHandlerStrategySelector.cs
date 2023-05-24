using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Exceptions;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// Picks the correct strategy to handle specific input.
    /// </summary>
    public class DataHandlerStrategySelector : IDataHandlerStrategySelector
    {
        private readonly ILogger<DataHandlerStrategySelector> _logger;
        private readonly IEnumerable<IDataHandlerStrategy> _handlers;

        /// <summary>
        /// Initializes a new instance of <see cref="DataHandlerStrategySelector"/> class.
        /// <param name="logger">Logger instance.</param>
        /// <param name="handlers">The collection of all available input handlers.</param>
        /// </summary>
        public DataHandlerStrategySelector(ILogger<DataHandlerStrategySelector> logger, IEnumerable<IDataHandlerStrategy> handlers)
        {
            _logger = logger;
            _handlers = handlers;
        }

        /// <inheritdoc/>
        public IDataHandlerStrategy GetStrategy(JObject input)
        {
            _logger.LogDebug("Getting strategy for input: {@input}", input);

            var strategy = _handlers.FirstOrDefault(h => h.IsSuitable(input));

            if (strategy is null)
            {
                throw new NoSuitableHandlerException();
            }

            _logger.LogInformation("Chosen input handler is {strategyHandler}", strategy.GetType());
            return strategy;
        }
    }
}
