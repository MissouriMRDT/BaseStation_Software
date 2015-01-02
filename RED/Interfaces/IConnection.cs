using System.IO;

namespace RED.Interfaces
{
    interface IConnection
    {
        Stream DataStream { get; }
        void Close();
    }
}