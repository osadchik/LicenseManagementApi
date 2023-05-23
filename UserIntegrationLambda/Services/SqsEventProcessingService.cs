using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.Services
{
    /// <summary>
    /// SQS Event processor.
    /// </summary>
    internal class SqsEventProcessingService : ISqsEventProcessingService
    {
        private readonly IDataHandlerStrategySelector _strategySelector;
        private readonly ILogger<SqsEventProcessingService> _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SqsEventProcessingService"/> class.
        /// </summary>
        /// <param name="strategySelector"><see cref="IDataHandlerStrategySelector"/></param>
        /// <param name="logger">Logger instance.</param>
        public SqsEventProcessingService(IDataHandlerStrategySelector strategySelector, ILogger<SqsEventProcessingService> logger)
        {
            _strategySelector = strategySelector;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task ProcessAsync(JObject input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return ProcessInternalAsync(input);
        }

        private async Task ProcessInternalAsync(JObject input)
        {
            _logger.LogDebug("SQS message processing started. Message: {@message}", input);

            IDataHandlerStrategy dataHandler = _strategySelector.GetStrategy(input);

            await dataHandler.ProcessInputAsync(input);
        }
    }
}
