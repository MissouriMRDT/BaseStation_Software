using Caliburn.Micro;
using RED.Addons;
using RED.Interfaces.Network;
using RED.ViewModels.Network;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Threading;

namespace RED.ViewModels
{
    public class TelemetryLogToolViewModel : PropertyChangedBase
    {
        private IServerProvider _serverProvider;

        public ObservableCollection<ServerLog> TelemetryLog { get; set; }

        public TelemetryLogToolViewModel(NetworkManagerViewModel networkVM, IServerProvider serverProvider)
        {
            _serverProvider = serverProvider;

            TelemetryLog = new ObservableCollection<ServerLog>(_serverProvider.GetServerList().Select(x => new ServerLog(x)));
            networkVM.TelemetryRecieved += LogTelemetryRecieved;
        }

        private void LogTelemetryRecieved(IPAddress srcIP)
        {
            var server = TelemetryLog.FirstOrDefault(x => x.Address.Equals(srcIP));
            if (server != null)
                server.Timestamp = DateTime.Now;
        }

        public class ServerLog : PropertyChangedBase
        {
            private string _name;
            private IPAddress _address;
            private DateTime _timestamp;

            public string Name { get { return _name; } set { _name = value; NotifyOfPropertyChange(() => Name); } }
            public IPAddress Address { get { return _address; } set { _address = value; NotifyOfPropertyChange(() => Address); } }
            public DateTime Timestamp { get { return _timestamp; } set { _timestamp = value; NotifyOfPropertyChange(() => Timestamp); } }

            public ServerLog(Server srv)
            {
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
