namespace RED.Addons
{
    using RED.RoverComs;
    using RED.ViewModels.Modules;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Timers;

    public class AsyncTcpServer
    {
        private TcpListener tcpListener;
        private List<Client> clients;
        private NetworkingVM networkingVM;
        private Timer timeOut;
        private DateTime lastReceivedMessage;
        
        /// <summary>
        /// Constructor for a new server using an end point
        /// </summary>
        /// <param name="localEP">The local end point for the server.</param>
        public AsyncTcpServer(IPEndPoint localEP, NetworkingVM netVM) : this()
        {
            tcpListener = new TcpListener(localEP);
            networkingVM = netVM;
        }

        /// <summary>
        /// Private constructor for the common constructor operations.
        /// </summary>
        private AsyncTcpServer()
        {
            this.Encoding = Encoding.ASCII;
            this.clients = new List<Client>();
        }

        private void CheckConnection(object sender, ElapsedEventArgs e)
        {
            if((DateTime.Now - lastReceivedMessage).Seconds >= 5)
            {
                networkingVM.WriteToConsole("Connection timed out.");
                Stop();
                Start();
            }
        }

        /// <summary>
        /// The encoding to use when sending / receiving strings.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// An enumerable collection of all the currently connected tcp clients
        /// </summary>
        public IEnumerable<TcpClient> TcpClients
        {
            get
            {
                foreach (Client client in this.clients)
                {
                    yield return client.TcpClient;
                }
            }
        }

        /// <summary>
        /// Starts the TCP Server listening for new clients.
        /// </summary>
        public void Start()
        {
            this.tcpListener.Start();
            this.timeOut = new Timer(5000);
            this.timeOut.Elapsed += CheckConnection;
            networkingVM.WriteToConsole("Listening...");
            this.tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
            networkingVM.CanListen = false;
            networkingVM.CanDisconnect = true;
        }

        /// <summary>
        /// Stops the TCP Server listening for new clients and disconnects
        /// any currently connected clients.
        /// </summary>
        public void Stop()
        {
            this.tcpListener.Stop();
            lock (this.clients)
            {
                foreach (Client client in this.clients)
                {
                    client.TcpClient.Client.Disconnect(false);
                }
                this.clients.Clear();
            }
            networkingVM.IsConnected = false;
            networkingVM.WriteToConsole("Disconnected.");
            this.timeOut.Dispose();
        }

        /// <summary>
        /// Writes a string to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="data">The string to send.</param>
        public void Write(TcpClient tcpClient, string data)
        {
            byte[] bytes = this.Encoding.GetBytes(data);
            Write(tcpClient, bytes);
        }

        /// <summary>
        /// Writes a string to all clients connected.
        /// </summary>
        /// <param name="data">The string to send.</param>
        public void Write(string data)
        {
            foreach (Client client in this.clients)
            {
                Write(client.TcpClient, data);
            }
        }

        /// <summary>
        /// Writes a byte array to all clients connected.
        /// </summary>
        /// <param name="bytes">The bytes to send.</param>
        public void Write(byte[] bytes)
        {
            foreach (Client client in this.clients)
            {
                Write(client.TcpClient, bytes);
            }
        }

        /// <summary>
        /// Writes a byte array to a given TCP Client
        /// </summary>
        /// <param name="tcpClient">The client to write to</param>
        /// <param name="bytes">The bytes to send</param>
        public void Write(TcpClient tcpClient, byte[] bytes)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.BeginWrite(bytes, 0, bytes.Length, WriteCallback, tcpClient);
        }

        /// <summary>
        /// Callback for the write opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void WriteCallback(IAsyncResult result)
        {
            TcpClient tcpClient = result.AsyncState as TcpClient;
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.EndWrite(result);
        }

