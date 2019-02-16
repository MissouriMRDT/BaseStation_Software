using RED.Interfaces;
using RED.Models.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RED.RoveProtocol
{
    class RovecommOne
    {
        [Flags]
        private enum RoveCommFlags : byte
        {
            None = 0b000_0000,
            ACK = 0b000_0001
        }

        public const byte VersionNumber = 1;

        static public Packet DecodePacket(byte[] encodedPacket, IDataIdResolver resolver)
        {
            ushort rawSequenceNumber;
            RoveCommFlags rawFlags;
            ushort rawDataId;
            byte[] rawData;

            using (var ms = new MemoryStream(encodedPacket))
            using (var br = new BinaryReader(ms))
            {
                byte versionNumber = br.ReadByte();
                if (versionNumber != VersionNumber)
                    throw new InvalidDataException("Version number of packet is not supported.");

                rawSequenceNumber = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                rawFlags = (RoveCommFlags)br.ReadByte();
                rawDataId = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                ushort dataLength = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                rawData = br.ReadBytes(dataLength);
            }
            
            return new Packet(resolver.GetName(rawDataId), rawData, 1, null);
        }

        static public byte[] EncodePacket(Packet packet, IDataIdResolver resolver)
        {
            try
            {
                var flags = RoveCommFlags.None;
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(VersionNumber);
                    bw.Write(IPAddress.HostToNetworkOrder((byte)0)); // Sequence number
                    bw.Write((byte)flags);
                    bw.Write(IPAddress.HostToNetworkOrder((short)resolver.GetId(packet.Name)));
                    bw.Write(IPAddress.HostToNetworkOrder((short)packet.Data.Length));
                    bw.Write(packet.Data);
                    return ms.ToArray();
                }
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("Data buffer too long.", "data", e);
            }
        }
    }
}
