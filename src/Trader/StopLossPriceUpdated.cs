namespace Trader
{
    public class StopLossPriceUpdated : IMessage
    {
        public int NewStopLossPrice { get; set; }
    }
}
