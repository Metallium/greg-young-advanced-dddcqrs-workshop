using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class Cook : IHandleOrder
    {
        private readonly IPublisher _publisher;
        private readonly IHorn _horn;
        private readonly string _cookName;
        private readonly int _processingTime;

        public Cook(IPublisher publisher, IHorn horn, string cookName, int processingTime)
        {
            _publisher = publisher;
            _horn = horn;
            _cookName = cookName;
            _processingTime = processingTime;
        }

        public void Handle(Order order)
        {
            _horn.Say($"[cook] {_cookName}: cooking order {order.OrderId}.");
            Thread.Sleep(_processingTime);
            order.Ingredients = "some stuff";
            _horn.Say($"[cook] {_cookName}: cooked order {order.OrderId}.");

            _publisher.Publish(TopicNames.OrderCooked, new Order(order));
        }
    }
}
