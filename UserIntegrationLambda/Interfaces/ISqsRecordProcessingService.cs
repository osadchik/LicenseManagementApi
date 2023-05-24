using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json.Linq;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    ///  SQS record processing service.
    /// </summary>
    public interface ISqsRecordProcessingService
    {
        /// <summary>
        /// Processes all records from an SQS Event.
        /// </summary>
        /// <param name="sQSEvent"><see cref="SQSEvent"/></param>
        /// <returns>Processing details in JSON type.</returns>
        Task<JObject> ProcessSqsRecordsAsync(SQSEvent sQSEvent);

        /// <summary>
        /// Processes one SQS message.
        /// </summary>
        /// <param name="sqsMessage"><see cref="SQSMessage"/></param>
        /// <returns>Task.</returns>
        Task ProcessMessageAsync(SQSMessage sqsMessage);
    }
}