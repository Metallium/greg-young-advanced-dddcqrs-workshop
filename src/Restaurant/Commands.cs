using System;

namespace Restaurant
{
    public class CookFood : IMessage
    {
        public CookFood(Order order)
        {
            Order = order;
        }
        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
        public Order Order { get; }
    }

    public class PriceOrder : IMessage
    {
        public PriceOrder(Order order)
        {
            Order = order;
        }
        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
        public Order Order { get; }
    }

    public class TakePayment : IMessage
    {
        public TakePayment(Order order)
        {
            Order = order;
        }
        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
        public Order Order { get; }
    }

    public class PrintReceipt : IMessage
    {
        public PrintReceipt(Order order)
        {
            Order = order;
        }
        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
        public Order Order { get; }
    }
}
