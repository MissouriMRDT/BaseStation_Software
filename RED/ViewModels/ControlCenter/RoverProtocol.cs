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

        public RoverProtocol()
        { }

        public byte[] EncodePacket(ushort dataId, byte[] data, ushort seqNum, bool requireACK)
        {
            try
            {
                var flags = (requireACK) ? RoveCommFlags.ACK : RoveCommFlags.None;
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.Write(VersionNumber);
                        bw.Write(IPAddress.HostToNetworkOrder((short)seqNum));
                        bw.Write((byte)flags);
                        bw.Write(IPAddress.HostToNetworkOrder((short)dataId));
                        bw.Write(IPAddress.HostToNetworkOrder((short)data.Length));
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

        public byte[] DecodePacket(byte[] data, out ushort dataId, out ushort seqNum, out bool requiresACK)
        {
            byte versionNumber;
            ushort rawSequenceNumber;
            RoveCommFlags rawFlags;
            ushort rawDataId;
            byte[] rawData;

            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    versionNumber = br.ReadByte();
                    if (versionNumber != VersionNumber)
                        throw new InvalidDataException("Version number of packet is not supported.");

                    rawSequenceNumber = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    rawFlags = (RoveCommFlags)br.ReadByte();
                    rawDataId = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    ushort dataLength = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                    rawData = br.ReadBytes(dataLength);
                }
            }

            dataId = rawDataId;
            seqNum = rawSequenceNumber;
            requiresACK = (rawFlags & RoveCommFlags.ACK) != RoveCommFlags.None;
            return rawData;
        }

        [Flags]
        private enum RoveCommFlags : byte
        {
            None = 0,
            ACK = 1
        }
    }
}