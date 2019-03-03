using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.Network;
using Core.Configurations;
using Core.Network;

namespace Core.Roveprotocol
{
    /// <summary>
    /// indicates the subscription status of a device on our network, IE if we're subscribed to it or not
    /// </summary>
    public enum SubscriptionStatus
    {
        Subscribed,
        Unsubscribed
    }

    /// <summary>
    /// The rove communication protocol class; it offers a variety of api for sending and receiving messages across the network 
    /// using the rovecomm protocol. Most devices used on rover use rovecomm, so use this to communicate with its devices. Rovecomm itself will encode 
    /// and decode messages between Core and devices and pass it over the network.
    /// 
    /// This class works together with MetadataManager, the latter containing all of the metadata ID's that rovecomm uses when passing messages around as well 
    /// as information such as the ip addresses of the devices. As well with NetworkManagerViewModel, Rovecomm relies on it for actual network access.
    /// </summary>
    public class Rovecomm: IRovecomm
    {
        private static Rovecomm instance;

		CommonLog log = CommonLog.Instance;

        private Rovecomm() {
            ipProvider = new MetadataManager(log, new XMLConfigManager(log));

            registrations = new Dictionary<ushort, List<IRovecommReceiver>>();
            subscriptions = new Dictionary<IPAddress, SubscriptionRecord>();

            allDeviceIPs = ipProvider.GetAllIPAddresses();

            continuousDataSocket = new UDPEndpoint(DestinationPort, DestinationPort);
            Listen();
        }

        public static Rovecomm Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Rovecomm();
                }

