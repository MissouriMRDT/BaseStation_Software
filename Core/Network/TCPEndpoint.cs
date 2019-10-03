using Core.Interfaces.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Network
{
    public class TCPEndpoint : INetworkTransportProtocol
    {
        private TcpClient client;
        private ushort remotePort;
        private ushort localPort;

        public TCPEndpoint(ushort localPort, ushort remotePort)
        {
            // hack for basic POC implementation
            client = new TcpClient();
            this.localPort = localPort;
            this.remotePort = remotePort;
        }

        public async Task SendMessage(IPAddress destIP, byte[] data)
        {
            // you can probably rejse the connection
            await client.ConnectAsync(destIP, remotePort);
            await Task.Delay(25); //boards can get overwhelmed and fault if done too quick.
            await client.GetStream().WriteAsync(data, 0, data.Length);
        }

        public async Task<Tuple<IPAddress, byte[]>> ReceiveMessage()
        {
            NetworkStream networkStream = client.GetStream();
            byte[] readBuffer;

            readBuffer = new byte[client.ReceiveBufferSize];
            using (var writer = new MemoryStream())
            {
                do
                {
                    int numberOfBytesRead = await networkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    writer.Write(readBuffer, 0, numberOfBytesRead);
                } while (networkStream.DataAvailable);
            }

            return Tuple.Create(IPAddress.Parse(client.Client.RemoteEndPoint.ToString()), readBuffer);
        }
    }
}
