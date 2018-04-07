using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Network;
using RED.Models.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RED.ViewModels.Network
{
    public class NetworkManagerViewModel : PropertyChangedBase
    {
        private const ushort DestinationPort = 11000;
        private const bool defaultReliable = false;

        private readonly TimeSpan ReliableRetransmissionTimeout = TimeSpan.FromMilliseconds(1000);
        private const int ReliableMaxRetries = 5;

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
        public event Action<IPAddress> TelemetryRecieved;

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
            await continuousDataSocket.SendMessage(destIP, packetData);
        }

        public async void SendPacketReliable(IPAddress destIP, byte[] packetData, ushort dataId, ushort seqNum)
        {
            if(!EnableReliablePackets)
            {
                _log.Log($"Reliable packets not enabled. Sending command for DataId={dataId} unreliably");
                SendPacketUnreliable(destIP, packetData);
            }

            //deprecated due to buggy implementation until we fix it later
            /*var packetInfo = new UnACKedPacket() { DataId = dataId };
            outgoingUnACKed.Add(packetInfo);

            for (int i = 0; i < ReliableMaxRetries; i++)
            {
                await continuousDataSocket.SendMessage(destIP, packetData);
                await Task.Delay(ReliableRetransmissionTimeout);
                if (!outgoingUnACKed.Contains(packetInfo)) return;
            }

            outgoingUnACKed.Remove(packetInfo);
            if (dataId != (ushort)SystemDataId.Subscribe)
                _log.Log($"No ACK recieved for DataId={packetInfo.DataId} after {ReliableMaxRetries} retries");U\*/
        }

        private void ReceivePacket(IPAddress srcIP, byte[] buffer)
        {
            TelemetryRecieved?.Invoke(srcIP);
            PacketReceived?.Invoke(srcIP, buffer);
        }
    }
}