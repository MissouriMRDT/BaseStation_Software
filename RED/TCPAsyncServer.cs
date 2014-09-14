using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RED
{
    public class TCPAsyncServer
    {
        public bool IsListening { get; private set; }
        public bool IsConnected { get { return Connections.Count > 0; } }
        public short ListeningPort { get; private set; }

        private TcpListener server;
        private List<TcpClient> Connections = new List<TcpClient>();

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
            foreach (TcpClient client in Connections)
                client.Close();
        }

        private async void Listen()
        {
            server.Start();
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                throw new System.NotImplementedException();
                //TODO: Create the iSubscribe-implementing-TCPClient here
                Connections.Add(client);
            }
        }
    }
}