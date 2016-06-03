using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Restaurant
{
    public class AlarmClock : IHandle<SendMeIn>
    {
        private readonly IPublisher _publisher;
        private IList<SendMeIn> _bag;
        private bool _started;

        public AlarmClock(IPublisher publisher)
        {
            _publisher = publisher;
            _bag = new List<SendMeIn>();
        }

        public void Handle(SendMeIn message)
        {
            if (!_started)
            {
                throw new InvalidOperationException();
            }
            lock (_bag)
            {
                _bag.Add(message);
            }
        }

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException();
            }
            _started = true;
            var thread = new Thread(ThreadRunner) { IsBackground = true };
            thread.Start();
        }

        private void ThreadRunner()
        {
            while (true)
            {
                var now = DateTime.UtcNow;
                List<SendMeIn> messagesToSend;
                lock (_bag)
                {
                    messagesToSend = _bag.Where(x => x.When <= now).ToList();
                    _bag = _bag.Except(messagesToSend).ToList();
                }

                messagesToSend.ForEach(x=> _publisher.Publish(x.MessageFactory(x)));

                Thread.Sleep(1);
            }
        }
    }

    public class SendMeIn : IdentifiedMessage
    {
        public DateTime When { get; }
        public Func<IMessage, IMessage> MessageFactory { get; }

        public SendMeIn(IMessage causationMessage, DateTime when, Func<IMessage, IMessage> messageFactory) : base(causationMessage)
        {
            When = when;
            MessageFactory = messageFactory;
        }
    }
}
