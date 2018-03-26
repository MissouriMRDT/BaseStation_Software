using Caliburn.Micro;
using RED.Configurations.Input.Controllers;
using RED.Configurations.Modules;
using RED.Configurations.Network;
using RED.Contexts;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.Settings.Input.Controllers;
using RED.ViewModels.Settings.Modules;
using RED.ViewModels.Settings.Network;

namespace RED.ViewModels
{
    /// <summary>
    /// The overseer class for RED's setting tab, containing logic for operating the tab in the GUI as well 
    /// as all the individual setting sections contained within it. 
    /// 
    /// This class is split into a variety of sub classes, each in turn populating the overall settings tab 
    /// with a specific section, such as controller settings classes.
    /// </summary>
    public class SettingsManagerViewModel : PropertyChangedBase
    {
        private readonly SettingsManagerModel _model;
        private readonly IConfigurationManager _configManager;
        private readonly ControlCenterViewModel _controlCenter;

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
                return _model.Drive;
            }
            set
            {
                _model.Drive = value;
                NotifyOfPropertyChange(() => Drive);
            }
        }
        public ScienceSettingsViewModel Science
        {
            get
            {
                return _model.Science;
            }
            set
            {
                _model.Science = value;
                NotifyOfPropertyChange(() => Science);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox1
        {
            get
            {
                return _model.Xbox1;
            }
            set
            {
                _model.Xbox1 = value;
                NotifyOfPropertyChange(() => Xbox1);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox2
        {
            get
            {
                return _model.Xbox2;
            }
            set
            {
                _model.Xbox2 = value;
                NotifyOfPropertyChange(() => Xbox2);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox3
        {
            get
            {
                return _model.Xbox3;
            }
            set
            {
                _model.Xbox3 = value;
                NotifyOfPropertyChange(() => Xbox3);
            }
        }
        public XboxControllerInputSettingsViewModel Xbox4
        {
            get
            {
                return _model.Xbox4;
            }
            set
            {
                _model.Xbox4 = value;
                NotifyOfPropertyChange(() => Xbox4);
            }
        }
        public GPSSettingsViewModel GPS
        {
            get
            {
                return _model.GPS;
            }
            set
            {
                _model.GPS = value;
                NotifyOfPropertyChange(() => GPS);
            }
        }
        public PowerSettingsViewModel Power
        {
            get
            {
                return _model.Power;
            }
            set
            {
                _model.Power = value;
                NotifyOfPropertyChange(() => Power);
            }
        }
        public NetworkManagerSettingsViewModel Network
        {
            get
            {
                return _model.Network;
            }
            set
            {
                _model.Network = value;
                NotifyOfPropertyChange(() => Network);
            }
        }

        /// <summary>
        /// Default constructor. For every element in the control center view that corresponds to a settings section,
        /// construct it so that its gui gets populated. These elements are hardcoded in, IE there's a section 
        /// for 4 xbox controllers specifically and so on. 
        /// </summary>
        /// <param name="configManager">the configuration manager that contains the default setting configurations
        /// for the initial settings.</param>
        /// <param name="cc">the control center view model. This class keeps a handle to it so we can manipulate
        /// the elements in it when the settings change, such as changing the operation of its first xbox 
        /// controller when the user changes its deadband settings.</param>
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
                Drive = DriveConfig.DefaultConfig,
                Xbox1 = XboxControllerInputConfig.DefaultConfig,
                Xbox2 = XboxControllerInputConfig.DefaultConfig,
                Xbox3 = XboxControllerInputConfig.DefaultConfig,
                Xbox4 = XboxControllerInputConfig.DefaultConfig,
                GPS = GPSConfig.DefaultConfig,
                Science = ScienceConfig.DefaultConfig,
                Power = PowerConfig.DefaultConfig,
                Network = NetworkManagerConfig.DefaultConfig
            };
        }
    }
}
