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

        public Properties.Settings CurrentSettings
        {
            get
            {
                return Properties.Settings.Default;
            }
        }

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
        public DriveSettingsViewModel Drive
        {
            get
            {
                return _model.drive;
            }
            set
            {
                _model.drive = value;
                NotifyOfPropertyChange(() => Drive);
            }
        }
        public ScienceSettingsViewModel Science
        {
            get
            {
                return _model.science;
            }
            set
            {
                _model.science = value;
                NotifyOfPropertyChange(() => Science);
            }
        }

        public SettingsManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;

            Network = new NetworkSettingsViewModel(this, cc.TcpAsyncServer);
            Drive = new DriveSettingsViewModel(this, (ViewModels.ControlCenter.DriveControllerModeViewModel)cc.Input.ControllerModes[0]);
            Science = new ScienceSettingsViewModel(this, cc.Science);
        }

        public void SaveSettings()
        {
            CurrentSettings.Save();
        }
    }
}