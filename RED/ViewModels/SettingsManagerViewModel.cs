using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels
{
    public class SettingsManagerViewModel : PropertyChangedBase
    {
        private SettingsManagerModel _model;
        private ControlCenterViewModel _controlCenter;

        public NetworkSettingsViewModel Network
        {
            get
            {
                return _model.network;
            }
            set
            {
                _model.network = value;
                NotifyOfPropertyChange(() => Network);
            }
        }

        public SettingsManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;

            Network = new NetworkSettingsViewModel(this, cc.TcpAsyncServer);
        }
    }
}
