using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface INetworkEncoding
    {
        byte[] EncodePacket(ushort dataId, byte[] data, ushort seqNum, bool requireACK);
        byte[] DecodePacket(byte[] data, out ushort dataId, out ushort seqNum, out bool requiresACK);
    }
}