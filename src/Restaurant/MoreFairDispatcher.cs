using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class MoreFairDispatcher<TMessage> : IHandle<TMessage>
    {
        private readonly IList<QueuedHandler<TMessage>> _queuedHandlers;

        public MoreFairDispatcher(IList<QueuedHandler<TMessage>> queuedHandlers)
        {
            _queuedHandlers = queuedHandlers;
        }

        public void Handle(TMessage message)
        {
            IHandle<TMessage> handler;
            do
            {
                handler = _queuedHandlers.FirstOrDefault(x => x.QueueDepth < 5);
                Thread.Sleep(1);
            } while (handler == null);
            handler.Handle(message);
        }
    }
}
