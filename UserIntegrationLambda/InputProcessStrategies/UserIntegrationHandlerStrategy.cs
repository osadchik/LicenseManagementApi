using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// 
    /// </summary>
    public class UserIntegrationHandlerStrategy : IDataHandlerStrategy
    {
        private readonly ISqsRecordProcessingService _sqsRecordProcessingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqsRecordProcessingService"></param>
        public UserIntegrationHandlerStrategy(ISqsRecordProcessingService sqsRecordProcessingService)
        {
            _sqsRecordProcessingService = sqsRecordProcessingService;
        }

        /// <inheritdoc/>
        public bool IsSuitable(JObject input)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<JObject> ProcessInputAsync(JObject input)
        {
            throw new NotImplementedException();
        }
    }
}