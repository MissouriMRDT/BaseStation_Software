using Caliburn.Micro;
using RED.Models;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class NetworkManagerViewModel : PropertyChangedBase, ISubscribe
    {
        private const ushort DestinationPort = 11000;
        private const bool defaultReliable = true;

        private TimeSpan ReliableRetransmissionTimeout { get { return TimeSpan.FromMilliseconds(1000); } }
        private int ReliableMaxRetries { get { return 5; } }

        private NetworkManagerModel _model;
        private ControlCenterViewModel _cc;

        private INetworkEncoding encoding;
        private INetworkTransportProtocol continuousDataSocket;
        private IIPAddressProvider ipAddressProvider;
        private ISequenceNumberProvider sequenceNumberProvider;

        private HashSet<UnACKedPacket> OutgoingUnACKed = new HashSet<UnACKedPacket>();

        public NetworkManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new NetworkManagerModel();
            _cc = cc;

            sequenceNumberProvider = new SequenceNumberManager();
            encoding = new RoverProtocol();
            continuousDataSocket = new UDPEndpoint(DestinationPort, DestinationPort);
            ipAddressProvider = cc.MetadataManager;

            foreach (var command in _cc.MetadataManager.Commands)
                _cc.DataRouter.Subscribe(this, command.Id);
            //_cc.DataRouter.Subscribe(this, 1);
            //_cc.DataRouter.Subscribe(this, 50);

            Listen();
        }

        private async void Listen()
        {
            while (true)
            {
                byte[] buffer = await continuousDataSocket.ReceiveMessage();
                ReceivePacket(buffer);
            }
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            IPAddress destIP = ipAddressProvider.GetIPAddress(dataId);
            SendPacket(dataId, data, destIP);
        }

        public void SendPacket(byte dataId, byte[] data, IPAddress destIP, bool isReliable)
        {
            if (destIP == null)
            {
                _cc.Console.WriteToConsole("Attempted to send packet with unknown IP address. DataId=" + dataId.ToString());
                return;
            }

            ushort seqNum = sequenceNumberProvider.IncrementValue(dataId);
            byte[] packetData = encoding.EncodePacket(dataId, data, seqNum);

            if (isReliable)
                SendPacketReliable(destIP, packetData, dataId, seqNum);
            else
                SendPacketUnreliable(destIP, packetData);
        }

        private async void SendPacketUnreliable(IPAddress destIP, byte[] packetData)
        {
            await continuousDataSocket.SendMessage(destIP, packetData);
        }

        private async void SendPacketReliable(IPAddress destIP, byte[] packetData, byte dataId, ushort seqNum)
        {
            var packetInfo = new UnACKedPacket() { DataId = dataId, SeqNum = seqNum };
            OutgoingUnACKed.Add(packetInfo);

            for (int i = 0; i < ReliableMaxRetries; i++)
            {
                await continuousDataSocket.SendMessage(destIP, packetData);
                await Task.Delay(ReliableRetransmissionTimeout);
                if (!OutgoingUnACKed.Contains(packetInfo)) return;
            }

            _cc.Console.WriteToConsole("No ACK recieved for reliable packet with DataId=" + packetInfo.DataId.ToString() + " and SeqNum=" + packetInfo.SeqNum.ToString() + " after " + ReliableMaxRetries + " retries.");
        }

        private void ReceivePacket(byte[] buffer)
        {
            byte dataId = 0;
            ushort seqNum;
            byte[] data = encoding.DecodePacket(buffer, out dataId, out seqNum);
            if (!sequenceNumberProvider.UpdateNewer(dataId, seqNum))
            {
                _cc.Console.WriteToConsole("Packet recieved with invalid sequence number=" + seqNum.ToString() + " DataId=" + dataId);
                return;
            }
            _cc.DataRouter.Send(dataId, data);
        }

        private struct UnACKedPacket { public byte DataId; public ushort SeqNum;}
    }
}