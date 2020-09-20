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
    public class TCPEndpoint
    {
        private TcpClient client;
        private readonly ushort port;
        public readonly IPAddress serverIP;

        public TCPEndpoint(IPAddress serverIP, ushort port)
        {
            // hack for basic POC implementation
            client = new TcpClient();
            this.port = port;
            this.serverIP = serverIP;
            client.ConnectAsync(this.serverIP, this.port);
        }

        public async Task SendMessage(byte[] data)
        {
            await client.GetStream().WriteAsync(data, 0, data.Length);
        }

        public bool PacketWaiting()
        {
            return IsConnected() && client.GetStream().DataAvailable;
        }

        public bool IsConnected()
        {
            return client.Connected;
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

            return Tuple.Create(serverIP, readBuffer);
        }
    }
}
