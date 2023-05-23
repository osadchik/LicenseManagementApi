using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json.Linq;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    /// Interface of SQS Event processor service.
    /// </summary>
    internal interface ISqsEventProcessingService
    {
        /// <summary>
        /// Processes the SQS Event and logs incomming message.
        /// </summary>
        /// <param name="input">JSON input.</param>
        /// <returns>Task.</returns>
        Task ProcessAsync(JObject input);
    }
}