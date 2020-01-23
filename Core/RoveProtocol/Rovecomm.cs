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
using Core.Addons.Network;

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
    /// The rove communication protocol class; it offers the api for sending and receiving messages
    /// across the network using the rovecomm protocol. Most devices used on rover use rovecomm, so 
    /// use this to communicate with its devices. Rovecomm itself will encode and decode messages 
    /// and pass it over the network.
    /// 
    /// This class works together with MetadataManager, the latter containing all of the metadata 
    /// ID's that rovecomm uses when passing messages around as well as information such as the ip 
    /// addresses of the devices.
    /// 
    /// This implementation uses RovecommTwo to encode/decode packets internaly, and the two 
    /// endpoint classes to deliver and recieve the raw packets.
    /// </summary>
    public class Rovecomm: IRovecomm
    {
        private static Rovecomm instance;

        private Rovecomm() {
            metadataManager = new MetadataManager(log, new XMLConfigManager(log));

            registrations = new Dictionary<string, List<IRovecommReceiver>>();
            subscriptions = new Dictionary<IPAddress, SubscriptionRecord>();

            allDevices = metadataManager.GetServerList();

            networkClientUDP = new UDPEndpoint(DestinationPort, DestinationPort);
            networkTCPClients = new List<TCPEndpoint>();

            // Listen for packets from the network
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

        private readonly Server[] allDevices;
        private readonly MetadataManager metadataManager;
        private readonly CommonLog log = CommonLog.Instance;
        private Dictionary<string, List<IRovecommReceiver>> registrations;
        private Dictionary<IPAddress, SubscriptionRecord> subscriptions;

        private UDPEndpoint networkClientUDP;
        private List<TCPEndpoint> networkTCPClients;
        private const bool EnableReliablePackets = false;

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
                foreach(TCPEndpoint client in networkTCPClients)
                {
                    if(client.PacketWaiting())
                    {
                        Tuple<IPAddress, byte[]> result = await client.ReceiveMessage();
                        HandleReceivedPacket(result.Item1, result.Item2);
                    }
                }

                await Task.Delay(10);
            }
        }

        /// <summary>
        /// Method for ViewModels to request to be notified whenever a rovecomm message comes in
        /// from the network carrying the corresponding dataName (dataId)
        /// </summary>
        /// <param name="receiver">The receiver to be notified (typically, "this")</param>
        /// <param name="dataName">The dataName of the message to be notified of</param>
        public void NotifyWhenMessageReceived(IRovecommReceiver receiver, string dataName)
        {
            // Verify it isn't an empty string
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
        /// Request to subscribe this computer to a device on the network so that it will send this
        /// pc rovecomm messages. This must be called before any rovecomm streams can be received 
        /// by this computer.
        /// </summary>
        /// <param name="deviceIP">The ip address of the device to request</param>
        public void SubscribeTo(IPAddress deviceIP)
        {
            SendCommand(Packet.Create("Subscribe"), true, deviceIP);

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
        /// Request to subscribe this computer to all known rover devices on the network so that
        /// they will send this pc rovecomm messages.
        /// </summary>
        public void SubscribeToAll()
        {
            networkTCPClients.RemoveRange(0, networkTCPClients.Count);

            foreach (Server device in allDevices)
            {
                SubscribeTo(device.Address);
                networkTCPClients.Add(new TCPEndpoint(device.Address, device.TCPPort));
            }
            
            log.Log("Telemetry Subscriptions Sent");
        }

        /// <summary>
        /// This method handles messages from the network when they are received, and distributes 
        /// the data to the rest of the application subscribers. 
        /// 
        /// Steps contained include:
        /// Decode the message into a "packet".
        /// Switch on the packet's name to determine if it is a system packet and act accordingly.
        /// If it passes though without being a system packet, pass to internal subscribers.
        /// </summary>
        /// <param name="srcIP">The source ip address</param>
        /// <param name="packet">The packet data received</param>
        private void HandleReceivedPacket(IPAddress srcIP, byte[] encodedPacket)
        {
            Packet packet = RovecommTwo.DecodePacket(encodedPacket, metadataManager);

            switch (packet.Name)
            {
                case "Null":
                    log.Log("Packet recieved with null dataId"); //likely means a) wasn't a message for rovecomm b) message was just gobblygook
                    break;
                case null:
                    log.Log("Packet recieved with null name");
                    break;
                case "Ping":
                    SendCommand(Packet.Create("PingReply"), false, srcIP);
                    break;
                case "Subscribe":
                    log.Log($"Packet recieved requesting subscription to dataId={packet.Name}");
                    break;
                case "Unsubscribe":
                    log.Log($"Packet recieved requesting unsubscription from dataId={packet.Name}");
                    break;
                default: //Regular DataId

                    if (registrations.TryGetValue(packet.Name, out List<IRovecommReceiver> registered))
                    {
                        foreach (IRovecommReceiver subscription in registered)
                        {
                            try
                            {
                                subscription.ReceivedRovecommMessageCallback(packet, false);
                            }
                            catch (System.Exception e)
                            {
                                log.Log("Error parsing packet with dataid={0}{1}{2}", packet.DataType, System.Environment.NewLine, e);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// This method provides access to send a packet to a device on the network.
        /// 
        /// Steps contained include:
        /// Check the destination IP, and if it's null try to populate from metadata.
        ///     If unsuccessful, return out
        /// Check if the destination IP is none, and if it is return out
        /// Encode the packet into RovecommTwo
        /// If the call requests reliable and reliable is enabled, send using TCP
        /// If not, send using UDP
        /// </summary>
        /// <param name="packet">Packet to send</param>
        /// <param name="reliable">Whether to send it reliably (TCP)</param>
        /// <param name="destIP">IP of the device to send the message to</param>
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
                foreach(TCPEndpoint client in networkTCPClients)
                {
                    if(client.serverIP.Equals(destIP))
                    {
                        await client.SendMessage(packetData);
                        return;
                    }
                }

                throw new Exception();
            }
            catch
            {
                log.Log($"Attempt to send reliable packet to {destIP} failed. Sending unreliable instead");

                // We should still send information, so try again with UDP
                SendPacketUnreliable(destIP, packetData);
            }

        }
    }
}
