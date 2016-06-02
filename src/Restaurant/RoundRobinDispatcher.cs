using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            Console.WriteLine($"RoundRobin dispatcher dispatches order {order.OrderId}.");
            _queue.Peek().Handle(order);
            _queue.Enqueue(_queue.Dequeue());
        }
    }
}
