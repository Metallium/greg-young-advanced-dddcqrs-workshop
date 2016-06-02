using System;
using System.Threading;

namespace Restaurant
{
    public class Cashier : IHandleOrder
    {
        private readonly IHorn _horn;
        private readonly IHandleOrder _handleOrder;

        public Cashier(IHorn horn, IHandleOrder handleOrder)
        {
            _horn = horn;
            _handleOrder = handleOrder;
        }

        public void Handle(Order order)
        {
            _horn.Say($"[cashier]: taking payment for {order.OrderId}.");

            Thread.Sleep(500);

            order.IsPaid = true;

            _horn.Say($"[cashier]: took payment for {order.OrderId}.");

            _handleOrder.Handle(new Order(order));
        }
    }
}
