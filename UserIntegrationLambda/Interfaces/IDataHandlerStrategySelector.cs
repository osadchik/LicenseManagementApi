using Newtonsoft.Json.Linq;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface for strategy pattern selector service.
    /// </summary>
    public interface IDataHandlerStrategySelector
    {
        /// <summary>
        /// Picks strategy to process given input.
        /// </summary>
        /// <param name="input">JSON object.</param>
        /// <returns>Strategy to handle input JSON.</returns>
        IDataHandlerStrategy GetStrategy(JObject input);
    }
}