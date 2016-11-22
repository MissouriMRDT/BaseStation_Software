namespace RED.Interfaces
{
    public interface IDataRouter
    {
        void Send(ushort dataId, dynamic obj);
        void Send(ushort dataId, byte obj);
        void Send(ushort dataId, byte[] data);
        void Subscribe(ISubscribe subscriber, ushort dataId);
        void UnSubscribe(ISubscribe subscriber);
        void UnSubscribe(ISubscribe subscriber, ushort dataId);
    }
}
