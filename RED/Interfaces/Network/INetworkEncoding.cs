namespace RED.Interfaces.Network
{
    public interface INetworkEncoding
    {
        byte[] EncodePacket(ushort dataId, byte[] data, ushort seqNum, bool requireACK);
        byte[] DecodePacket(byte[] data, out ushort dataId, out ushort seqNum, out bool requiresACK);
    }
}