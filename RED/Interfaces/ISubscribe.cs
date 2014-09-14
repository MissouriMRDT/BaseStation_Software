namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void Receive(int dataId, byte[] data);
    }
}