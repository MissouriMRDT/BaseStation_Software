namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Models.ControlCenter;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public class AsyncTcpServer : PropertyChangedBase
    {
        private readonly AsyncTcpServerModel _model;
        private readonly ControlCenterViewModel _controlCenterVm;
        private readonly List<TcpConnection> _connections;
        private TcpListener _server;

        public string LocalMachineName
        {
            get
            {
                return _model.LocalMachineName;
            }
            private set
            {
                _model.LocalMachineName = value;
                NotifyOfPropertyChange(() => LocalMachineName);
            }
        }
        public string LocalSoftwareName
        {
            get
            {
                return _model.LocalSoftwareName;
            }
            private set
            {
                _model.LocalSoftwareName = value;
                NotifyOfPropertyChange(() => LocalSoftwareName);
            }
        }

        public bool IsListening
        {
            get
            {
                return _model.IsListening;
            }
            private set
            {
                _model.IsListening = value;
                NotifyOfPropertyChange(() => IsListening);
            }
        }
        public bool IsConnected
        {
            get
            {
                return _connections.Count > 0;
            }
        }
        public short ListeningPort
        {
            get
            {
                return _model.ListeningPort;
            }
            private set
            {
                _model.ListeningPort = value;
                NotifyOfPropertyChange(() => ListeningPort);
            }
        }

        public AsyncTcpServer(short port, ControlCenterViewModel controlCenter)
        {
            _connections = new List<TcpConnection>();
            _model = new AsyncTcpServerModel();
            _controlCenterVm = controlCenter;

            LocalMachineName = "Red Master";
            LocalSoftwareName = "RED";

            ListeningPort = port;
        }

        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, ListeningPort);
            IsListening = true;
            _controlCenterVm.Console.WriteToConsole("Server Started");
            Listen();
        }

        public void Stop()
        {
            _server.Stop();
            IsListening = false;
            foreach (TcpConnection client in _connections)
                client.Close();
            _controlCenterVm.Console.WriteToConsole("Server Stopped");
        }

        private async void Listen()
        {
            _server.Start();
            try
            {
                while (IsListening)
                {
                    TcpClient client = await _server.AcceptTcpClientAsync();
                    _connections.Add(new TcpConnection(client, _controlCenterVm));
                }
            }
            catch (ObjectDisposedException)
            {
                //disregard - server stopped listening
            }
        }
    }
}