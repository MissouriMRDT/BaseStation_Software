namespace RED.Interfaces
{
    /// <summary>
    /// This interface describes objects who want to subscribe to the network and receive messages from it. 
    /// 
    /// This interface works together with INetworkMessenger, the latter containing the api for requesting a 
    /// subscription to the network.
    /// </summary>
    public interface INetworkSubscriber
    {
        /// <summary>
        /// The callback method that's called whenever the network has received a message that this subscriber 
        /// asked to be notified about. 
        /// </summary>
        /// <param name="dataId">The id of the message received, corresponding to rovecomm metadata ID's</param>
        /// <param name="data">The data received</param>
        /// <param name="reliable">whether or not the message was delivered reliably -- IE with ensured protocols
        /// such as tcp -- or not -- IE with broadcast protocols such as udp.</param>
        void ReceivedNetworkMessageCallback(ushort dataId, byte[] data, bool reliable);
    }
}