using RoverNetworkManager.Addons.Network;
using System.Net;

namespace RoverNetworkManager.Interfaces.Network
{
    public interface IServerProvider
    {
        Server[] GetServerList();
        Server GetServer(IPAddress ip);
    }
}
