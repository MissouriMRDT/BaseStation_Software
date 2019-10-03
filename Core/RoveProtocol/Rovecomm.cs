using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.Network;
using Core.Configurations;
using Core.Network;
using System.IO;
using Core.Models;

namespace Core.RoveProtocol
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
            metadataManager = new MetadataManager(log, new XMLConfigManager(log));

            registrations = new Dictionary<string, List<IRovecommReceiver>>();
            subscriptions = new Dictionary<IPAddress, SubscriptionRecord>();

            allDeviceIPs = metadataManager.GetAllIPAddresses();

            networkClientUDP = new UDPEndpoint(DestinationPort, DestinationPort);
            networkClientTCP = new TCPEndpoint(DestinationPort, DestinationPort);

            _packet = new Packet("Empty");

            UDPListen();
            TCPListen();
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

        private Packet _packet;

        private readonly IPAddress[] allDeviceIPs;
        private readonly MetadataManager metadataManager;
        private Dictionary<string, List<IRovecommReceiver>> registrations;
        // private readonly ILogger log;
        private Dictionary<IPAddress, SubscriptionRecord> subscriptions;

        private INetworkTransportProtocol networkClientUDP;
        private INetworkTransportProtocol networkClientTCP;
        private readonly bool EnableReliablePackets = false;

        private async void UDPListen()
        {
            while (true)
            {
                Tuple<IPAddress, byte[]> result = await networkClientUDP.ReceiveMessage();
                HandleReceivedPacket(result.Item1, result.Item2);
            }
        }

        private async void TCPListen()
        {
            while (true)
            {
                Tuple<IPAddress, byte[]> result = await networkClientTCP.ReceiveMessage();
                HandleReceivedPacket(result.Item1, result.Item2);
            }
        }


        public async void SendPacketUnreliable(IPAddress destIP, byte[] packetData)
        {
            try
            {
                await networkClientUDP.SendMessage(destIP, packetData);
            }
            catch
            {
                log.Log("No network to send msg");
            }
        }

        public async void SendPacketReliable(IPAddress destIP, byte[] packetData)
        {
            try
            {
                await networkClientTCP.SendMessage(destIP, packetData);
            }
            catch
            {
                log.Log($"Attempt to send reliable packet to {destIP} failed. Sending unreliable instead");
                SendPacketUnreliable(destIP, packetData);
            }
            
        }

        /// <summary>
        /// request to be notified whenever a rovecomm message comes in from the network carrying the 
        /// corresponding dataid, which is based on rovecomm metadata id's
        /// </summary>
        /// <param name="receiver">the receiver to be notified (the class should input itself as this)</param>
        /// <param name="dataId">the data id of the message you want to be notified of</param>
        public void NotifyWhenMessageReceived(IRovecommReceiver receiver, string dataName)
        {
            if (dataName == null) return;

            if (registrations.TryGetValue(dataName, out List<IRovecommReceiver> existingRegistrations))
            {
                if (!existingRegistrations.Contains(receiver))
                    existingRegistrations.Add(receiver);
            }
            else
            {
                registrations.Add(dataName, new List<IRovecommReceiver> { receiver });
            }
        }

        /// <summary>
        /// request to subscribe this computer to a device on the network so that it will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        public void SubscribeTo(IPAddress deviceIP)
        {
            SendCommand(new Packet("Subscribe"), true, deviceIP);

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
        public void SubscribeToAll()
        {
            foreach(IPAddress deviceIP in allDeviceIPs)
            {
                SubscribeTo(deviceIP);
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
        private void HandleReceivedPacket(IPAddress srcIP, byte[] encodedPacket)
        {
            RovecommTwo.DecodePacket(encodedPacket, metadataManager, _packet);

            switch (_packet.Name)
            {
                case "Null":
                    log.Log("Packet recieved with null dataId"); //likely means a) wasn't a message for rovecomm b) message was just gobblygook
                    break;
                case null:
                    log.Log("Packet recieved with null name");
                    break;
                case "Ping":
                    SendCommand(new Packet("PingReply"), false, srcIP);
                    break;
                case "Subscribe":
                    log.Log($"Packet recieved requesting subscription to dataId={_packet.Name}");
                    break;
                case "Unsubscribe":
                    log.Log($"Packet recieved requesting unsubscription from dataId={_packet.Name}");
                    break;
                default: //Regular DataId

                    if (registrations.TryGetValue(_packet.Name, out List<IRovecommReceiver> registered))
                    {
                        foreach (IRovecommReceiver subscription in registered)
                        {
                            try
                            {
                                subscription.ReceivedRovecommMessageCallback(_packet, false);
                            }
                            catch (System.Exception e)
                            {
                                log.Log("Error parsing packet with dataid={0}{1}{2}", _packet.DataType, System.Environment.NewLine, e);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// fullest implementation of send packet. Encodes the packet into Rovecomm protocol and passes it to the network layer for sending.
        /// </summary>
        /// <param name="packet">packet to send</param>
        /// <param name="destIP">ip of the device to send the message to</param>
        /// <param name="reliable">whether to send it reliably (IE with a non broadcast protocol) or not</param>
        public void SendCommand(Packet packet, bool reliable = false, IPAddress destIP = null)
        {
            if(destIP == null)
            {
                destIP = metadataManager.GetIPAddress(metadataManager.GetId(packet.Name));
                if(destIP == null)
                {
                    log.Log($"Attempted to send packet with unknown IP address. DataId={packet.Name}");
                    return;
                }
            }
            if(destIP.Equals(IPAddress.None))
            {
                log.Log($"Attempted to send packet with invalid IP address. DataId={packet.Name} IP={destIP}");
                return;
            }

            byte[] packetData = RovecommTwo.EncodePacket(packet, metadataManager);

            if (reliable && EnableReliablePackets)
            {
                SendPacketReliable(destIP, packetData);
            }
            else
            {
                SendPacketUnreliable(destIP, packetData);
            }
        }
    }
}
