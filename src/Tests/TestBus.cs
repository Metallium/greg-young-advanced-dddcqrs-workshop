using System.Collections.Generic;

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