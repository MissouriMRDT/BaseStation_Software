using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Settings
{
    public class NetworkSettingsViewModel
    {
        private SettingsManagerViewModel _settings;
        private AsyncTcpServerViewModel _server;

        public NetworkSettingsViewModel(SettingsManagerViewModel settings, AsyncTcpServerViewModel server)
        {
            _settings = settings;
            _server = server;
        }
    }
}
