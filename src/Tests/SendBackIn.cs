using System;

namespace Tests
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