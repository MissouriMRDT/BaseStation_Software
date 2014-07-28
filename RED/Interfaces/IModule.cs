namespace RED.Interfaces
{
    public interface IModule
    {
        string Title { get; }
        bool InUse { get; set; }
        bool IsManageable { get; }
        void TelemetryReceiver<T>(object message);
    }
}
