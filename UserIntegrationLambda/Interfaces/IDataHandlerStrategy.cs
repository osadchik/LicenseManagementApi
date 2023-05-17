using Newtonsoft.Json.Linq;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface of a message handler for an appropriate incoming message type.
    /// </summary>
    public interface IDataHandlerStrategy
    {
        /// <summary>
        /// Runs input message handler.
        /// </summary>
        /// <param name="input">Incomming message.</param>
        /// <returns>Message processing details in JSON format.</returns>
        Task<JObject> ProcessInputAsync(JObject input);

        /// <summary>
        /// Checks if this particular handler fits the input.
        /// </summary>
        /// <param name="input">Incomming JSON message.</param>
        /// <returns>True/False.</returns>
        bool IsSuitable(JObject input);
    }
}