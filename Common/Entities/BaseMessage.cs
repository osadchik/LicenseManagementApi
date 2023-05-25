using Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Entities
{
    public class BaseMessage<T> : IMessage<T>
    {
        public BaseMessage(string entityId, ProcessAction action)
        {
            EntityId = entityId;
            Action = action;
        }

        public string EntityId { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProcessAction Action { get; }

        public DateTimeOffset MessageSentOn { get; set; } = DateTimeOffset.UtcNow;

        public T Content { get; set; } = default!;

        object IMessage.Content => Content!;
    }
}
