using System;

namespace Restaurant
{
    public class MessageTracer :
        IHandle<IMessage>
    {
        private readonly string _topic;

        public MessageTracer(string topic)
        {
            _topic = topic;
        }

        public void Handle(IMessage message)
        {
            var orderPlaced = message as OrderPlaced;
            if (orderPlaced != null)
            {
                Console.WriteLine($"[status({_topic})]: {message.GetType().Name}, dodgy={orderPlaced.IsDodgyCustomer}");
                return;
            }
            Console.WriteLine($"[status({_topic})]: {message.GetType().Name}");
        }
    }
}
