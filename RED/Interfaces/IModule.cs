namespace RED.Interfaces
{
    using RoverComs;

    public interface IModule
    {
        string Title { get; }
        bool InUse { get; set; }
        bool IsManageable { get; }
        void TelemetryReceiver<T>(IProtocol<T> message);
    }
}
