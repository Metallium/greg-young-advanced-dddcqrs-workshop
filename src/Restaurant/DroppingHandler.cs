using System;

namespace Restaurant
{
    public class DroppingHandler<T> : IHandle<T>
    {
        private readonly IHandle<T> _handler;
        private static readonly Random Random = new Random(Convert.ToInt32(DateTime.Now.Ticks % int.MaxValue));

        public DroppingHandler(IHandle<T> handler)
        {
            _handler = handler;
        }

        public void Handle(T message)
        {
            var nextDouble = Random.NextDouble();
            if (nextDouble <= 0.333)
            {
                return;
            }
            _handler.Handle(message);
        }
    }
}
