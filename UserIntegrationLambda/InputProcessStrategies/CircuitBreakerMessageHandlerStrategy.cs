using Newtonsoft.Json.Linq;
using UserIntegrationLambda.Interfaces;

namespace UserIntegrationLambda.InputProcessStrategies
{
    /// <summary>
    /// 
    /// </summary>
    internal class CircuitBreakerMessageHandlerStrategy : IDataHandlerStrategy
    {
        public bool IsSuitable(JObject input)
        {
            throw new NotImplementedException();
        }

        public Task<JObject> ProcessInputAsync(JObject input)
        {
            throw new NotImplementedException();
        }
    }
}
