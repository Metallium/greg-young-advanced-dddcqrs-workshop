using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class Cook : IHandleOrder
    {
        private readonly string _cookName;
        private readonly int _processingTime;
        private readonly IHandleOrder _orderHandler;

        public Cook(string cookName, int processingTime, IHandleOrder orderHandler)
        {
            _cookName = cookName;
            _processingTime = processingTime;
            _orderHandler = orderHandler;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"[cook] {_cookName}: cooking order {order.OrderId}.");
            Thread.Sleep(_processingTime);
            order.Ingredients = "some stuff";
            Console.WriteLine($"[cook] {_cookName}: cooked order {order.OrderId}.");

            _orderHandler.Handle(new Order(order));
        }
    }
}
