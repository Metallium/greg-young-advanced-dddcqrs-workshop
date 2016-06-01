using System;

namespace Trader
{
    public class SendBackIn : IMessage
    {
        public SendBackIn(TimeSpan timeout, IMessage messageToSend)
        {
            Timeout = timeout;
            MessageToSend = messageToSend;
        }

        public TimeSpan Timeout { get; }
        public IMessage MessageToSend { get; }
    }
}
