namespace RED.ViewModels.ControlCenter
{
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using RED.Contexts;
    using RED.JSON;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

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

        public TcpConnection(TcpClient client, ControlCenterViewModel controlCenter)
        {
            _model = new TcpConnectionModel();
            _controlCenter = controlCenter;

            Client = client;
            netStream = Client.GetStream();

            Connect();
        }

        private async void Connect()
        {
            if (await InitializeConnection())
            {
                //Start Listening
                ReceiveNetworkData();
            }
            else
            {
                Close();
            }
        }
        private async Task<bool> InitializeConnection()
        {
            using (var bs = new BufferedStream(netStream))
            {
                using (var br = new BinaryReader(bs))
                {
                    try
                    {
                        if ((synchronizeStatuses)(br.ReadByte()) == synchronizeStatuses.init)
                        {
                            //Synchronization Process
                            messageTypes messageType;
                            do
                            {
                                messageType = (messageTypes)(br.ReadByte());

                                switch (messageType)
                                {
                                    case messageTypes.console:
                                        receiveConsoleMessage(bs);
                                        break;
                                    case messageTypes.commandMetadata:
                                        await receiveCommandMetadata(bs);
                                        break;
                                    case messageTypes.telemetryMetadata:
                                        await receiveTelemetryMetadata(bs);
                                        break;
                                    case messageTypes.errorMetadata:
                                        await receiveErrorMetadata(bs);
                                        break;
                                    case messageTypes.synchronizeStatus:
                                        break;
                                    default:
                                        throw new ArgumentException("Illegal MessageType Byte received");
                                }
                            }
                            while (messageType != messageTypes.synchronizeStatus);

                            var status = (synchronizeStatuses)(br.ReadByte());

                            if (status == synchronizeStatuses.wait)
                                bs.Write(new byte[] { (byte)(synchronizeStatuses.ack) }, 0, 1);
                            else if (status == synchronizeStatuses.fail)
                                return false;

                            return true;
                        }
                        else
                        {
                            _controlCenter.Console.WriteToConsole("Init Synchronization byte not received.");
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        _controlCenter.Console.WriteToConsole("Exception caught during synchronization. Will request a repeat. '" + e.ToString() + "'");
                    }
                    //Still haven't succeeded so, requesting a repeat
                    bs.Write(new byte[] { (byte)(synchronizeStatuses.repeat) }, 0, 1);
                    return await InitializeConnection();
                }
            }
        }
        public void Close()
        {
            Client.Close();
        }

        private async void ReceiveNetworkData()
        {
            using (var bs = new BufferedStream(netStream))
            {
                using (var br = new BinaryReader(bs))
                {
                    while (true) //TODO: have this stop if we close
                    {
                        messageTypes messageType = (messageTypes)(br.ReadByte());

                        switch (messageType)
                        {
                            case messageTypes.console:
                                receiveConsoleMessage(bs);
                                break;
                            case messageTypes.command:
                                receiveCommandData(bs);
                                break;
                            case messageTypes.telemetry:
                                receiveTelemetryData(bs);
                                break;
                            case messageTypes.error:
                                receiveErrorData(bs);
                                break;
                            default:
                                throw new ArgumentException("Illegal MessageType Byte received");
                        }
                    }
                }
            }
        }

        private void receiveConsoleMessage(Stream s)
        {
            string message = readNullTerminated(s);
            _controlCenter.Console.WriteToConsole(message);
        }

        private void receiveSynchronizeStatus(Stream s)
        {
            //read status code
            //change state
        }

        private async Task receiveCommandMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            CommandMetadataContext context = await JSONDeserializer.Deserialize<CommandMetadataContext>(json);
            _controlCenter.MetadataManager.Add(context);
        }
        private async Task receiveTelemetryMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            TelemetryMetadataContext context = await JSONDeserializer.Deserialize<TelemetryMetadataContext>(json);
            _controlCenter.MetadataManager.Add(context);
        }
        private async Task receiveErrorMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            ErrorMetadataContext context = await JSONDeserializer.Deserialize<ErrorMetadataContext>(json);
            _controlCenter.MetadataManager.Add(context);
        }

        private async Task receiveCommandData(Stream s)
        {
            //look up the length to receive
            //download data
            //forward to router
        }
        private async Task receiveTelemetryData(Stream s)
        {
            //look up the length to receive
            //download data
            //forward to router
        }
        private async Task receiveErrorData(Stream s)
        {
            //look up the length to receive
            //download data
            //forward to router
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