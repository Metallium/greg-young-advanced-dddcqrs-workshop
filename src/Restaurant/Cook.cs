using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class Cook : IHandleOrder
    {
        private readonly string _cookName;
        private readonly IHandleOrder _orderHandler;

        public Cook(string cookName, IHandleOrder orderHandler)
        {
            _cookName = cookName;
            _orderHandler = orderHandler;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"[cook] {_cookName}: cooking order {order.OrderId}.");
            Thread.Sleep(order.LineItems.Sum(it => it.Quantity) * 1000);
            order.Ingredients = "some stuff";
            Console.WriteLine($"[cook] {_cookName}: cooked order {order.OrderId}.");

            _orderHandler.Handle(new Order(order));
        }
    }
}
