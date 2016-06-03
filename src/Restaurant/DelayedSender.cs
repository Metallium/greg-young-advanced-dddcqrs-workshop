using System;

namespace Restaurant
{
    public class DelayedSender<TMessage> : IHandle<TMessage>
        where TMessage:IMessage
    {
        public DelayedSender(IHandle<TMessage> handler)
        {

        }
        public void Handle(TMessage message)
        {

        }
    }
}
