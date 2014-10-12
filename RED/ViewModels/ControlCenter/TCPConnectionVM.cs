using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using RED.Interfaces;
using System.IO;
using Caliburn.Micro;

namespace RED
{
    public class TCPConnectionVM : PropertyChangedBase, ISubscribe
    {
        public const string LocalMachineName = "RED Master";
        public const string LocalSoftwareName = "RED";

        public TcpClient Client { get; set; }
        private NetworkStream Stream { get; set; }
        public IPAddress RemoteIP
        {
            get
            {
                return ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            }
        }
        public string RemoteName { get; set; }
        public string RemoteSoftware { get; set; }
        private DataRouterVM router { get; set; }

        public TCPConnectionVM(TcpClient client)
        {
            this.Client = client;
            Stream = Client.GetStream();

            InitializeConnection();

            //Start Listening
            ReceiveNetworkData();
        }

        private async void InitializeConnection()
        {
            Encoding ascii = Encoding.ASCII;
            byte[] buffer;

            //Send Local Name
            buffer = ascii.GetBytes(LocalMachineName);
            await Stream.WriteAsync(buffer, 0, buffer.Length);

            //Get and Save Remote Name
            buffer = new byte[256];
            int remoteNameLength = await Stream.ReadAsync(buffer, 0, buffer.Length);
            RemoteName = ascii.GetString(buffer, 0, remoteNameLength);

            //Send Local Software
            buffer = ascii.GetBytes(LocalSoftwareName);
            await Stream.WriteAsync(buffer, 0, buffer.Length);

            //Get and Save Remote Software
            buffer = new byte[256];
            int remoteSoftwareLength = await Stream.ReadAsync(buffer, 0, buffer.Length);
            RemoteSoftware = ascii.GetString(buffer, 0, remoteSoftwareLength);
        }

        private async void ReceiveNetworkData()
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
