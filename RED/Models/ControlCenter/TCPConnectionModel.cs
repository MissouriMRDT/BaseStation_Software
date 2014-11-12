namespace RED.Models.ControlCenter
{
    using System.Net.Sockets;

    public class TcpConnectionModel
    {
        internal TcpClient Client;
        internal string RemoteName;
        internal string RemoteSoftware;
    }
}
