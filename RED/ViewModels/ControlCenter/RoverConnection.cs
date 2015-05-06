using RED.Contexts;
using RED.Interfaces;
using RED.JSON;
using RED.JSON.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class RoverConnection : IProtocol, ISubscribe
    {
        private const string noSyncMetadataFileURL = "NoSyncMetadata.xml";
        private readonly ControlCenterViewModel _controlCenter;

        private IConnection _sourceConnection;

        public bool ExpectSync { get; set; }

        public RoverConnection(ControlCenterViewModel controlCenter)
        {
            _controlCenter = controlCenter;
            ExpectSync = false;
        }

        public async Task Connect(IConnection source)
        {
            try
            {
                _sourceConnection = source;
                if (await InitializeConnection())
                {
                    //Start Listening
                    await Task.Run(() => ReceiveNetworkData());
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                _controlCenter.Console.WriteToConsole("Unexpected error in RoverConnection Receive:" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
            }
        }
        public void Disconnect()
        {
            _controlCenter.DataRouter.UnSubscribe(this);
            _sourceConnection.Close();
        }
        private async Task<bool> InitializeConnection()
        {
            if (!ExpectSync)
            {
                if (File.Exists(noSyncMetadataFileURL))
                {
                    _controlCenter.MetadataManager.AddFromFile(noSyncMetadataFileURL);
                    foreach (var command in _controlCenter.MetadataManager.Commands)
                        _controlCenter.DataRouter.Subscribe(this, command.Id);
                }
                return true;
            }
            using (var bs = new BufferedStream(_sourceConnection.DataStream))
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
                                bs.Write(new byte[] { (byte)(messageTypes.synchronizeStatus), (byte)(synchronizeStatuses.ack) }, 0, 2);
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
                    bs.Write(new byte[] { (byte)(messageTypes.synchronizeStatus), (byte)(synchronizeStatuses.repeat) }, 0, 2);
                    return await InitializeConnection();
                }
            }
        }

        private async Task ReceiveNetworkData()
        {
            try
            {
                using (var bs = new BufferedStream(_sourceConnection.DataStream))
                {
                    using (var br = new BinaryReader(bs))
                    {
                        while (true) //TODO: have this stop if we close
                        {
                            messageTypes messageType = (messageTypes)(br.ReadByte());//Here: is the reason this doesn't run asyncronously

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
            catch (EndOfStreamException)
            {
                _controlCenter.Console.WriteToConsole("EndOfStreamException in RoverConnection.Receive.");
            }
            catch (IOException)
            {
                _controlCenter.Console.WriteToConsole("IOException in RoverConnection.Receive.");
            }
            Disconnect();
        }

        private void receiveConsoleMessage(Stream s)
        {
            string message = readNullTerminated(s);
            _controlCenter.Console.WriteToConsole(message);
        }

        private async Task receiveCommandMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            var context = new CommandMetadataContext(await JSONDeserializer.Deserialize<JsonCommandMetadataContext>(json));
            _controlCenter.MetadataManager.Add(context);
            _controlCenter.DataRouter.Subscribe(this, context.Id);
        }
        private async Task receiveTelemetryMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            var context = new TelemetryMetadataContext(await JSONDeserializer.Deserialize<JsonTelemetryMetadataContext>(json));
            _controlCenter.MetadataManager.Add(context);
        }
        private async Task receiveErrorMetadata(Stream s)
        {
            string json = readNullTerminated(s);
            var context = new ErrorMetadataContext(await JSONDeserializer.Deserialize<JsonErrorMetadataContext>(json));
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
        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            //This forwards the data across the connection
            try
            {

                //Validate Length
                if (data.Length != _controlCenter.MetadataManager.GetDataTypeByteLength(dataId))
                {
                    _controlCenter.Console.WriteToConsole("Sending of a command with data id " + dataId.ToString() + " and invalid data length " + data.Length.ToString() + " was attempted.");
                    return;
                }

                using (var bw = new BinaryWriter(_sourceConnection.DataStream, Encoding.ASCII, true))
                {
                    bw.Write((byte)(messageTypes.command));
                    bw.Write(dataId);
                    bw.Write(data);
                }
            }
            catch (ArgumentException e)
            {
                _controlCenter.Console.WriteToConsole("ArgumentException (Stream not Writable) in RoverConnection.Receive.");
            }
            catch (Exception e)
            {
                _controlCenter.Console.WriteToConsole("Unexpected error in RoverConnection Send:" + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
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
