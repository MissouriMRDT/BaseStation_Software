namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void ReceivedNetworkMessageCallback(ushort dataId, byte[] data, bool reliable);
    }
}