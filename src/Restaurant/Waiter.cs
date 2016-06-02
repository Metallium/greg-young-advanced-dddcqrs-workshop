using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public class Waiter
    {
        private readonly IHorn _horn;
        private readonly IHandleOrder _orderHandler;

        public Waiter(IHorn horn, IHandleOrder orderHandler)
        {
            _horn = horn;
            _orderHandler = orderHandler;
        }

        public Guid PlaceNewOrder(IDictionary<string, int> itemQuantitySpec)
        {
            var orderId = Guid.NewGuid();

            var order = new Order
            {
                OrderId = orderId.ToString("N"),
                LineItems = itemQuantitySpec.Select(pair => new LineItemDto { Item = pair.Key, Quantity = pair.Value }).ToList()
            };

            _horn.Say($"[waiter]: placing new order {order.OrderId}.");
            _orderHandler.Handle(new Order(order));
            _horn.Say($"[waiter]: placed new order {order.OrderId}.");

            return orderId;
        }
    }
}
