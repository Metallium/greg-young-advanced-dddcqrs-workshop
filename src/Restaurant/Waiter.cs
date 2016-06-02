using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public class Waiter
    {
        private readonly IHandleOrder _orderHandler;

        public Waiter(IHandleOrder orderHandler)
        {
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

            Console.WriteLine($"[waiter]: placing new order {order.OrderId}.");
            _orderHandler.Handle(new Order(order));
            Console.WriteLine($"[waiter]: placed new order {order.OrderId}.");

            return orderId;
        }
    }
}
