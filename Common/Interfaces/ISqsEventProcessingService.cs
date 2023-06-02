using Newtonsoft.Json.Linq;

namespace Common.Interfaces
{
    /// <summary>
    /// Interface of SQS Event processor service.
    /// </summary>
    public interface ISqsEventProcessingService
    {
        /// <summary>
        /// Processes the SQS Event and logs incomming message.
        /// </summary>
        /// <param name="input">JSON input.</param>
        /// <returns>Task.</returns>
        Task ProcessAsync(JObject input);
    }
}