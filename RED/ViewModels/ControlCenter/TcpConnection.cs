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

        private IProtocol protocol;

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
                return ((IPEndPoint)(Client.Client.RemoteEndPoint)).Address;
            }
        }

        public TcpConnection(TcpClient client, ControlCenterViewModel controlCenter)
        {
            _model = new TcpConnectionModel();
            _controlCenter = controlCenter;

            protocol = new RoverConnection(_controlCenter); //In the future, detect which protocol we're using for various types of connections and create the coresponding object

            Client = client;
            DataStream = Client.GetStream();

            protocol.Connect(this);
        }

        public void Close()
        {
            Client.Close();
        }
    }
}