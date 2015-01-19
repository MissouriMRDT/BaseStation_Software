using System.IO;

namespace RED.Interfaces
{
    public interface IConnection
    {
        Stream DataStream { get; }
        void Close();
    }
}