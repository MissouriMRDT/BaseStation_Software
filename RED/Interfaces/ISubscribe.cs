namespace RED.Interfaces
{
    public interface iSubscribe
    {
        void Receive(int dataCode, byte[] data);
    }
}