                return instance;
            }
        }

        private const ushort DestinationPort = 11000;
        private const ushort DestinationReliablePort = 11001;

        public const byte VersionNumber = 1;
        public const byte SubscriptionDataId = 3;
        public const byte UnSubscribeDataId = 4;

        private IPAddress[] allDeviceIPs;
        private readonly IIPAddressProvider ipProvider;
        private Dictionary<ushort, List<IRovecommReceiver>> registrations;
        // private readonly ILogger log;
        private HashSet<PendingPing> pendingPings = new HashSet<PendingPing>();
        private Dictionary<IPAddress, SubscriptionRecord> subscriptions;

        private INetworkTransportProtocol continuousDataSocket;
        private bool EnableReliablePackets = false;

        private async void Listen()
        {
            while (true)
            {
                Tuple<IPAddress, byte[]> result = await continuousDataSocket.ReceiveMessage();
                HandleReceivedPacket(result.Item1, result.Item2);
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
                log.Log("No network to send msg");
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
                            HandleReceivedPacket(destIP, response);
                        }

                        tcpConnection.Close();
                    }
                }
                catch
                {
                    log.Log($"Attempt to send reliable packet to {destIP} failed. Sending unreliable instead");
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

        /// <summary>
        /// send a rovecomm message over the network. This overload takes any object as data to send, and will 
        /// be transformed into bytes in the process.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        public void SendCommand(ushort dataId, dynamic obj, bool reliable = false)
        {

            SendCommand(dataId, System.BitConverter.GetBytes(obj), reliable);
        }

        /// <summary>
        /// send a rovecomm message over the network. This overload takes any byte as data to send
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        public void SendCommand(ushort dataId, byte obj, bool reliable = false)
        {
            SendCommand(dataId, new byte[] { obj }, reliable);
        }

        /// <summary>
        /// send a rovecomm message over the network. This overload takes a series of bytes to send.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        public void SendCommand(ushort dataId, byte[] data, bool reliable = false)
        {
            IPAddress destIP = ipProvider.GetIPAddress(dataId);
            SendPacket(dataId, data, destIP, reliable);
        }

        /// <summary>
        /// attempt to ping a device on the network using rovecomm.
        /// </summary>
        /// <param name="ip">ip of the device to ping</param>
        /// <param name="timeout">how many milliseconds to wait before timing out</param>
        /// <returns>how long it took to reply to the ping</returns>
        public async Task<TimeSpan> SendPing(IPAddress ip, TimeSpan timeout)
        {
            PendingPing ping = new PendingPing()
            {
                Timestamp = DateTime.Now,
                SeqNum = 0,
                Semaphore = new System.Threading.SemaphoreSlim(0)
            };
            pendingPings.Add(ping);

            SendPacket((ushort)SystemDataId.Ping, new byte[0], ip, ping.SeqNum, true, true);
            await ping.Semaphore.WaitAsync(timeout);
            return ping.RoundtripTime;
        }

        /// <summary>
        /// request to be notified whenever a rovecomm message comes in from the network carrying the 
        /// corresponding dataid, which is based on rovecomm metadata id's
        /// </summary>
        /// <param name="receiver">the receiver to be notified (the class should input itself as this)</param>
        /// <param name="dataId">the data id of the message you want to be notified of</param>
        public void NotifyWhenMessageReceived(IRovecommReceiver receiver, ushort dataId)
        {
            if (dataId == 0) return;
            if (registrations.TryGetValue(dataId, out List<IRovecommReceiver> existingRegistrations))
            {
                if (!existingRegistrations.Contains(receiver))
                    existingRegistrations.Add(receiver);
            }
            else
            {
                registrations.Add(dataId, new List<IRovecommReceiver> { receiver });
            }
        }

        /// <summary>
        /// request to stop being notified of receiving all rovecomm messages you previously requested to receive
        /// </summary>
        /// <param name="receiver">the reciever to stop being notified (the class should input itself as this)</param>
        public void StopReceivingNotifications(IRovecommReceiver receiver)
        {
            var registrationCopy = new Dictionary<ushort, List<IRovecommReceiver>>(registrations); //Use a copy because we may modify it while removing stuff and that breaks the foreach
            foreach (KeyValuePair<ushort, List<IRovecommReceiver>> kvp in registrationCopy)
            {
                StopReceivingNotifications(receiver, kvp.Key);
            }
        }

        /// <summary>
        /// request to stop being notified of receiving rovecomm messages that correspond to the dataid, which is 
        /// based on rovecomm metadata id's
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="dataId"></param>
        public void StopReceivingNotifications(IRovecommReceiver subscriber, ushort dataId)
        {
            if (registrations.TryGetValue(dataId, out List<IRovecommReceiver> existingRegistrations))
            {
                existingRegistrations.Remove(subscriber);
                if (existingRegistrations.Count == 0)
                {
                    registrations.Remove(dataId);
                }
            }
        }

        /// <summary>
        /// request to subscribe this computer to a device on the network so that it will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        public void SubscribeMyPCToDevice(IPAddress deviceIP)
        {
            SendPacket(SubscriptionDataId, new byte[] { 0 }, deviceIP, true);

            if (subscriptions.ContainsKey(deviceIP))
            {
                subscriptions[deviceIP].Status = SubscriptionStatus.Subscribed;
                subscriptions[deviceIP].Timestamp = DateTime.Now;
            }
            else
            {
                subscriptions.Add(deviceIP, new SubscriptionRecord(SubscriptionStatus.Subscribed, deviceIP, DateTime.Now));
            }
        }

        /// <summary>
        /// request to subscribe this computer to all rover devices on the network so that they will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        public void SubscribeMyPCToAllDevices()
        {
            foreach(IPAddress deviceIP in allDeviceIPs)
            {
                SubscribeMyPCToDevice(deviceIP);
            }

            log.Log("Telemetry Subscriptions Sent");
        }

        /// <summary>
        /// handler for when a message from the network is received. This should be tied to a network layer 'received message' event in this class's constructor.
        /// Decode the message, check to see if it's a rovecomm system ID and should be processed seperately, and finally pass the message to any other object that 
        /// has requested to be notified of a message receival with this dataid.
        /// </summary>
        /// <param name="srcIP">the source ip address</param>
        /// <param name="packet">the packet data received.</param>
        private void HandleReceivedPacket(IPAddress srcIP, byte[] packet)
        {

            byte[] data = DecodePacket(packet, out ushort dataId, out ushort seqNum);

            bool passToSubscribers = HandleSystemDataID(srcIP, data, dataId, seqNum);

            if (passToSubscribers)
            {
                NotifyReceivers(dataId, data);
            }
        }

        /// <summary>
        /// checks to see if a received message has a rovecomm system data id, IE if the protocol itself is supposed to do anything here such as
        /// replying to a ping or subscription.
        /// </summary>
        /// <param name="srcIP">the ip address of the device that sent this PC the message</param>
        /// <param name="data">the packet data</param>
        /// <param name="dataId">the dataid in the rovecomm message we received</param>
        /// <param name="seqNum">the sequence number in the rovecomm message we received</param>
        /// <returns></returns>
        private bool HandleSystemDataID(IPAddress srcIP, byte[] data, ushort dataId, ushort seqNum)
        {
            switch ((SystemDataId)dataId)
            {
                case SystemDataId.Null:
                    log.Log("Packet recieved with null dataId"); //likely means a) wasn't a message for rovecomm b) message was just gobblygook
                    break;
                case SystemDataId.Ping:
                    SendPacket((ushort)SystemDataId.PingReply, BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)seqNum)), srcIP, false);
                    break;
                case SystemDataId.PingReply:
                    ProcessPing(data);
                    break;
                case SystemDataId.Subscribe:
                    log.Log($"Packet recieved requesting subscription to dataId={dataId}");
                    break;
                case SystemDataId.Unsubscribe:
                    log.Log($"Packet recieved requesting unsubscription from dataId={dataId}");
                    break;
                default: //Regular DataId
                    return true;
            }

            return false;
        }

        /// <summary>
        /// send the received rovecomm message to any objects in this program that requested to be notified when a rovecomm message came in with a certain dataID, 
        /// if the message's dataID corresponds to their request.
        /// </summary>
        /// <param name="dataId">the dataid of the message received</param>
        /// <param name="data">the data received</param>
        /// <param name="reliable">whether or not it was sent over a 'reliable' (non broadcast or not) network protocol</param>
        private void NotifyReceivers(ushort dataId, byte[] data, bool reliable = false)
        {
            if (dataId == 0) return;
            if (registrations.TryGetValue(dataId, out List<IRovecommReceiver> registered))
                foreach (IRovecommReceiver subscription in registered)
                {
                    try
                    {
                        subscription.ReceivedRovecommMessageCallback(dataId, data, reliable);
                    }
                    catch (System.Exception e)
                    {
                        log.Log("Error parsing packet with dataid={0}{1}{2}", dataId, System.Environment.NewLine, e);
                    }
                }
        }

        /// <summary>
        /// send a rovecomm message over the network. Again. Public overload since the ip addresses are baked into the dataid anyway, so only
        /// certain rovecomm-system messages should call it or the network manager with a custom ip address.
        /// </summary>
        /// <param name="dataId">dataid of the message to send</param>
        /// <param name="data">data to send</param>
        /// <param name="destIP">ip of the device to send the message to</param>
        /// <param name="reliable">whether to send it reliably (IE with a non broadcast protocol) or not</param>
        public void SendPacket(ushort dataId, byte[] data, IPAddress destIP, bool reliable = false)
        {
			ushort seqNum = 0;
            SendPacket(dataId, data, destIP, seqNum, reliable);
        }

        /// <summary>
        /// fullest implementation of send packet. Encodes the packet into Rovecomm protocol and passes it to the network layer for sending.
        /// </summary>
        /// <param name="dataId">dataid of the message to send</param>
        /// <param name="data">data to send</param>
        /// <param name="destIP">ip of the device to send the message to</param>
        /// <param name="seqNum">sequence number of this dataID</param>
        /// <param name="reliable">whether to send it reliably (IE with a non broadcast protocol) or not</param>
        private void SendPacket(ushort dataId, byte[] data, IPAddress destIP, ushort seqNum, bool reliable = false, bool getReliableResponse = false)
        {
            if (destIP == null)
            {
                log.Log($"Attempted to send packet with unknown IP address. DataId={dataId}");
                return;
            }
            if (destIP.Equals(IPAddress.None))
            {
                log.Log($"Attempted to send packet with invalid IP address. DataId={dataId} IP={destIP}");
                return;
            }

            if (reliable)
            {
                byte[] packetData = EncodePacket(dataId, data, seqNum);
                SendPacketReliable(destIP, packetData, getReliableResponse);
            }
            else
            {
                byte[] packetData = EncodePacket(dataId, data, seqNum);
                SendPacketUnreliable(destIP, packetData);
            }
        }

        /// <summary>
        /// creates a rovecomm encoded data packet 
        /// </summary>
        /// <param name="dataId">dataid of the message to send</param>
        /// <param name="data">data to send</param>
        /// <param name="seqNum">sequence number of the message</param>
        /// <returns>an encoded rovecomm message ready for sending.</returns>
        private byte[] EncodePacket(ushort dataId, byte[] data, ushort seqNum)
        {
            try
            {
                var flags = RoveCommFlags.None;
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(VersionNumber);
                    bw.Write(IPAddress.HostToNetworkOrder((short)seqNum));
                    bw.Write((byte)flags);
                    bw.Write(IPAddress.HostToNetworkOrder((short)dataId));
                    bw.Write(IPAddress.HostToNetworkOrder((short)data.Length));
                    bw.Write(data);
                    return ms.ToArray();
                }
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("Data buffer too long.", "data", e);
            }
        }

        /// <summary>
        /// decodes a rovecomm protocol message and parses out its dataId, sequence number, and data
        /// </summary>
        /// <param name="encodedPacket">the packet to decode</param>
        /// <param name="dataId">out return that will contain the message's dataid</param>
        /// <param name="seqNum">out return that will contain the message's sequence number</param>
        /// <returns>the internal data that was packaged (not including dataId and sequence number)</returns>
        private byte[] DecodePacket(byte[] encodedPacket, out ushort dataId, out ushort seqNum)
        {
            byte versionNumber;
            ushort rawSequenceNumber;
            RoveCommFlags rawFlags;
            ushort rawDataId;
            byte[] rawData;

            using (var ms = new MemoryStream(encodedPacket))
            using (var br = new BinaryReader(ms))
            {
                versionNumber = br.ReadByte();
                if (versionNumber != VersionNumber)
                    throw new InvalidDataException("Version number of packet is not supported.");

                rawSequenceNumber = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                rawFlags = (RoveCommFlags)br.ReadByte();
                rawDataId = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                ushort dataLength = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                rawData = br.ReadBytes(dataLength);
            }

            dataId = rawDataId;
            seqNum = rawSequenceNumber;
            return rawData;
        }

        /// <summary>
        /// processes a received ping by computing how much time it took and releasing it from wait state.
        /// </summary>
        /// <param name="data"></param>
        private void ProcessPing(byte[] data)
        {
            DateTime now = DateTime.Now;
            ushort responseSeqNum = BitConverter.ToUInt16(data, 0);
            PendingPing ping = pendingPings.FirstOrDefault(x => x.SeqNum == responseSeqNum);
            if (ping == null) return;
            
            ping.RoundtripTime = now - ping.Timestamp;
            ping.Semaphore.Release();
        }

        [Flags]
        private enum RoveCommFlags : byte
        {
            None = 0b000_0000,
            ACK = 0b000_0001
        }

        private enum SystemDataId : ushort
        {
            Null = 0,
            Ping = 1,
            PingReply = 2,
            Subscribe = 3,
            Unsubscribe = 4,
            ForceUnsubscribe = 5
        }

        private class PendingPing
        {
            public ushort SeqNum;
            public DateTime Timestamp;
            public System.Threading.SemaphoreSlim Semaphore;
            public TimeSpan RoundtripTime;
        }
    }
}
