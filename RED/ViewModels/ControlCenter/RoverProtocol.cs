using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class RoverProtocol : INetworkEncoding
    {
        public RoverProtocol()
        {
        }

        public byte[] EncodePacket(byte dataId, byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] DecodePacket(byte[] data, out byte dataId)
        {
            throw new NotImplementedException();
        }
    }
}