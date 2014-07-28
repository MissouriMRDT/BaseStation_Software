namespace RED.ViewModels.Modules
{
    using ControlCenter;
    using FirstFloor.ModernUI.Presentation;
    using Interfaces;
    using Models.Modules;
    using RED.Addons;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Windows.Input;

    public class NetworkingVM : BaseVM, IModule
    {
        private static readonly NetworkingModel Model = new NetworkingModel();
        private AsyncTcpServer Server;

        public string Title
        {
            get { return Model.Title; }
        }
        public bool IsManageable
        {
            get { return Model.IsManageable; }
        }
        public bool InUse
        {
            get { return Model.InUse; }
            set { Model.InUse = value; }
        }

        // Connection state properties
        public bool IsConnected
        {
            get { return Model.isConnected; }
            set
            {
                SetField(ref Model.isConnected, value);
                if (value)
                {
                    CanListen = false;
                    CanDisconnect = true;
                    CanSend = true;
                }
                else
                {
                    CanListen = true;
                    CanDisconnect = false;
                    CanSend = false;
                }
                ControlCenterVM.StateVM.NetworkHasConnection = value;
            }
        }
        public bool CanListen
        {
            get { return Model.CanListen; }
            set { SetField(ref Model.CanListen, value); }
        }
        public bool CanDisconnect
        {
            get { return Model.CanDisconnect; }
            set { SetField(ref Model.CanDisconnect, value); }
        }
        public bool CanSend
        {
            get { return Model.CanSend; }
            set { SetField(ref Model.CanSend, value); }
        }
        // Console-related properties
        public string ConsoleText
        {
            get { return Model.ConsoleText; }
            set { SetField(ref Model.ConsoleText, value); }
        }
        public int IdToSend
        {
            get
            {
                return Model.IdToSend;
            }
            set
            {
                if (value.GetTypeCode() != TypeCode.Int32) return;
                SetField(ref Model.IdToSend, value);
            }
        }
        public string ValueToSend
        {
            get
            {
                return Model.ValueToSend;
            }
            set
            {
                SetField(ref Model.ValueToSend, value);
            }
        }

        public ICommand ClearConsoleCommand { get; private set; }
        public ICommand NetworkListenCommand { get; private set; }
        public ICommand NetworkDisconnectCommand { get; private set; }
        public ICommand NetworkSendCommand { get; private set; }

        public NetworkingVM()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            Server = new AsyncTcpServer(localEndPoint, this);

            ClearConsoleCommand = new RelayCommand(c => ClearConsole());

            NetworkListenCommand = new RelayCommand(c => Server.Start(), b => CanListen);
            NetworkDisconnectCommand = new RelayCommand(c => Server.Stop(), b => CanDisconnect);
            NetworkSendCommand = new RelayCommand(c =>
                {
                    SendProtocol<object>(new { Id = IdToSend, Value = ValueToSend });
                    WriteToConsole("Sending: Id -> " + IdToSend + ", Value -> " + ValueToSend);
                },
                b => CanSend);
        }

        private void ClearConsole()
        {
            ConsoleText = String.Empty;
        }
        private void Listen()
        {
            Server.Start();
        }
        private void Disconnect()
        {
            Server.Stop();
        }
        public bool SendProtocol<T>(object obj)
        {
            // Public method to send a message adhering to the IProtocol interface
            if (!IsConnected || !CanSend) return false;

            string serializedMessage = string.Empty;
            try
            {
                throw new NotImplementedException("JSON compatibility removed. Sending is not currently supported.");
                // Remove any delimiters that would interfere with the transmission
            }
            catch (Exception e)
            {

            }

            try
            {
                // Send protocol as serialized string on the current socket
                Server.Write(serializedMessage);
            }
            catch (Exception e)
            {

            }

            return true;
        }

        internal void WriteToConsole(string text)
        {
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.ff", CultureInfo.InvariantCulture);
            var newText = String.Format("{0}: {1} {2}", timeStamp, text, Environment.NewLine);
            ConsoleText += newText;
        }

        internal void ParseAndDeliverTelemetry(string message)
        {
            throw new NotImplementedException("JSON Parsing has been removed. No new form of parsing has currently exists.");
        }

        public void TelemetryReceiver<T>(object message)
        {
            throw new NotImplementedException("Networking Module does not currently display telemetry data.");
        }
    }
}