using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Interfaces
{
    public interface INetworkEncoding
    {
        byte[] EncodePacket(byte dataId, byte[] data, ushort seqNum);
        byte[] DecodePacket(byte[] data, out byte dataId, out ushort seqNum);
    }
}