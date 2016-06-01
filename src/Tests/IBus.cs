namespace Tests
{
    public interface IBus
    {
        void Fire(IMessage @event);
    }
}