        /// <summary>
        /// Callback for the accept tcp client opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void AcceptTcpClientCallback(IAsyncResult result)
        {
            try
            {
                TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
                networkingVM.IsConnected = true;
                if(!timeOut.Enabled)
                {
                    timeOut.Start();
                }
                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                Client client = new Client(tcpClient, buffer);
                lock (this.clients)
                {
                    this.clients.Add(client);
                }
                NetworkStream networkStream = client.NetworkStream;
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
                networkingVM.WriteToConsole("Connection established.");
                tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
            }
            catch(ObjectDisposedException e)
            {
                // The client's connection was closed.
            }
        }

        /// <summary>
        /// Callback for the read opertaion.
        /// </summary>
        /// <param name="result">The async result object</param>
        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                Client client = result.AsyncState as Client;
                if (client == null || client.TcpClient.Connected == false)
                {
                    return;
                }
                NetworkStream networkStream = client.NetworkStream;

                int read = networkStream.EndRead(result);

                if (read == 0)
                {
                    lock (this.clients)
                    {
                        this.clients.Remove(client);
                        return;
                    }
                }

                string data = this.Encoding.GetString(client.Buffer, 0, read);

                // End of message; parse, deliver, and reset buffer to right after delimeter.
                BuildMessage(client, data);

                lastReceivedMessage = DateTime.Now;
                networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
            }
            catch (IOException)
            {
                Console.WriteLine("Client Disconnected.");
            }
        }

        private void BuildMessage(Client client, string data)
        {
            if (data.Contains("\0"))
            {
                var delimeterIndex = data.IndexOf('\0');
                var split = data.Split('\0');

                if (delimeterIndex == 0) // Delimeter at beginning.
                {
                    if (data.Length == 1) // Delimeter is also the only thing in this read.
                    {
                        //networkingVM.WriteToConsole(client.StringBuilder.ToString());
                        networkingVM.ParseAndDeliverTelemetry(client.StringBuilder.ToString());
                        client.StringBuilder.Clear();
                    }
                    else
                    {
                        //networkingVM.WriteToConsole(client.StringBuilder.ToString());
                        networkingVM.ParseAndDeliverTelemetry(client.StringBuilder.ToString());
                        client.StringBuilder.Clear();
                        client.StringBuilder.Append(split[1]);
                    }
                }
                else if (delimeterIndex == data.Length - 1) // Delimeter at end and characters are in front of it.
                {
                    client.StringBuilder.Append(split[0]);
                    //networkingVM.WriteToConsole(client.StringBuilder.ToString());
                    networkingVM.ParseAndDeliverTelemetry(client.StringBuilder.ToString());
                    client.StringBuilder.Clear();
                }
                else // Delimeter is between two sets of characters.
                {
                    client.StringBuilder.Append(split[0]);
                   // networkingVM.WriteToConsole(client.StringBuilder.ToString());
                    networkingVM.ParseAndDeliverTelemetry(client.StringBuilder.ToString());
                    client.StringBuilder.Clear();
                    client.StringBuilder.Append(split[1]);
                }
            }
            else
            {
                client.StringBuilder.Append(data);
            }
        }

        #region Client Class
        /// <summary>
        /// Internal class to join the TCP client and buffer together 
        /// for easy management in the server
        /// </summary>
        internal class Client
        {
            /// <summary>
            /// Constructor for a new Client
            /// </summary>
            /// <param name="tcpClient">The TCP client</param>
            /// <param name="buffer">The byte array buffer</param>
            public Client(TcpClient tcpClient, byte[] buffer)
            {
                if (tcpClient == null) throw new ArgumentNullException("tcpClient");
                if (buffer == null) throw new ArgumentNullException("buffer");
                this.TcpClient = tcpClient;
                this.Buffer = buffer;
            }

            /// <summary>
            /// Gets the TCP Client
            /// </summary>
            public TcpClient TcpClient { get; private set; }

            /// <summary>
            /// Gets the Buffer.
            /// </summary>
            public byte[] Buffer { get; private set; }

            /// <summary>
            /// Gets the network stream
            /// </summary>
            public NetworkStream NetworkStream { get { return TcpClient.GetStream(); } }

            public StringBuilder StringBuilder = new StringBuilder();
        }
        #endregion
    }
}