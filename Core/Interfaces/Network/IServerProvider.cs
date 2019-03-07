using Core.Addons.Network;
using System.Net;

namespace Core.Interfaces.Network
{
    public interface IServerProvider
    {
        Server[] GetServerList();
        Server GetServer(IPAddress ip);
    }
}
