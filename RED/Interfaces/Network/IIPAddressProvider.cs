using System.Net;

namespace RED.Interfaces.Network
{
    public interface IIPAddressProvider
    {
        IPAddress GetIPAddress(ushort dataId);
        ushort[] GetAllDataIds(IPAddress ip);
        IPAddress[] GetAllIPAddresses();
    }
}