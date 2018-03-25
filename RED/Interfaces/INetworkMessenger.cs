using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    /// <summary>
    /// This interface describes objects whose job is to pass messages back and forth between the network and 
    /// the classes in RED. Other objects will request network services of these objects such as sending data over 
    /// the network, or handling subscriptions.
    /// 
    /// This interface works together with INetworkReceiver, the latter containing the api for receiving a subscribed
    /// message over the network
    /// </summary>
    public interface INetworkMessenger
    {
        /// <summary>
        /// send a message over the network. This overload takes any object as data to send, and will 
        /// be transformed into bytes in the process.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendOverNetwork(ushort dataId, dynamic obj, bool reliable = false);

        /// <summary>
        /// send a message over the network. This overload takes any byte as data to send
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendOverNetwork(ushort dataId, byte obj, bool reliable = false);

        /// <summary>
        /// send a message over the network. This overload takes a series of bytes to send.
        /// </summary>
        /// <param name="dataId">the id to attach to the message, corresponding to rovecomm metadata ID's</param>
        /// <param name="obj">the data to send over the network</param>
        /// <param name="reliable">whether to send it via a protocol that ensures that it gets there, or to 
        /// simply broadcast the data. The former is more useful for single one off messages, the latter 
        /// for repeated messages or commands. </param>
        void SendOverNetwork(ushort dataId, byte[] data, bool reliable = false);

        /// <summary>
        /// subscribe to the network. Whenever RED receives a message with the requested dataid attached, the 
        /// subscriber will be notified and given it.
        /// </summary>
        /// <param name="subscriber">the subscriber who sends the request (objects should usually pass themselves
        /// in as this parameter)</param>
        /// <param name="dataId">the data id to subscribe to</param>
        void Subscribe(INetworkSubscriber subscriber, ushort dataId);

        /// <summary>
        /// unsubscribe to the network entirely
        /// </summary>
        /// <param name="subscriber">the subscriber who sends the request (objects should usually pass themselves
        /// in as this parameter)</param>
        void UnSubscribe(INetworkSubscriber subscriber);

        /// <summary>
        /// unsubscribe from a certain data id. The subscriber will no longer receive messages with that id, but 
        /// if it has previously subscribed with other id's, it will still receive those messages.
        /// </summary>
        /// <param name="subscriber">the subscriber who sends the request (objects should usually pass themselves
        /// in as this parameter)</param>
        /// <param name="dataId">the data id to unsubscribe to</param>
        void UnSubscribe(INetworkSubscriber subscriber, ushort dataId);
    }
}
