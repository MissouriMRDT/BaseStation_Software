using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// The rove communication protocol interface; it offers a variety of api for sending and receiving messages across the network 
    /// using the rovecomm protocol. Most devices used on rover use rovecomm, so use this to communicate with its devices. Rovecomm itself will encode 
    /// and decode messages between Core and devices and pass it over the network.
    /// </summary>
    public interface IRovecomm
    {
        /// <summary>
        /// send a rovecomm message over the network. This overload takes any object as data to send, and will 
        /// be transformed into bytes in the process.
        /// </summary>
        /// <param name="packet">the packet</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendCommand(Packet packet, bool reliable = false);

		/// <summary>
		/// send a rovecomm message over the network. Again. Internal overload since the ip addresses are baked into the dataid, so only
		/// certain rovecomm-system messages should call it or the network manager sends a packet with a custom ip address.
		/// </summary>
		/// <param name="dataId">dataid of the message to send</param>
		/// <param name="data">data to send</param>
		/// <param name="destIP">ip of the device to send the message to</param>
		/// <param name="reliable">whether to send it reliably (IE with a non broadcast protocol) or not</param>
		void SendPacket(Packet packet, IPAddress destIP, bool reliable = false, bool getReliableResponse = false);

		/// <summary>
		/// request to be notified whenever a rovecomm message comes in from the network carrying the 
		/// corresponding dataid, which is based on rovecomm metadata id's
		/// </summary>
		/// <param name="receiver">the receiver to be notified (the class should input itself as this)</param>
		/// <param name="dataId">the data id of the message you want to be notified of</param>
		void NotifyWhenMessageReceived(IRovecommReceiver receiver, string dataName);

        /// <summary>
        /// request to stop being notified of receiving all rovecomm messages you previously requested to receive
        /// </summary>
        /// <param name="receiver">the reciever to stop being notified (the class should input itself as this)</param>
        void StopReceivingNotifications(IRovecommReceiver receiver);

        /// <summary>
        /// request to stop being notified of receiving rovecomm messages that correspond to the dataid, which is 
        /// based on rovecomm metadata id's
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="dataId"></param>
        void StopReceivingNotifications(IRovecommReceiver subscriber, string dataName);

        /// <summary>
        /// request to subscribe this computer to a device on the network so that it will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        void SubscribeMyPCToDevice(IPAddress deviceIP);

        /// <summary>
        /// request to subscribe this computer to all devices on the network so that they will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        void SubscribeMyPCToAllDevices();

        /// <summary>
        /// attempt to ping a device on the network using rovecomm.
        /// </summary>
        /// <param name="ip">ip of the device to ping</param>
        /// <param name="timeout">how many milliseconds to wait before timing out</param>
        /// <returns>how long it took to reply to the ping</returns>
        Task<TimeSpan> SendPing(IPAddress ip, TimeSpan timeout);

        /// <summary>
		/// return a packet with the given index
		/// </summary>
		/// <param name="index">index of the packet to get</param>
		/// <returns></returns>
		Packet GetPacketByID(int index);
    }
}
