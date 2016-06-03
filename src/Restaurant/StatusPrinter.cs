using System;

namespace Restaurant
{
    public class StatusPrinter :
        IHandle<IMessage>
    {
        public void Handle(IMessage message)
        {
            Console.WriteLine("[statusPrinter]: " + message.GetType().Name);
        }
    }
}
