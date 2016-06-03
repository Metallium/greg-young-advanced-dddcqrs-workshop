using System;

namespace Restaurant
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Guid CorellationId { get; }
        Guid CausationId { get; }
    }
}
