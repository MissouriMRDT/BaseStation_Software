using RED.Contexts;
using System.Net;

namespace RED.Addons.Network
{
    public class Server
    {
        public string Name { get; set; }
        public IPAddress Address { get; set; }

        public Server(string name, IPAddress address)
        {
            Name = name;
            Address = address;
        }

        public Server(MetadataServerContext context)
        {
            Name = context.Name;
            IPAddress.TryParse(context.Address, out IPAddress ip);
            Address = ip;
        }
    }
}
