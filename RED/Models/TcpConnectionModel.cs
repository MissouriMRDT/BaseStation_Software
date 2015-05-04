namespace RED.Models
{
    using System.Net;
    using System.Net.Sockets;

    public class TcpConnectionModel
    {
        internal TcpClient _client;
        internal IPAddress _ipAddress;
    }
}
