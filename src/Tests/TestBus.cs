using System.Collections.Generic;
using Trader;

namespace Tests
{
    public class TestBus : IBus
    {
        public IList<IMessage> FiredMessages { get; } = new List<IMessage>();

        public void Fire(IMessage @event)
        {
            FiredMessages.Add(@event);
        }
    }
}
