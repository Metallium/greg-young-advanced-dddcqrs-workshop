using System;
using System.Xml;
using NSubstitute;
using NUnit.Framework;
using Restaurant;

namespace Tests
{
    public class TopicBasedPubSubTests
    {
        public class TestMessage : IMessage
        {
            public Guid Id { get; } = Guid.NewGuid();
        }
        public class AnotherTestMessage : IMessage
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        [Test]
        public void NoSubscriptionsPublishDoesntThrowTest()
        {
            var pubSub = new TopicBasedPubSub();
            Assert.DoesNotThrow(() => pubSub.Publish(new TestMessage()));
        }

        [Test]
        public void SingleSubscriptionPublishTest()
        {
            var handler = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();
            pubSub.SubscribeByType(handler);
            pubSub.Publish(new TestMessage());
            pubSub.Publish(new AnotherTestMessage());

            handler.Received().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void MultipleSubscriptionsPublishTest()
        {
            var handler1 = Substitute.For<IHandle<TestMessage>>();
            var handler2 = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();
            pubSub.SubscribeByType(handler1);
            pubSub.SubscribeByType(handler2);
            pubSub.Publish(new TestMessage());

            handler1.Received().Handle(Arg.Any<TestMessage>());
            handler2.Received().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void SubscribeWhenPublishingDoesntThrowTest()
        {
            var handler1 = Substitute.For<IHandle<TestMessage>>();
            var handler2 = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();

            handler1
                .When(x => x.Handle(Arg.Any<TestMessage>()))
                .Do(callInfo =>
                {
                    Assert.DoesNotThrow(() => pubSub.SubscribeByType(handler2));
                });

            pubSub.SubscribeByType(handler1);
            pubSub.Publish(new TestMessage());

            handler2.DidNotReceive().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void UnsubscribeWhenPublishingTest()
        {
            var handler1 = Substitute.For<IHandle<TestMessage>>();
            var handler2 = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();

            handler1
                .When(x => x.Handle(Arg.Any<TestMessage>()))
                .Do(callInfo =>
                {
                    Assert.DoesNotThrow(() => pubSub.UnsubscribeByType(handler1));
                });

            pubSub.SubscribeByType(handler1);
            pubSub.SubscribeByType(handler2);
            pubSub.Publish(new TestMessage());

            handler2.Received().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void PublishWhenPublishingTest()
        {
            var handler1 = Substitute.For<IHandle<TestMessage>>();
            var handler2 = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();

            handler1
                .When(x => x.Handle(Arg.Any<TestMessage>()))
                .Do(callInfo =>
                {
                    Assert.DoesNotThrow(() => pubSub.UnsubscribeByType(handler1));
                });

            pubSub.SubscribeByType(handler1);
            pubSub.SubscribeByType(handler2);
            pubSub.Publish(new TestMessage());

            handler2.Received().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void UnsubscribeTest()
        {
            var handler = Substitute.For<IHandle<TestMessage>>();

            var pubSub = new TopicBasedPubSub();
            pubSub.SubscribeByType(handler);
            pubSub.UnsubscribeByType(handler);

            pubSub.Publish(new TestMessage());

            handler.DidNotReceive().Handle(Arg.Any<TestMessage>());
        }

        [Test]
        public void NarrowingHandlerEqualityTest()
        {
            var handler = Substitute.For<IHandle<TestMessage>>();
            var narrowed1 = handler.NarrowTo<IMessage, TestMessage>();

            Assert.That(narrowed1.Equals(handler), Is.True);

            var narrowed2 = handler.NarrowTo<IMessage, TestMessage>();

            Assert.That(narrowed1.Equals(narrowed2), Is.True);
            Assert.That(narrowed2.Equals(narrowed1), Is.True);
        }
    }
}
