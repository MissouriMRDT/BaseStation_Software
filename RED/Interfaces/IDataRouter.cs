namespace RED.Interfaces
{
    public interface IDataRouter
    {
        void Send(ushort dataId, dynamic obj, bool reliable = false);
        void Send(ushort dataId, byte obj, bool reliable = false);
        void Send(ushort dataId, byte[] data, bool reliable = false);
        void Subscribe(ISubscribe subscriber, ushort dataId);
        void UnSubscribe(ISubscribe subscriber);
        void UnSubscribe(ISubscribe subscriber, ushort dataId);
    }
}
