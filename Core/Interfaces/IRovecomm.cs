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
		/// send a rovecomm message over the network. Again. Internal overload since the ip addresses are baked into the dataid, so only
		/// certain rovecomm-system messages should call it or the network manager sends a packet with a custom ip address.
		/// </summary>
		/// <param name="dataId">dataid of the message to send</param>
		/// <param name="data">data to send</param>
		/// <param name="destIP">ip of the device to send the message to</param>
		/// <param name="reliable">whether to send it reliably (IE with a non broadcast protocol) or not</param>
		void SendCommand(Packet packet, bool reliable = false, IPAddress destIP = null);

		/// <summary>
		/// request to be notified whenever a rovecomm message comes in from the network carrying the 
		/// corresponding dataid, which is based on rovecomm metadata id's
		/// </summary>
		/// <param name="receiver">the receiver to be notified (the class should input itself as this)</param>
		/// <param name="dataId">the data id of the message you want to be notified of</param>
		void NotifyWhenMessageReceived(IRovecommReceiver receiver, string dataName);

        /// <summary>
        /// request to subscribe this computer to a device on the network so that it will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        void SubscribeTo(IPAddress deviceIP);

        /// <summary>
        /// request to subscribe this computer to all devices on the network so that they will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">the ip address of the device to request</param>
        void SubscribeToAll();
    }
}
