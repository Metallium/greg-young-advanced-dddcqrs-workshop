namespace Trader
{
    public interface IBus
    {
        void Fire(IMessage @event);
    }
}
