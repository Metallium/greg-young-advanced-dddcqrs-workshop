using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public class Waiter
    {
        private readonly IPublisher _publisher;
        private readonly IHorn _horn;

        public Waiter(IPublisher publisher, IHorn horn)
        {
            _publisher = publisher;
            _horn = horn;
        }

        public void PlaceNewOrder(Guid orderId, IDictionary<string, int> itemQuantitySpec)
        {
            var order = new Order
            {
                OrderId = orderId.ToString("N"),
                LineItems = itemQuantitySpec.Select(pair => new LineItemDto { Item = pair.Key, Quantity = pair.Value }).ToList()
            };

            _horn.Say($"[waiter]: placing new order {order.OrderId}.");

            var isDodgyCustomer = itemQuantitySpec.Any(x => x.Key == GoodsMenu.Drinkables.Vodka);
            _publisher.Publish(new OrderPlaced(isDodgyCustomer, new Order(order)));
            _horn.Say($"[waiter]: placed new order {order.OrderId}.");
        }
    }
}
