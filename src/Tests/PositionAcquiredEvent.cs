namespace Tests
{
    public class PositionAcquiredEvent : IMessage
    {
        public int Amount { get; set; }
        public int PricePerItem { get; set; }
    }
}