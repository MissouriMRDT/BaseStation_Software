namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void Receive(byte dataId, byte[] data);
    }
}