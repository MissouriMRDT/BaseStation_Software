namespace RED.Interfaces
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
    }
}
