﻿using Amazon.Lambda.SQSEvents;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace UserIntegrationLambda.Interfaces
{
    /// <summary>
    ///  Interface of SQS record processing service.
    /// </summary>
    public interface ISqsRecordProcessingService
    {
        /// <summary>
        /// Processes all records from an SQS Event.
        /// </summary>
        /// <param name="sQSEvent"><see cref="SQSEvent"/></param>
        /// <returns>Task.</returns>
        Task ProcessSqsRecordsAsync(SQSEvent sqsEvent);

        /// <summary>
        /// Processes one SQS message.
        /// </summary>
        /// <param name="sqsMessage"><see cref="SQSMessage"/></param>
        /// <returns>Task.</returns>
        Task ProcessMessageAsync(SQSMessage sqsMessage);
    }
}