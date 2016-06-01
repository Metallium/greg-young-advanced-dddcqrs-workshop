using System;

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
            Console.WriteLine($"Cook: placing ingredients for order {order.OrderId}.");

            order.Ingredients = "some stuff";
            _orderHandler.Handle(order);
        }
    }
}
