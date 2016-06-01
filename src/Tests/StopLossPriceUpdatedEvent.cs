namespace Tests
{
    public class StopLossPriceUpdatedEvent : IMessage
    {
        public int NewStopLossPrice { get; set; }
    }
}