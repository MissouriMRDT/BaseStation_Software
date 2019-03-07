using System.Net;

namespace RoverNetworkManager.Interfaces.Network
{
    public interface IIPAddressProvider
    {
        IPAddress GetIPAddress(ushort dataId);
        ushort[] GetAllDataIds(IPAddress ip);
        IPAddress[] GetAllIPAddresses();
    }
}