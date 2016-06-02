using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public class Multiplexor : IHandleOrder
    {
        private readonly List<IHandleOrder> _orderHandlers;

        public Multiplexor(IEnumerable<IHandleOrder> orderHandlers)
        {
            _orderHandlers = orderHandlers.ToList();
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Multiplexor dispatches order {order.OrderId} to {_orderHandlers.Count} handlers.");
            _orderHandlers.ForEach(x => x.Handle(order));
        }
    }
}
