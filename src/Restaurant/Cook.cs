using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class Cook : IHandleOrder
    {
        private readonly IHandleOrder _orderHandler;

        public Cook(IHandleOrder orderHandler)
        {
            _orderHandler = orderHandler;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Cook: cooking for order {order.OrderId}.");
            Thread.Sleep(order.LineItems.Sum(it => it.Quantity) * 1000);
            Console.WriteLine($"Cook: writing down ingredients for order {order.OrderId}.");
            order.Ingredients = "some stuff";
            _orderHandler.Handle(new Order(order));
        }
    }
}
