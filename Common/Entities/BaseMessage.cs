using Common.Interfaces;
using Newtonsoft.Json;

namespace Common.Entities
{
    public class BaseMessage<T> : IMessage<T>
    {
        public BaseMessage(string entityId, ProcessAction action)
        {
            EntityId = entityId;
            Action = action;
        }

        [JsonProperty("default")]
        public string Default { get; private set; } = "SQS Message";

        public string EntityId { get; }

        public ProcessAction Action { get; }

        public DateTimeOffset MessageSentOn { get; set; } = DateTimeOffset.UtcNow;

        public T Content { get; set; }

        object IMessage.Content => Content;
    }
}
