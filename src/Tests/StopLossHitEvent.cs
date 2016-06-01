namespace Tests
{
    public class StopLossHitEvent : IMessage
    {
        public StopLossHitEvent(int criticalPrice)
        {
            CriticalPrice = criticalPrice;
        }

        public int CriticalPrice { get; set; }
    }
}