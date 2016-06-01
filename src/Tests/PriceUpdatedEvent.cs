namespace Tests
{
    public class PriceUpdatedEvent : IMessage
    {
        public int NewPricePerItem { get; set; }
    }
}