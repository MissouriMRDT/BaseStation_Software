using Caliburn.Micro;
using RED.Addons.Network;
using RED.Interfaces.Network;
using RED.Models.Tools;
using RED.ViewModels.Network;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Threading;

namespace RED.ViewModels.Tools
{
    public class TelemetryLogToolViewModel : PropertyChangedBase
    {
        private readonly TelemetryLogToolModel _model;
        private readonly IServerProvider _serverProvider;

        public ObservableCollection<ServerLog> TelemetryLog
        {
            get
            {
                return _model.TelemetryLog;
            }
            private set
            {
                _model.TelemetryLog = value;
                NotifyOfPropertyChange(() => TelemetryLog);
            }
        }

        public TelemetryLogToolViewModel(NetworkManagerViewModel networkVM, IServerProvider serverProvider)
        {
            _model = new TelemetryLogToolModel();
            _serverProvider = serverProvider;

            TelemetryLog = new ObservableCollection<ServerLog>(_serverProvider.GetServerList().Select(x => new ServerLog(x)));
            networkVM.PacketReceived += LogTelemetryRecieved;
        }

        private void LogTelemetryRecieved(IPAddress srcIP, byte[] telemetry)
        {
            var server = TelemetryLog.FirstOrDefault(x => x.Address.Equals(srcIP));
            if (server != null)
                server.Timestamp = DateTime.Now;
        }

        public class ServerLog : PropertyChangedBase
        {
            private readonly TelemetryLogToolModel.ServerLogModel _model;

            public string Name
            {
                get
                {
                    return _model.Name;
                }
                set
                {
                    _model.Name = value; NotifyOfPropertyChange(() => Name);
                }

            }
            public IPAddress Address
            {
                get
                {
                    return _model.Address;
                }
                set
                {
                    _model.Address = value; NotifyOfPropertyChange(() => Address);
                }

            }
            public DateTime Timestamp
            {
                get
                {
                    return _model.Timestamp;
                }
                set
                {
                    _model.Timestamp = value; NotifyOfPropertyChange(() => Timestamp);
                }

            }

            public ServerLog(Server srv)
            {
                _model = new TelemetryLogToolModel.ServerLogModel();
                Name = srv.Name;
                Address = srv.Address;
                Timestamp = DateTime.MinValue;
                timer.Tick += (s, e) => NotifyOfPropertyChange(() => Timestamp);
            }

            private static DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(250),
                IsEnabled = true
            };
        }
    }
}
