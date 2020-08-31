using Core.Contexts;
using System.Net;

namespace Core.Addons.Network
{
    public class Server
    {
        public string Name { get; set; }
        public IPAddress Address { get; set; }
        public ushort TCPPort { get; set; }

        public Server(string name, IPAddress address, ushort tcpPort)
        {
            Name = name;
            Address = address;
            TCPPort = tcpPort;
        }

        public Server(MetadataServerContext context)
        {
            Name = context.Name;
            IPAddress.TryParse(context.Address, out IPAddress ip);
            Address = ip;
            ushort.TryParse(context.TCPPort, out ushort port);
            TCPPort = port;
        }
    }
}
