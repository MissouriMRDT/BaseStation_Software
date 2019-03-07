using System;
using System.Net;
using System.Threading.Tasks;

namespace Core.Interfaces.Network
{
    public interface INetworkTransportProtocol
    {
        Task SendMessage(IPAddress DestIP, byte[] data);
        Task<Tuple<IPAddress, byte[]>> ReceiveMessage();
    }
}
