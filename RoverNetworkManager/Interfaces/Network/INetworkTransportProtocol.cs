using System;
using System.Net;
using System.Threading.Tasks;

namespace RoverNetworkManager.Interfaces.Network
{
    public interface INetworkTransportProtocol
    {
        Task SendMessage(IPAddress DestIP, byte[] data);
        Task<Tuple<IPAddress, byte[]>> ReceiveMessage();
    }
}
