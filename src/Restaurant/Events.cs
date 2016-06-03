using System;

namespace Restaurant
{
    public class OrderPlaced : IMessage
    {
        public OrderPlaced(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }

        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
    }

    public class OrderCooked : IMessage
    {
        public OrderCooked(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }

        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
    }

    public class OrderPriced : IMessage
    {
        public OrderPriced(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }

        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
    }

    public class OrderPaid : IMessage
    {
        public OrderPaid(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }

        public Guid MessageId { get; } = Guid.NewGuid();
        public Guid CorellationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
    }
}
