using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    public class Multiplexor<TMessage> : IHandle<TMessage>
    {
        private readonly IList<IHandle<TMessage>> _orderHandlers;

        public Multiplexor(IList<IHandle<TMessage>> handlers)
        {
            _orderHandlers = handlers.ToList();
        }

        public void Handle(TMessage message)
        {
            _orderHandlers.ForEach(x => x.Handle(message));
        }
    }
}
