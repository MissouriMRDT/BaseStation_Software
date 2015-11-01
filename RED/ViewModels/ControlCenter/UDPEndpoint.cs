using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class UDPEndpoint : INetworkTransportProtocol
    {
        private UdpClient client;
        private ushort destPort;

        public UDPEndpoint(ushort destPort)
        {
            client = new UdpClient();
            this.destPort = destPort;
        }

        public async Task SendMessage(IPAddress destIP, byte[] data)
        {
            await client.SendAsync(data, data.Length, new IPEndPoint(destIP, destPort));
        }

        public async Task<byte[]> ReceiveMessage()
        {
            return (await client.ReceiveAsync()).Buffer;
        }
    }
}
