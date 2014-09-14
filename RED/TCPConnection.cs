using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using RED.Interfaces;
using System.IO;

namespace RED
{
    public class TCPConnection : ISubscribe
    {
        public TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        public IPAddress Ip
        {
            get
            {
                return ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            }
        }
        public string RemoteName { get; set; }
        public string RemoteSoftware { get; set; }
        public DataRouter router { get; set; }

        public TCPConnection(TcpClient client)
        {
            this.Client = client;
            Stream = Client.GetStream();

            getRemoteInfo();

            //Start Listening
            Listen();
        }

        private async void getRemoteInfo()
        {
            //Send Local Name
            Encoding ascii = Encoding.ASCII;
            byte[] localName = ascii.GetBytes("RED Master");
            await Stream.WriteAsync(localName, 0, localName.Length);

            //Get and Save Remote Name
            byte[] remoteNameData = new byte[256];
            int remoteNameLength = await Stream.ReadAsync(remoteNameData, 0, remoteNameData.Length);
            RemoteName = ascii.GetString(remoteNameData, 0, remoteNameLength);

            //Send Local Software
            byte[] localSoftware = ascii.GetBytes("RED ? Version");
            await Stream.WriteAsync(localSoftware, 0, localSoftware.Length);

            //Get and Save Remote Software
            byte[] remoteSoftwareData = new byte[256];
            int remoteSoftwareLength = await Stream.ReadAsync(remoteSoftwareData, 0, remoteSoftwareData.Length);
            RemoteSoftware = ascii.GetString(remoteSoftwareData, 0, remoteSoftwareLength);
        }

        private async void Listen()
        {
            byte[] buffer = new byte[1024];
            while (true)//TODO: have this stop if we close
            {
                await Stream.ReadAsync(buffer, 0, buffer.Length);
                using (BinaryReader br = new BinaryReader(new MemoryStream(buffer)))
                {
                    int dataId = br.ReadInt32();
                    Int16 dataLength = br.ReadInt16();
                    byte[] data = br.ReadBytes(dataLength);

                    switch (dataId)
                    {
                        case 1: router.Subscribe(this, dataId); break;//Subscribe Request
                        case 2: router.UnSubscribe(this, dataId); break;//Unsubscribe Request
                        default: router.Send(dataId, data); break;//Normal Packet
                    }
                }
            }
        }

        public void Close()
        {
            Client.Close();
        }

        public void Receive(int dataId, byte[] data)
        {
            using (BinaryWriter bw = new BinaryWriter(Stream))
            {
                bw.Write(dataId);
                bw.Write((Int16)(data.Length));
                bw.Write(data);
            }
        }
    }
}
