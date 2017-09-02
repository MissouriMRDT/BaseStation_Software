using RED.Interfaces.Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RED.ViewModels.Network
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

        public async Task<Tuple<IPAddress, byte[]>> ReceiveMessage()
        {
            var result = await client.ReceiveAsync();
            return Tuple.Create(result.RemoteEndPoint.Address, result.Buffer);
        }
    }
}
