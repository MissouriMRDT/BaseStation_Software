namespace RED.Models
{
    using System.Net.Sockets;

    public class TcpConnectionModel
    {
        internal TcpClient _client;
        internal string _remoteName;
        internal string _remoteSoftware;
    }
}
