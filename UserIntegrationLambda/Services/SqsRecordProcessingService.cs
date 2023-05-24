using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.Services
{
    public class SqsRecordProcessingService : ISqsRecordProcessingService
    {
        public Task ProcessMessageAsync(SQSEvent.SQSMessage sqsMessage)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> ProcessSqsRecordsAsync(SQSEvent sQSEvent)
        {
            throw new NotImplementedException();
        }
    }
}