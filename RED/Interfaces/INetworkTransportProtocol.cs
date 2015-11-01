using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface INetworkTransportProtocol
    {
        Task SendMessage(IPAddress DestIP, byte[] data);
        Task<byte[]> ReceiveMessage();
    }
}
