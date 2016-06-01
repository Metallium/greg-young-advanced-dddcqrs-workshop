namespace Trader
{
    public class PriceUpdated : IMessage
    {
        public int NewPricePerItem { get; set; }
    }
}
