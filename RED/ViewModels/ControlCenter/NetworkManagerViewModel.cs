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
        private const bool defaultReliable = false;

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
            //_cc.DataRouter.Subscribe(this, 180);

            Listen();
        }

        private async void Listen()
        {
            while (true)
            {
                byte[] buffer = await continuousDataSocket.ReceiveMessage();
                ReceivePacket(IPAddress.None, buffer);
            }
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            IPAddress destIP = ipAddressProvider.GetIPAddress(dataId);
            SendPacket(dataId, data, destIP, defaultReliable);
        }

        public void SendPacket(byte dataId, byte[] data, IPAddress destIP, bool isReliable)
        {
            if (destIP == null)
            {
                _cc.Console.WriteToConsole("Attempted to send packet with unknown IP address. DataId=" + dataId.ToString());
                return;
            }

            ushort seqNum = sequenceNumberProvider.IncrementValue(dataId);
            byte[] packetData = encoding.EncodePacket(dataId, data, seqNum, isReliable);

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

        private void ReceivePacket(IPAddress srcIP, byte[] buffer)
        {
            byte dataId;
            ushort seqNum;
            bool needsACK;
            byte[] data = encoding.DecodePacket(buffer, out dataId, out seqNum, out needsACK);
            if (!sequenceNumberProvider.UpdateNewer(dataId, seqNum))
            {
                _cc.Console.WriteToConsole("Packet recieved with invalid sequence number=" + seqNum.ToString() + " DataId=" + dataId);
                return;
            }
            if (needsACK)
                SendAck(srcIP, dataId, seqNum);
            InterpretDataId(srcIP, data, dataId, seqNum);
        }

        private void InterpretDataId(IPAddress srcIP, byte[] data, byte dataId, ushort seqNum)
        {
            switch ((SystemDataId)dataId)
            {
                case SystemDataId.Null:
                    _cc.Console.WriteToConsole("Packet recieved with null dataId.");
                    break;
                case SystemDataId.Ping:
                    SendPacket((byte)SystemDataId.PingReply, BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)seqNum)), srcIP, false);
                    break;
                case SystemDataId.Subscribe:
                    _cc.Console.WriteToConsole("Packet recieved requesting subscription to dataId=" + dataId.ToString());
                    break;
                case SystemDataId.Unsubscribe:
                    _cc.Console.WriteToConsole("Packet recieved requesting unsubscription from dataId=" + dataId.ToString());
                    break;
                case SystemDataId.ACK:
                    byte ackedId = (byte)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 0));
                    ushort ackedSeqNum = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 2));
                    if (!OutgoingUnACKed.Remove(new UnACKedPacket { DataId = dataId, SeqNum = seqNum }))
                        _cc.Console.WriteToConsole("Unexected ACK recieved from ip=??? with dataId=" + ackedId.ToString() + " and seqNum=" + ackedSeqNum.ToString() + ".");
                    break;
                default: //Regular DataId
                    _cc.DataRouter.Send(dataId, data);
                    break;
            }
        }

        private void SendAck(IPAddress srcIP, byte dataId, ushort seqNum)
        {
            byte[] ackedId = BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)(ushort)dataId));
            byte[] ackedSeqNum = BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)seqNum));
            byte[] data = new byte[4]; data.CopyTo(ackedId, 0); data.CopyTo(ackedSeqNum, ackedId.Length);
            SendPacket((byte)SystemDataId.ACK, data, srcIP, false);
        }

        private enum SystemDataId : ushort
        {
            Null = 0,
            Ping = 1,
            PingReply = 2,
            Subscribe = 3,
            Unsubscribe = 4,
            ForceUnsubscribe = 5,
            ACK = 6
        }

        private struct UnACKedPacket { public byte DataId; public ushort SeqNum;}
    }
}