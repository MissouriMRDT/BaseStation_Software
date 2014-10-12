using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Caliburn.Micro;
using RED.Models.ControlCenter;

namespace RED
{
    public class TCPAsyncServerVM : PropertyChangedBase
    {
        private TCPAsyncServerModel Model;

        public bool IsListening
        {
            get
            {
                return Model.IsListening;
            }
            set
            {
                Model.IsListening = value;
                NotifyOfPropertyChange(() => IsListening);
            }
        }
        public bool IsConnected
        {
            get
            {
                return Connections.Count > 0;
            }
        }
        public short ListeningPort
        {
            get
            {
                return Model.ListeningPort;
            }
            set
            {
                Model.ListeningPort = value;
                NotifyOfPropertyChange(() => ListeningPort);
            }
        }

        private TcpListener server;
        private List<TCPConnectionVM> Connections = new List<TCPConnectionVM>();

        public TCPAsyncServerVM(short portNum)
        {
            Model = new TCPAsyncServerModel();
            ListeningPort = portNum;
        }

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, ListeningPort);
            IsListening = true;
            Listen();
        }

        public void Stop()
        {
            server.Stop();
            IsListening = false;
            foreach (TCPConnectionVM client in Connections)
                client.Close();
        }

        private async void Listen()
        {
            server.Start();
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Connections.Add(new TCPConnectionVM(client));
            }
        }
    }
}