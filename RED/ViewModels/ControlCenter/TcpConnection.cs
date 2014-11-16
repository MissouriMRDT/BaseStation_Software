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

        private NetworkStream netStream { get; set; }
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
            netStream = Client.GetStream();

            InitializeConnection();

            //Start Listening
            ReceiveNetworkData();
        }

        private async void InitializeConnection()
        {

        }

        private async void ReceiveNetworkData()
        {
            while (true) //TODO: have this stop if we close
            {
                //await netStream.ReadAsync(buffer, 0, buffer.Length);
                using (var bs = new BufferedStream(netStream))
                {
                    using (var br = new BinaryReader(bs))
                    {
                        messageTypes messageType = (messageTypes)(br.ReadByte());

                        switch (messageType)
                        {
                            case messageTypes.console:
                                recieveConsoleMessage(bs);
                                break;
                            case messageTypes.synchronizeStatus:
                                recieveSynchronizeStatus(bs);
                                break;
                            case messageTypes.commandMetadata:
                                break;
                            case messageTypes.telemetryMetadata:
                                break;
                            case messageTypes.errorMetadata:
                                break;
                            case messageTypes.command:
                                break;
                            case messageTypes.telemetry:
                                break;
                            case messageTypes.error:
                                break;
                            default:
                                throw new ArgumentException("Illegal Message Type Byte Recieved");
                        }
                    }
                }
            }
        }

        private void recieveConsoleMessage(Stream s)
        {
            string message = readNullTerminated(s);
            _controlCenter.Console.WriteToConsole(message);
        }

        private void recieveSynchronizeStatus(Stream s)
        {
            //read status code
            //change state
        }

        private void recieveMetadata<T>(Stream s)
        {
            string data = readNullTerminated(s);
            //deserialize to type T
            //add to MetadataManager
        }

        private void recieveData<T>(Stream s)
        {
            //look up the length to recieve
            //download data
            //forward to router
        }

        public void Close()
        {
            Client.Close();
        }

        private string readNullTerminated(Stream s)
        {
            StringBuilder sb = new StringBuilder();
            char lastchar;
            do
            {
                lastchar = (char)(s.ReadByte());
                sb.Append(lastchar);
            }
            while (lastchar != '\0');

            return sb.ToString();
        }

        //ISubscribe.Receive
        public void Receive(int dataId, byte[] data)
        {
            throw new NotImplementedException();
            using (var bw = new BinaryWriter(netStream))
            {
                bw.Write(dataId);
                bw.Write((Int16)(data.Length));
                bw.Write(data);
            }
        }
        private enum messageTypes : byte
        {
            console = 0x00,
            synchronizeStatus = 0x01,
            commandMetadata = 0x02,
            telemetryMetadata = 0x03,
            errorMetadata = 0x04,
            command = 0x05,
            telemetry = 0x06,
            error = 0x07
        }

        private enum synchronizeStatuses : byte
        {
            init = 0x00,
            wait = 0x01,
            ack = 0x02,
            repeat = 0x03,
            fail = 0x04
        }
    }
}