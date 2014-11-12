namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class TcpConnection : PropertyChangedBase, ISubscribe
    {
        private readonly TcpConnectionModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        private NetworkStream Stream { get; set; }
        public TcpClient Client
        {
            get
            {
                return _model._client;
            }
            set
            {
                _model._client = value;
                NotifyOfPropertyChange(() => Client);
            }
        }
        public IPAddress RemoteIp
        {
            get
            {
                return ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            }
        }
        public string RemoteName
        {
            get
            {
                return _model._remoteName;
            }
            set
            {
                _model._remoteName = value;
                NotifyOfPropertyChange(() => RemoteName);
            }
        }
        public string RemoteSoftware
        {
            get
            {
                return _model._remoteSoftware;
            }
            set
            {
                _model._remoteSoftware = value;
                NotifyOfPropertyChange(() => RemoteSoftware);
            }
        }

        public TcpConnection(TcpClient client, ControlCenterViewModel controlCenter)
        {
            _model = new TcpConnectionModel();
            _controlCenter = controlCenter;

            Client = client;
            Stream = Client.GetStream();

            InitializeConnection();

            //Start Listening
            ReceiveNetworkData();
        }

        private async void InitializeConnection()
        {
            var ascii = Encoding.ASCII;

            //Send Local Name
            var buffer = ascii.GetBytes(_controlCenter.TcpAsyncServer.LocalMachineName);
            await Stream.WriteAsync(buffer, 0, buffer.Length);

            //Get and Save Remote Name
            buffer = new byte[256];
            var remoteNameLength = await Stream.ReadAsync(buffer, 0, buffer.Length);
            RemoteName = ascii.GetString(buffer, 0, remoteNameLength);

            //Send Local Software
            buffer = ascii.GetBytes(_controlCenter.TcpAsyncServer.LocalSoftwareName);
            await Stream.WriteAsync(buffer, 0, buffer.Length);

            //Get and Save Remote Software
            buffer = new byte[256];
            int remoteSoftwareLength = await Stream.ReadAsync(buffer, 0, buffer.Length);
            RemoteSoftware = ascii.GetString(buffer, 0, remoteSoftwareLength);
        }

        private async void ReceiveNetworkData()
        {
            var buffer = new byte[1024];
            while (true)//TODO: have this stop if we close
            {
                await Stream.ReadAsync(buffer, 0, buffer.Length);
                using (var ms = new MemoryStream(buffer))
                {
                    using (var br = new BinaryReader(ms))
                    {
                        var dataId = br.ReadInt32();
                        var dataLength = br.ReadInt16();
                        var data = br.ReadBytes(dataLength);

                        switch (dataId)
                        {
                            case 1: _controlCenter.DataRouter.Subscribe(this, dataId); break;//Subscribe Request
                            case 2: _controlCenter.DataRouter.UnSubscribe(this, dataId); break;//Unsubscribe Request
                            default: _controlCenter.DataRouter.Send(dataId, data); break;//Normal Packet
                        }
                    }
                }
            }
        }

        public void Close()
        {
            Client.Close();
        }

        //ISubscribe.Receive
        public void Receive(int dataId, byte[] data)
        {
            using (var bw = new BinaryWriter(Stream))
            {
                bw.Write(dataId);
                bw.Write((Int16)(data.Length));
                bw.Write(data);
            }
        }
    }
}
