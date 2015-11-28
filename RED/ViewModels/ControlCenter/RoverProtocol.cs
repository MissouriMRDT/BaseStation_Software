using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class RoverProtocol : INetworkEncoding
    {
        public const byte VersionNumber = 1;
        private const ushort TempSequenceNumber = 0;

        public RoverProtocol()
        {
        }

        public byte[] EncodePacket(byte dataId, byte[] data)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.Write(VersionNumber);
                        bw.Write(IPAddress.HostToNetworkOrder((short)TempSequenceNumber));
                        bw.Write(IPAddress.HostToNetworkOrder((short)(ushort)dataId));
                        bw.Write(IPAddress.HostToNetworkOrder((short)(ushort)data.Length));
                        bw.Write(data);
                    }
                    return ms.ToArray();
                }
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("Data buffer too long.", "data", e);
            }
        }

        public byte[] DecodePacket(byte[] data, out byte dataId)
        {
            byte versionNumber;
            ushort sequenceNumber;
            ushort rawDataId;
            byte[] rawData;

            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    versionNumber = br.ReadByte();
                    sequenceNumber = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    rawDataId = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    ushort dataLength = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    rawData = br.ReadBytes(dataLength);
                }
            }

            if (versionNumber != VersionNumber)
                throw new InvalidDataException("Version number of packet is not supported.");
            dataId = (byte)rawDataId;
            return rawData;
        }
    }

    [Serializable]
    public class SequenceNumberException : Exception
    {
        public readonly ushort OffendingSequenceNumber;
        public SequenceNumberException() { }
        public SequenceNumberException(string message) : base(message) { }
        public SequenceNumberException(string message, Exception inner) : base(message, inner) { }
        protected SequenceNumberException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
        public SequenceNumberException(ushort seqNum) : base() { }
    }
}