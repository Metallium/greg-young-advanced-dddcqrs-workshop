using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class MoreFairDispatcher : IHandleOrder
    {
        private readonly IList<QueuedHandler> _queuedHandlers;

        public MoreFairDispatcher(IList<QueuedHandler> queuedHandlers)
        {
            _queuedHandlers = queuedHandlers;
        }

        public void Handle(Order order)
        {
            IHandleOrder handler;
            do
            {
                handler = _queuedHandlers.FirstOrDefault(x => x.QueueDepth < 5);
                Thread.Sleep(1);
            } while (handler == null);
            handler.Handle(order);
        }
    }
}
