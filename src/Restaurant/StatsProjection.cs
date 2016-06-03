namespace Restaurant
{
    public class StatsProjection : IHandle<OrderPlaced>, IHandle<OrderFinalized>
    {
        public StatsProjection()
        {
            PlacedOrdersCount = 0;
            FinalizedOrdersCount = 0;
        }

        public int FinalizedOrdersCount { get; private set; }

        public int PlacedOrdersCount { get; private set; }

        public void Handle(OrderPlaced message)
        {
            ++PlacedOrdersCount;
        }

        public void Handle(OrderFinalized message)
        {
            ++FinalizedOrdersCount;
        }
    }
}
