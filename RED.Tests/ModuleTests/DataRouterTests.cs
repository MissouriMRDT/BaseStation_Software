namespace RED.Tests.ModuleTests
{
    using System.Collections.Generic;
    using System.Net.Sockets;
    using Interfaces;
    using Moq;
    using NUnit.Framework;
    using ViewModels;
    using ViewModels.ControlCenter;

    [TestFixture]
    public class DataRouterTests
    {
        

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void DataRouterSingleKeySingleSubscriberSubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscriber = new Mock<ISubscribe>();
            // Act
            dataRouter.Subscribe(mockSubscriber.Object, 1);
            // Assert
            Assert.Contains(1, dataRouter.Registrations.Keys);
            Assert.Contains(mockSubscriber.Object, dataRouter.Registrations[1]);
        }
        [Test]
        public void DataRouterSingleKeySingleSubscriberUnsubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscriber = new Mock<ISubscribe>();
            // Act
            dataRouter.Subscribe(mockSubscriber.Object, 1);
            dataRouter.UnSubscribe(mockSubscriber.Object, 1);
            // Assert
            Assert.That(dataRouter.Registrations.Count == 0);
        }

        [Test]
        public void DataRouterSingleKeyMultipleSubscriberSubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>
            {
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>()
            };
            // Act
            foreach (var subscriber in mockSubscribers)
            {
                dataRouter.Subscribe(subscriber.Object, 1);
            }
            // Assert
            Assert.Contains(1, dataRouter.Registrations.Keys);
            foreach (var subscriber in mockSubscribers)
            {
                Assert.Contains(subscriber.Object, dataRouter.Registrations[1]);
            }
        }
        [Test]
        public void DataRouterSingleKeyMultipleSubscriberUnsubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>
            {
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>()
            };
            // Act
            foreach (var subscriber in mockSubscribers)
            {
                dataRouter.Subscribe(subscriber.Object, 1);
            }
            foreach (var subscriber in mockSubscribers)
            {
                dataRouter.UnSubscribe(subscriber.Object, 1);
            }
            // Assert
            Assert.That(dataRouter.Registrations.Count == 0);
        }

        [Test]
        public void DataRouterMultipleKeySingleSubscriberSubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>
            {
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>()
            };
            // Act
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.Subscribe(mockSubscribers[i].Object, i);
            }
            // Assert
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                Assert.Contains(i, dataRouter.Registrations.Keys);
                Assert.Contains(mockSubscribers[i].Object, dataRouter.Registrations[i]);
            }
        }
        [Test]
        public void DataRouterMultipleKeySingleSubscriberUnsubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>
            {
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>(),
                new Mock<ISubscribe>()
            };
            // Act
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.Subscribe(mockSubscribers[i].Object, i);
            }
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.UnSubscribe(mockSubscribers[i].Object, i);
            }
            // Assert
            Assert.That(dataRouter.Registrations.Count == 0);
        }

        [Test]
        public void DataRouterMultipleKeyMultipleSubscriberSubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>();
            for (var i = 0; i < 25; i++)
            {
                mockSubscribers.Add(new Mock<ISubscribe>());
            }
            // Act
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.Subscribe(mockSubscribers[i].Object, i % 5);
            }
            // Assert
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                Assert.Contains(i % 5, dataRouter.Registrations.Keys);
                Assert.Contains(mockSubscribers[i].Object, dataRouter.Registrations[i % 5]);
            }
        }
        [Test]
        public void DataRouterMultipleKeyMultipleSubscriberUnsubscribeTest()
        {
            // Arrange
            var dataRouter = new DataRouter();
            var mockSubscribers = new List<Mock<ISubscribe>>();
            for (var i = 0; i < 25; i++)
            {
                mockSubscribers.Add(new Mock<ISubscribe>());
            }
            // Act
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.Subscribe(mockSubscribers[i].Object, i % 5);
            }
            for (var i = 0; i < mockSubscribers.Count; i++)
            {
                dataRouter.UnSubscribe(mockSubscribers[i].Object, i % 5);
            }
            // Assert
            Assert.That(dataRouter.Registrations.Count == 0);
        }
    }
}
