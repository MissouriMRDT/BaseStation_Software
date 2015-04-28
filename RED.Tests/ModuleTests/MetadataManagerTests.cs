namespace RED.Tests.ModuleTests
{
    using Contexts;
    using NUnit.Framework;
    using ViewModels;
    using ViewModels.ControlCenter;

    [TestFixture]
    public class MetadataManagerTests
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
        public void MetadataManagerConstructor()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            // Act
            var manager = new MetadataManager(cc);
            // Assert
            Assert.IsNotNull(manager.Commands);
            Assert.IsNotNull(manager.Telemetry);
            Assert.IsNotNull(manager.Errors);
        }

        [Test]
        public void MetadataManagerAddCommand()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            CommandMetadataContext context = new CommandMetadataContext();
            // Act
            manager.Add(context);
            // Assert
            Assert.Contains(context, manager.Commands);
        }

        [Test]
        public void MetadataManagerAddTelemetry()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            TelemetryMetadataContext context = new TelemetryMetadataContext();
            // Act
            manager.Add(context);
            // Assert
            Assert.Contains(context, manager.Telemetry);
        }

        [Test]
        public void MetadataManagerAddError()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            ErrorMetadataContext context = new ErrorMetadataContext();
            // Act
            manager.Add(context);
            // Assert
            Assert.Contains(context, manager.Errors);
        }

        [Test]
        public void MetadataManagerGetCommand()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            CommandMetadataContext context1 = new CommandMetadataContext();
            context1.Id = 5;
            manager.Add(context1);
            CommandMetadataContext context2 = new CommandMetadataContext();
            context2.Id = 10;
            manager.Add(context2);
            // Act
            var x = manager.GetCommand(5);
            // Assert
            Assert.AreEqual(x.Id, 5);
        }

        [Test]
        public void MetadataManagerGetTelemetry()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            TelemetryMetadataContext context1 = new TelemetryMetadataContext();
            context1.Id = 5;
            manager.Add(context1);
            TelemetryMetadataContext context2 = new TelemetryMetadataContext();
            context2.Id = 10;
            manager.Add(context2);
            // Act
            var x = manager.GetTelemetry(5);
            // Assert
            Assert.AreEqual(x.Id, 5);
        }

        [Test]
        public void MetadataManagerGetError()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            ErrorMetadataContext context1 = new ErrorMetadataContext();
            context1.Id = 5;
            manager.Add(context1);
            ErrorMetadataContext context2 = new ErrorMetadataContext();
            context2.Id = 10;
            manager.Add(context2);
            // Act
            var x = manager.GetError(5);
            // Assert
            Assert.AreEqual(x.Id, 5);
        }

        [Test]
        public void MetadataManagerGetMetadataCommand()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            manager.Add(new CommandMetadataContext() { Id = 1 });
            manager.Add(new CommandMetadataContext() { Id = 2 });
            manager.Add(new TelemetryMetadataContext() { Id = 3 });
            manager.Add(new TelemetryMetadataContext() { Id = 4 });
            manager.Add(new ErrorMetadataContext() { Id = 5 });
            manager.Add(new ErrorMetadataContext() { Id = 6 });
            // Act
            var x = manager.GetMetadata(1);
            // Assert
            Assert.AreEqual(x.Id, 1);
        }

        [Test]
        public void MetadataManagerGetMetadataTelemetry()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            manager.Add(new CommandMetadataContext() { Id = 1 });
            manager.Add(new CommandMetadataContext() { Id = 2 });
            manager.Add(new TelemetryMetadataContext() { Id = 3 });
            manager.Add(new TelemetryMetadataContext() { Id = 4 });
            manager.Add(new ErrorMetadataContext() { Id = 5 });
            manager.Add(new ErrorMetadataContext() { Id = 6 });
            // Act
            var x = manager.GetMetadata(3);
            // Assert
            Assert.AreEqual(x.Id, 3);
        }

        [Test]
        public void MetadataManagerGetMetadataError()
        {
            // Arrange
            var cc = new ControlCenterViewModel();
            var manager = new MetadataManager(cc);
            manager.Add(new CommandMetadataContext() { Id = 1 });
            manager.Add(new CommandMetadataContext() { Id = 2 });
            manager.Add(new TelemetryMetadataContext() { Id = 3 });
            manager.Add(new TelemetryMetadataContext() { Id = 4 });
            manager.Add(new ErrorMetadataContext() { Id = 5 });
            manager.Add(new ErrorMetadataContext() { Id = 6 });
            // Act
            var x = manager.GetMetadata(5);
            // Assert
            Assert.AreEqual(x.Id, 5);
        }
    }
}