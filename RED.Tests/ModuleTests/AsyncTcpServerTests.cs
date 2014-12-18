namespace RED.Tests.ModuleTests
{
    using Contexts;
    using NUnit.Framework;
    using ViewModels;
    using ViewModels.ControlCenter;

    [TestFixture]
    public class AsyncTcpServerTests
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
        public void ServerConstructor()
        {
            // Arrange
            // Act
            var cc=new ControlCenterViewModel();
            var server = new AsyncTcpServerViewModel(11000, cc);
            // Assert
            Assert.AreEqual(server.ListeningPort, 11000);
        }

        [Test]
        public void ServerListen()
        {
            // Arrange
            // Act
            var cc = new ControlCenterViewModel();
            var server = new AsyncTcpServerViewModel(11000, cc);
            // Assert
            Assert.AreEqual(server.ListeningPort, 11000);
        }
    }
}