using System.Net;
using Core.RoveProtocol;

namespace Core.Interfaces
{
    /// <summary>
    /// The rove communication protocol interface.
    /// It offers a api for sending and receiving messages across the network using the rovecomm protocol. 
    /// Most devices used on rover use rovecomm, so use this to communicate with its devices. Rovecomm 
    /// implementations will reference an encoder/decoder to encode and decode messages from the network.
    /// </summary>
    public interface IRovecomm
    {

        /// <summary>
        /// Send a rovecomm message over the network.
        /// </summary>
        /// <param name="packet">Packet of the message to send</param>
        /// <param name="reliable">Whether to send it reliably (TCP) or not (UDP)</param>
        /// <param name="destIP">IP of the device to send the message to</param>
        void SendCommand(Packet packet, bool reliable = false, IPAddress destIP = null);

		/// <summary>
		/// Request for view models to be notified whenever a rovecomm message comes in from the network 
        /// carrying the corresponding dataid
		/// </summary>
		/// <param name="receiver">Receiver to be notified (typically, "this")</param>
		/// <param name="dataId">Data id of the message you want to be notified of</param>
		void NotifyWhenMessageReceived(IRovecommReceiver receiver, string dataName);

        /// <summary>
        /// Request to subscribe to a device on the network so that it will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        /// <param name="deviceIP">IP address of the device to request</param>
        void SubscribeTo(IPAddress deviceIP);

        /// <summary>
        /// Request to subscribe to all devices on the network so that they will send this pc rovecomm
        /// messages. This must be called before any rovecomm streams can be received by this computer.
        /// </summary>
        void SubscribeToAll();
    }
}
