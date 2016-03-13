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
        private ushort remotePort;
        private ushort localPort;

        public UDPEndpoint(ushort localPort, ushort remotePort)
        {
            client = new UdpClient(localPort);
            this.localPort = localPort;
            this.remotePort = remotePort;
        }

        public async Task SendMessage(IPAddress destIP, byte[] data)
        {
            await client.SendAsync(data, data.Length, new IPEndPoint(destIP, remotePort));
        }

        public async Task<byte[]> ReceiveMessage()
        {
            return (await client.ReceiveAsync()).Buffer;
        }
    }
}
