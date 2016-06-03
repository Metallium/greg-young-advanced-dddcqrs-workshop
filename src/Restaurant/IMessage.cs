using System;

namespace Restaurant
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Guid CorellationId { get; }
        Guid CausationId { get; }
    }

    public abstract class IdentifiedMessage : IMessage
    {
        protected IdentifiedMessage(Guid causationId, Guid corellationId, Guid messageId)
        {
            CausationId = causationId;
            CorellationId = corellationId;
            MessageId = messageId;
        }

        protected IdentifiedMessage(IMessage causationMessage)
            : this(causationMessage.MessageId, causationMessage.CorellationId, Guid.NewGuid())
        {
        }

        public Guid MessageId { get; }
        public Guid CorellationId { get; }
        public Guid CausationId { get; }
    }
}
