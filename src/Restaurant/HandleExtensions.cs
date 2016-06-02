namespace Restaurant
{
    public static class HandleExtensions
    {
        public static IHandle<TInput> WidenFrom<TInput, TOutput>(this IHandle<TOutput> handler)
            where TOutput : IMessage
            where TInput : TOutput
        {
            return new WideningHandler<TInput, TOutput>(handler);
        }

        public static IHandle<TInput> NarrowTo<TInput, TOutput>(this IHandle<TOutput> handler)
            where TInput : IMessage
            where TOutput : TInput
        {
            return new NarrowingHandler<TInput, TOutput>(handler);
        }
    }

    public class NarrowingHandler<TInput, TOutput> : IHandle<TInput>
        where TInput : IMessage
        where TOutput : TInput
    {
        private readonly IHandle<TOutput> _handler;

        public NarrowingHandler(IHandle<TOutput> handler)
        {
            _handler = handler;
        }

        public void Handle(TInput message)
        {
            _handler.Handle((TOutput) message); // will throw if message type is wrong
        }
    }

    public class WideningHandler<TInput, TOutput> : IHandle<TInput>
        where TInput : TOutput
        where TOutput : IMessage
    {
        private readonly IHandle<TOutput> _handler;

        public WideningHandler(IHandle<TOutput> handler)
        {
            _handler = handler;
        }

        public void Handle(TInput message)
        {
            _handler.Handle(message);
        }

        public override string ToString()
        {
            return $"WideningHandler<{typeof (TInput).Name}, {typeof (TOutput).Name}>({_handler})";
        }
    }
}
