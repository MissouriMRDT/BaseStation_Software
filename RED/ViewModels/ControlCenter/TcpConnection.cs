namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using RED.Contexts;
    using RED.JSON;
    using RED.JSON.Contexts;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class TcpConnection : PropertyChangedBase, IConnection
    {
        private readonly TcpConnectionModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        private AsyncTcpServerViewModel _sourceServer;
        private IProtocol protocol;
        private bool connected;

        public Stream DataStream { get; private set; }
        public TcpClient Client
        {
            get
            {
                return _model._client;
            }
            private set
            {
                _model._client = value;
                NotifyOfPropertyChange(() => Client);
            }
        }
        public IPAddress RemoteIp
        {
            get
            {
                return _model._ipAddress;
            }
        }

        public TcpConnection(AsyncTcpServerViewModel server, TcpClient client, ControlCenterViewModel controlCenter)
        {
            _model = new TcpConnectionModel();
            _controlCenter = controlCenter;

            _sourceServer = server;
            protocol = new RoverConnection(_controlCenter); //In the future, detect which protocol we're using for various types of connections and create the coresponding object

            Client = client;
            DataStream = Client.GetStream();
        }

        public async Task Connect()
        {
            _model._ipAddress = ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            _controlCenter.Console.WriteToConsole("Connected to " + RemoteIp.ToString());
            connected = true;

            await protocol.Connect(this);
        }

        public void Close()
        {
            if (!connected) return;
            connected = false;
            _controlCenter.Console.WriteToConsole("Disconnected from " + RemoteIp.ToString());
            Client.Close();
            _sourceServer.CloseConnection(this);
        }
    }
}