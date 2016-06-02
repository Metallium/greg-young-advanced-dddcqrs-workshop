using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class Cook : IHandleOrder
    {
        private readonly IHorn _horn;
        private readonly string _cookName;
        private readonly int _processingTime;
        private readonly IHandleOrder _orderHandler;

        public Cook(IHorn horn, string cookName, int processingTime, IHandleOrder orderHandler)
        {
            _horn = horn;
            _cookName = cookName;
            _processingTime = processingTime;
            _orderHandler = orderHandler;
        }

        public void Handle(Order order)
        {
            _horn.Say($"[cook] {_cookName}: cooking order {order.OrderId}.");
            Thread.Sleep(_processingTime);
            order.Ingredients = "some stuff";
            _horn.Say($"[cook] {_cookName}: cooked order {order.OrderId}.");

            _orderHandler.Handle(new Order(order));
        }
    }
}
