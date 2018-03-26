namespace RED.Interfaces
{
    /// <summary>
    /// This interface describes objects who want to receive rovecomm messages from the network.
    /// 
    /// This interface works together with IRovecomm, the latter containing the api for requesting to be notified
    /// when rovecomm messages come in from the network.
    /// </summary>
    public interface IRovecommReceiver
    {
        /// <summary>
        /// The callback method that's called whenever the network has received a rovecomm message that this receiver 
        /// asked to be notified about. Rovecomm will call this function and give it the dataid and data that 
        /// was in the message it received.
        /// </summary>
        /// <param name="dataId">The id of the message received, corresponding to rovecomm metadata ID's</param>
        /// <param name="data">The data received</param>
        /// <param name="reliable">whether or not the message was delivered reliably -- IE with ensured protocols
        /// such as tcp -- or not -- IE with broadcast protocols such as udp.</param>
        void ReceivedRovecommMessageCallback(ushort dataId, byte[] data, bool reliable);
    }
}