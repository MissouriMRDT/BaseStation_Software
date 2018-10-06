using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RoverNetworkManager.Networking {
	public class Rovecomm {
		public const byte VersionNumber = 1;

		private INetworkTransportProtocol continuousDataSocket = new UDPEndpoint(11000, 11000);

		[Flags]
		private enum RoveCommFlags : byte {
			None = 0b000_0000,
			ACK = 0b000_0001
		}

		public void SendPacket(ushort dataId, byte[] data, IPAddress destIP, ushort seqNum) {
			byte[] packetData = EncodePacket(dataId, data, seqNum);
			SendPacketUnreliable(destIP, packetData);
		}

		private byte[] EncodePacket(ushort dataId, byte[] data, ushort seqNum) {
			try {
				var flags = RoveCommFlags.None;
				using (var ms = new MemoryStream())
				using (var bw = new BinaryWriter(ms)) {
					bw.Write(VersionNumber);
					bw.Write(IPAddress.HostToNetworkOrder((short)seqNum));
					bw.Write((byte)flags);
					bw.Write(IPAddress.HostToNetworkOrder((short)dataId));
					bw.Write(IPAddress.HostToNetworkOrder((short)data.Length));
					bw.Write(data);
					return ms.ToArray();
				}
			}
			catch (InvalidCastException e) {
				throw new ArgumentException("Data buffer too long.", "data", e);
			}
		}

		public async void SendPacketUnreliable(IPAddress destIP, byte[] packetData) {
			try {
				await continuousDataSocket.SendMessage(destIP, packetData);
			}
			catch (Exception ex) {
				System.Windows.MessageBox.Show(ex.ToString());
			}
		}
	}
}
