namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void ReceiveFromRouter(ushort dataId, byte[] data, bool reliable);
    }
}