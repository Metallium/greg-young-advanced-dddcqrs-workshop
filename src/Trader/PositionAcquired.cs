namespace Trader
{
    public class PositionAcquired : IMessage
    {
        public int Amount { get; set; }
        public int PricePerItem { get; set; }
    }
}
