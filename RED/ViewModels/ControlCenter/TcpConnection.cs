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
                            case messageTypes.telemetry:
                                await receiveTelemetryData(bs);
                                break;
                            case messageTypes.error:
                                await receiveErrorData(bs);
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

        private async Task receiveTelemetryData(Stream s)
        {
            byte dataId = (byte)(s.ReadByte()); //TODO: handle disconnection here
            int dataLength = _controlCenter.MetadataManager.GetDataTypeByteLength(dataId);
            byte[] buffer = new byte[dataLength];
            await s.ReadAsync(buffer, 0, dataLength);
            _controlCenter.DataRouter.Send(dataId, buffer);
        }
        private async Task receiveErrorData(Stream s)
        {
            byte dataId = (byte)(s.ReadByte()); //TODO: handle disconnection here
            int dataLength = _controlCenter.MetadataManager.GetDataTypeByteLength(dataId);
            byte[] buffer = new byte[dataLength];
            await s.ReadAsync(buffer, 0, dataLength);
            _controlCenter.Console.WriteToConsole(_controlCenter.MetadataManager.GetError(dataId).Description + ": " + buffer.ToString()); //TODO: print this based on datatype
        }

        //ISubscribe.Receive
        public void Receive(byte dataId, byte[] data)
        {
            //This forwards the data across the connection

            //Validate Length
            if (data.Length != _controlCenter.MetadataManager.GetDataTypeByteLength(dataId))
            {
                _controlCenter.Console.WriteToConsole("Sending of a command with data id " + dataId.ToString() + " and invalid data length " + data.Length.ToString() + " was attempted.");
                return;
            }

            using (var bw = new BinaryWriter(netStream))
            {
                bw.Write((byte)(messageTypes.command));
                bw.Write(dataId);
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