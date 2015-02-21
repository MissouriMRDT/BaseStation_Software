namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void ReceiveFromRouter(byte dataId, byte[] data);
    }
}