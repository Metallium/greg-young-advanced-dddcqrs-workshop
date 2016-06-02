using System;

namespace Restaurant
{
    public interface IMessage
    {
        Guid Id { get; }
    }

    public class OrderPlaced : IMessage
    {
        public OrderPlaced(Order order)
        {
            Id = Guid.NewGuid();
            Order = order;
        }

        public Order Order { get; set; }

        public Guid Id { get; }
    }

    public class OrderCooked : IMessage
    {
        public OrderCooked(Order order)
        {
            Id = Guid.NewGuid();
            Order = order;
        }

        public Order Order { get; set; }

        public Guid Id { get; }
    }

    public class OrderPriced : IMessage
    {
        public OrderPriced(Order order)
        {
            Id = Guid.NewGuid();
            Order = order;
        }

        public Order Order { get; set; }

        public Guid Id { get; }
    }

    public class OrderPaid : IMessage
    {
        public OrderPaid(Order order)
        {
            Id = Guid.NewGuid();
            Order = order;
        }

        public Order Order { get; set; }

        public Guid Id { get; }
    }
}
