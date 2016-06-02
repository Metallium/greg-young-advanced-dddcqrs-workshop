using System;
using System.Threading;

namespace Restaurant
{
    public class Cashier : IHandleOrder
    {
        private readonly IHandleOrder _handleOrder;

        public Cashier(IHandleOrder handleOrder)
        {
            _handleOrder = handleOrder;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"[cashier]: taking payment for {order.OrderId}.");

            Thread.Sleep(1000);

            order.IsPaid = true;
            _handleOrder.Handle(new Order(order));
        }
    }
}
