using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RED.Interfaces
{
    /// <summary>
    /// The rove communication protocol interface; it offers a variety of api for sending and receiving messages across the network 
    /// using the rovecomm protocol. Most devices used on rover use rovecomm, so use this to communicate with its devices. Rovecomm itself will encode 
    /// and decode messages between RED and devices and pass it over the network.
    /// </summary>
    public interface IRovecomm
    {
        /// <summary>
        /// send a rovecomm message over the network. This overload takes any object as data to send, and will 
        /// be transformed into bytes in the process.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendCommand(ushort dataId, dynamic obj, bool reliable = false);

        /// <summary>
        /// send a rovecomm message over the network. This overload takes any byte as data to send
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendCommand(ushort dataId, byte obj, bool reliable = false);

        /// <summary>
        /// send a rovecomm message over the network. This overload takes a series of bytes to send.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendCommand(ushort dataId, byte[] data, bool reliable = false);

        /// <summary>
        /// request to be notified whenever a rovecomm message comes in from the network carrying the 
        /// corresponding dataid, which is based on rovecomm metadata id's
        /// </summary>
        /// <param name="receiver">the receiver to be notified (the class should input itself as this)</param>
        /// <param name="dataId">the data id of the message you want to be notified of</param>
        void NotifyWhenMessageReceived(IRovecommReceiver receiver, ushort dataId);

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
        void StopReceivingNotifications(IRovecommReceiver subscriber, ushort dataId);

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
    }
}
