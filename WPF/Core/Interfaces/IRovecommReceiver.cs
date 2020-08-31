using Core.RoveProtocol;

namespace Core.Interfaces
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
        /// <param name="packet">The message received</param>
        /// <param name="reliable">Whether to send it reliably (TCP) or not (UDP)</param>
        void ReceivedRovecommMessageCallback(Packet packet, bool reliable);
    }
}