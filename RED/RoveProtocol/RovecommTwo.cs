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
    public enum DataTypes : byte { INT8_T = 0, UINT8_T = 1, INT16_T = 2, UINT16_T = 3, INT32_T = 4, UINT32_T = 5 };

    public class RovecommTwo
    {

        public const byte VersionNumber = 2;
        static readonly private int[] sizes = new int[] { 1, 1, 2, 2, 4, 4};

        static public Packet DecodePacket(byte[] encodedPacket, IDataIdResolver resolver)
        {
            ushort rawDataId;
            byte[] rawData;
            byte dataSize;
            byte dataType;

            using (var ms = new MemoryStream(encodedPacket))
            using (var br = new BinaryReader(ms))
            {
                byte versionNumber = br.ReadByte();
                if (versionNumber != VersionNumber)
                    throw new InvalidDataException("Version number of packet is not supported.");

                rawDataId = (ushort)IPAddress.NetworkToHostOrder((short)br.ReadUInt16());
                dataSize = br.ReadByte();
                dataType = br.ReadByte();

                // Per data type
                rawData = br.ReadBytes(dataSize * sizes[dataType]);
            }

            return new Packet(resolver.GetName(rawDataId), rawData, dataSize, dataType);
        }

        static public byte[] EncodePacket(Packet packet, IDataIdResolver resolver)
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(VersionNumber);
                    bw.Write(IPAddress.HostToNetworkOrder((short)resolver.GetId(packet.Name)));
                    bw.Write(packet.Count);
                    bw.Write(packet.DataType);
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
