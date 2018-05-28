using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Network;
using RED.Models.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RED.ViewModels.Network
{
    public class NetworkManagerViewModel : PropertyChangedBase
    {
        private const ushort DestinationPort = 11000;
        private const ushort DestinationReliablePort = 11001;

        private readonly NetworkManagerModel _model;
        private readonly ILogger _log;

        private INetworkTransportProtocol continuousDataSocket;

        public Dictionary<ushort, List<IRovecommReceiver>> Registrations
        {
            get
            {
                return _model._registrations;
            }
        }

        public bool EnableReliablePackets
        {
            get
            {
                return _model.EnableReliablePackets;
            }
            set
            {
                _model.EnableReliablePackets = value;
                NotifyOfPropertyChange(() => EnableReliablePackets);
            }
        }

        public event Action<IPAddress, byte[]> PacketReceived;

        public NetworkManagerViewModel(ILogger log)
        {
            _model = new NetworkManagerModel();
            _log = log;

            continuousDataSocket = new UDPEndpoint(DestinationPort, DestinationPort);
            Listen();
        }

        private async void Listen()
        {
            while (true)
            {
                Tuple<IPAddress, byte[]> result = await continuousDataSocket.ReceiveMessage();
                ReceivePacket(result.Item1, result.Item2);
            }
        }

        public async void SendPacketUnreliable(IPAddress destIP, byte[] packetData)
        {
            try
            {
                await continuousDataSocket.SendMessage(destIP, packetData);
            }
            catch
            {
                _log.Log("No network to send msg");
            }
        }

        public async void SendPacketReliable(IPAddress destIP, byte[] packetData, bool getResponse = false)
        {
            if (!EnableReliablePackets)
            {
                //_log.Log($"Reliable packets not enabled. Sending command for destIP={destIP} unreliably");
                SendPacketUnreliable(destIP, packetData);
            }
            else
            {
                byte[] response;

                try
                {
                    using (TcpClient tcpConnection = new TcpClient())
                    {
                        //no timeouts for now. We might want to implement something later to check to see if there's
                        //even a connection to be had so we aren't spawning unending threads when we aren't connected
                        //to rover. Or not.
                        await tcpConnection.ConnectAsync(destIP, DestinationReliablePort);
                        await Task.Delay(25); //boards can get overwhelmed and fault if done too quick.
                        await tcpConnection.GetStream().WriteAsync(packetData, 0, packetData.Length);
                        await Task.Delay(25);
                        if (getResponse)
                        {
                            response = await ReadTcpPacket(tcpConnection);
                            ReceivePacket(destIP, response);
                        }

                        tcpConnection.Close();
                    }
                }
                catch
                {
                    _log.Log($"Attempt to send reliable packet to {destIP} failed. Sending unreliable instead");
                    SendPacketUnreliable(destIP, packetData);
                }
            }
        }

        private async Task<byte[]> ReadTcpPacket(TcpClient client)
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

            return readBuffer;
        }

        private void ReceivePacket(IPAddress srcIP, byte[] buffer)
        {
            PacketReceived?.Invoke(srcIP, buffer);
        }
    }
}