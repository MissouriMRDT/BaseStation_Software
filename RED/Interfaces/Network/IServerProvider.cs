using RED.Addons.Network;
using System.Net;

namespace RED.Interfaces.Network
{
    public interface IServerProvider
    {
        Server[] GetServerList();
        Server GetServer(IPAddress ip);
    }
}
