using System.Collections.Generic;

namespace Restaurant
{
    public class RoundRobinDispatcher : IHandleOrder
    {
        private readonly Queue<IHandleOrder> _queue;

        public RoundRobinDispatcher(IEnumerable<IHandleOrder> orderHandlers)
        {
            _queue = new Queue<IHandleOrder>(orderHandlers);
        }

        public void Handle(Order order)
        {
            _queue.Peek().Handle(order);
            _queue.Enqueue(_queue.Dequeue());
        }
    }
}
