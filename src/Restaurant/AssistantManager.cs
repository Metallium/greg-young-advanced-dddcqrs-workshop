using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class AssistantManager : IHandle<OrderCooked>
    {
        private readonly IPublisher _publisher;
        private readonly IHorn _horn;

        public AssistantManager(IPublisher publisher, IHorn horn)
        {
            _publisher = publisher;
            _horn = horn;
        }

        public void Handle(OrderCooked message)
        {
            var order = message.Order;
            _horn.Say($"[asstManager]: calculating prices for {order.OrderId}.");

            Thread.Sleep(250);
            var random = new Random();

            order.LineItems = order.LineItems.Select(x => new LineItemDto
            {
                Item = x.Item,
                Price = random.Next(1000),
                Quantity = x.Quantity
            }).ToList();
            var totalPerItems = order.LineItems.Select(x => x.Price).Sum();
            var tax = (int)Math.Round(0.2 * totalPerItems);
            order.Tax = tax;
            order.Totals = tax + totalPerItems;

            _publisher.Publish(new OrderPriced(new Order(order)));
        }
    }
}
