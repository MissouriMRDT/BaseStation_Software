﻿using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Interfaces.Network;
using RED.Models.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RED.ViewModels.Network
{
    public class NetworkManagerViewModel : PropertyChangedBase, RED.Interfaces.ISubscribe
    {
        private const ushort DestinationPort = 11000;
        private const bool defaultReliable = false;

        private TimeSpan ReliableRetransmissionTimeout { get { return TimeSpan.FromMilliseconds(1000); } }
        private int ReliableMaxRetries { get { return 5; } }

        private NetworkManagerModel _model;
        private IDataRouter _router;
        private ILogger _log;
        private IIPAddressProvider _ipProvider;

        private INetworkEncoding encoding;
        private INetworkTransportProtocol continuousDataSocket;
        private ISequenceNumberProvider sequenceNumberProvider;

        private HashSet<UnACKedPacket> OutgoingUnACKed = new HashSet<UnACKedPacket>();

        public NetworkManagerViewModel(IDataRouter router, MetadataRecordContext[] commands, ILogger log, IIPAddressProvider ipProvider)
        {
            _model = new NetworkManagerModel();
            _router = router;
            _log = log;
            _ipProvider = ipProvider;

            sequenceNumberProvider = new SequenceNumberManager();
            encoding = new RoverProtocol();
            continuousDataSocket = new UDPEndpoint(DestinationPort, DestinationPort);

            foreach (var command in commands)
                _router.Subscribe(this, command.Id);
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

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            IPAddress destIP = _ipProvider.GetIPAddress(dataId);
            SendPacket(dataId, data, destIP, defaultReliable);
        }

        public void SendPacket(ushort dataId, byte[] data, IPAddress destIP, bool isReliable)
        {
            if (destIP == null)
            {
                _log.Log("Attempted to send packet with unknown IP address. DataId={0}", dataId);
                return;
            }
            if (destIP.Equals(IPAddress.None))
            {
                _log.Log("Attempted to send packet with invalid IP address. DataId={0} IP={1}", dataId, destIP);
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

        private async void SendPacketReliable(IPAddress destIP, byte[] packetData, ushort dataId, ushort seqNum)
        {
            var packetInfo = new UnACKedPacket() { DataId = dataId, SeqNum = seqNum };
            OutgoingUnACKed.Add(packetInfo);

            for (int i = 0; i < ReliableMaxRetries; i++)
            {
                await continuousDataSocket.SendMessage(destIP, packetData);
                await Task.Delay(ReliableRetransmissionTimeout);
                if (!OutgoingUnACKed.Contains(packetInfo)) return;
            }

            _log.Log("No ACK recieved for reliable packet with DataId={0} and SeqNum={1} after {2} retries.", packetInfo.DataId, packetInfo.SeqNum, ReliableMaxRetries);
        }

        private void ReceivePacket(IPAddress srcIP, byte[] buffer)
        {
            ushort dataId;
            ushort seqNum;
            bool needsACK;
            byte[] data = encoding.DecodePacket(buffer, out dataId, out seqNum, out needsACK);
            if (!sequenceNumberProvider.UpdateNewer(dataId, seqNum))
            {
                _log.Log("Packet recieved with invalid sequence number={0} DataId={1}", seqNum, dataId);
                return;
            }
            if (needsACK)
                SendAck(srcIP, dataId, seqNum);
            InterpretDataId(srcIP, data, dataId, seqNum);
        }

        private void InterpretDataId(IPAddress srcIP, byte[] data, ushort dataId, ushort seqNum)
        {
            switch ((SystemDataId)dataId)
            {
                case SystemDataId.Null:
                    _log.Log("Packet recieved with null dataId.");
                    break;
                case SystemDataId.Ping:
                    SendPacket((ushort)SystemDataId.PingReply, BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)seqNum)), srcIP, false);
                    break;
                case SystemDataId.Subscribe:
                    _log.Log("Packet recieved requesting subscription to dataId={0}", dataId);
                    break;
                case SystemDataId.Unsubscribe:
                    _log.Log("Packet recieved requesting unsubscription from dataId={0}", dataId);
                    break;
                case SystemDataId.ACK:
                    ushort ackedId = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 0));
                    ushort ackedSeqNum = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, 2));
                    if (!OutgoingUnACKed.Remove(new UnACKedPacket { DataId = dataId, SeqNum = seqNum }))
                        _log.Log("Unexected ACK recieved from ip=??? with dataId={0} and seqNum={1}.", ackedId, ackedSeqNum);
                    break;
                default: //Regular DataId
                    _router.Send(dataId, data);
                    break;
            }
        }

        private void SendAck(IPAddress srcIP, ushort dataId, ushort seqNum)
        {
            byte[] ackedId = BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)dataId));
            byte[] ackedSeqNum = BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)seqNum));
            byte[] data = new byte[4]; data.CopyTo(ackedId, 0); data.CopyTo(ackedSeqNum, ackedId.Length);
            SendPacket((ushort)SystemDataId.ACK, data, srcIP, false);
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

        private struct UnACKedPacket { public ushort DataId; public ushort SeqNum;}
    }
}