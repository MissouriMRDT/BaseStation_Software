namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public class AsyncTcpServerViewModel : PropertyChangedBase
    {
        private readonly AsyncTcpServerModel _model;
        private readonly ControlCenterViewModel _controlCenter;
        private readonly List<TcpConnection> _connections;
        private TcpListener _server;

        public string LocalMachineName
        {
            get
            {
                return _model._localMachineName;
            }
            private set
            {
                _model._localMachineName = value;
                NotifyOfPropertyChange(() => LocalMachineName);
            }
        }
        public string LocalSoftwareName
        {
            get
            {
                return _model._localSoftwareName;
            }
            private set
            {
                _model._localSoftwareName = value;
                NotifyOfPropertyChange(() => LocalSoftwareName);
            }
        }

        public bool IsListening
        {
            get
            {
                return _model._isListening;
            }
            private set
            {
                _model._isListening = value;
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
                return _model._listeningPort;
            }
            set
            {
                _model._listeningPort = value;
                NotifyOfPropertyChange(() => ListeningPort);
            }
        }

        public AsyncTcpServerViewModel(short port, ControlCenterViewModel controlCenter)
        {
            _connections = new List<TcpConnection>();
            _model = new AsyncTcpServerModel();
            _controlCenter = controlCenter;

            LocalMachineName = "Red Master";
            LocalSoftwareName = "RED";

            ListeningPort = port;
        }

        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, ListeningPort);
            IsListening = true;
            _controlCenter.Console.WriteToConsole("Server Started");
            Listen();
        }

        public void Stop()
        {
            _server.Stop();
            IsListening = false;
            var connectionCopy = new List<TcpConnection>(_connections); //Use a copy because we may modify it while removing stuff and that breaks the foreach
            foreach (TcpConnection c in connectionCopy)
                c.Close();
            _controlCenter.Console.WriteToConsole("Server Stopped");
        }

        private async void Listen()
        {
            _server.Start();
            try
            {
                while (IsListening)
                {
                    var client = await _server.AcceptTcpClientAsync();
                    var conn = new TcpConnection(this, client, _controlCenter);
                    _connections.Add(conn);
                    conn.Connect();
                }
            }
            catch (ObjectDisposedException)
            {
                //disregard - server stopped listening
            }
        }

        public void CloseConnection(TcpConnection connection)
        {
            _connections.Remove(connection);
        }
    }
}