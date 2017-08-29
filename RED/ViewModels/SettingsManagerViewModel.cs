using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.Settings.Input;
using RED.ViewModels.Settings.Input.Controllers;
using RED.ViewModels.Settings.Network;
using RED.ViewModels.Settings.Modules;

namespace RED.ViewModels
{
    public class SettingsManagerViewModel : PropertyChangedBase
    {
        private SettingsManagerModel _model;
        private IConfigurationManager _configManager;
        private ControlCenterViewModel _controlCenter;

        private const string SettingsConfigName = "GeneralSettings";

        public REDSettingsContext CurrentSettingsConfig
        {
            get;
            private set;
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
        public XboxControllerInputSettingsViewModel Xbox1
        {
            get
            {
                return _model.xbox1;
            }
            set
            {
                _model.xbox1 = value;
                NotifyOfPropertyChange(() => Xbox1);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox2
        {
            get
            {
                return _model.xbox2;
            }
            set
            {
                _model.xbox2 = value;
                NotifyOfPropertyChange(() => Xbox2);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox3
        {
            get
            {
                return _model.xbox3;
            }
            set
            {
                _model.xbox3 = value;
                NotifyOfPropertyChange(() => Xbox3);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox4
        {
            get
            {
                return _model.xbox4;
            }
            set
            {
                _model.xbox4 = value;
                NotifyOfPropertyChange(() => Xbox4);
            }
        }
        public GPSSettingsViewModel GPS
        {
            get
            {
                return _model.gps;
            }
            set
            {
                _model.gps = value;
                NotifyOfPropertyChange(() => GPS);
            }
        }
        public PowerSettingsViewModel Power
        {
            get
            {
                return _model.power;
            }
            set
            {
                _model.power = value;
                NotifyOfPropertyChange(() => Power);
            }
        }
        public NetworkManagerSettingsViewModel Network
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

        public SettingsManagerViewModel(IConfigurationManager configManager, ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;
            _configManager = configManager;

            _configManager.AddRecord(SettingsConfigName, GetDefaultConfig());
            CurrentSettingsConfig = _configManager.GetConfig<REDSettingsContext>(SettingsConfigName);

            Drive = new DriveSettingsViewModel(CurrentSettingsConfig.Drive, cc.Drive);
            Science = new ScienceSettingsViewModel(CurrentSettingsConfig.Science, cc.Science);
            Xbox1 = new XboxControllerInputSettingsViewModel(CurrentSettingsConfig.Xbox1, cc.XboxController1, 1);
            Xbox2 = new XboxControllerInputSettingsViewModel(CurrentSettingsConfig.Xbox2, cc.XboxController2, 2);
            Xbox3 = new XboxControllerInputSettingsViewModel(CurrentSettingsConfig.Xbox3, cc.XboxController3, 3);
            Xbox4 = new XboxControllerInputSettingsViewModel(CurrentSettingsConfig.Xbox4, cc.XboxController4, 4);
            GPS = new GPSSettingsViewModel(CurrentSettingsConfig.GPS, cc.GPS, cc.Map);
            Power = new PowerSettingsViewModel(CurrentSettingsConfig.Power, cc.Power);
            Network = new NetworkManagerSettingsViewModel(CurrentSettingsConfig.Network, cc.NetworkManager);
        }

        public void SaveSettings()
        {
            _configManager.SetConfig(SettingsConfigName, CurrentSettingsConfig);
        }

        static REDSettingsContext GetDefaultConfig()
        {
            return new REDSettingsContext()
            {
                Drive = DriveSettingsViewModel.DefaultConfig,
                Xbox1 = XboxControllerInputSettingsViewModel.DefaultConfig,
                Xbox2 = XboxControllerInputSettingsViewModel.DefaultConfig,
                Xbox3 = XboxControllerInputSettingsViewModel.DefaultConfig,
                Xbox4 = XboxControllerInputSettingsViewModel.DefaultConfig,
                GPS = GPSSettingsViewModel.DefaultConfig,
                Science = ScienceSettingsViewModel.DefaultConfig,
                Power = PowerSettingsViewModel.DefaultConfig,
                Network = NetworkManagerSettingsViewModel.DefaultConfig
            };
        }
    }
}
