using Caliburn.Micro;
using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RED.ViewModels.Settings
{
    public class NetworkSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private AsyncTcpServerViewModel _server;

        public short ListeningPort
        {
            get
            {
                return _server.ListeningPort;
            }
            set
            {
                _server.ListeningPort = value;
                _settings.CurrentSettings.ServerListeningPort = value;
                NotifyOfPropertyChange(() => ListeningPort);
            }
        }

        public NetworkSettingsViewModel(SettingsManagerViewModel settings, AsyncTcpServerViewModel server)
        {
            _settings = settings;
            _server = server;

            _server.ListeningPort = _settings.CurrentSettings.ServerListeningPort;
        }
    }
}
