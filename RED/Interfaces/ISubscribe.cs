namespace RED.Interfaces
{
    public interface ISubscribe
    {
        void Receive(int dataCode, byte[] data);
    }
}