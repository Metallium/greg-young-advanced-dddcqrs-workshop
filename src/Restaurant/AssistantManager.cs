using System;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class AssistantManager : IHandleOrder
    {
        private readonly IHorn _horn;
        private readonly IHandleOrder _orderHandler;

        public AssistantManager(IHorn horn, IHandleOrder orderHandler)
        {
            _horn = horn;
            _orderHandler = orderHandler;
        }

        public void Handle(Order order)
        {
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

            _orderHandler.Handle(new Order(order));
        }
    }
}
