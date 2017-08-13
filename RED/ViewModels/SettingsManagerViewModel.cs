using Caliburn.Micro;
using RED.Contexts;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.Settings.Input;
using RED.ViewModels.Settings.Input.Controllers;
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
        public XboxControllerInputSettingsViewModel Xbox
        {
            get
            {
                return _model.xbox;
            }
            set
            {
                _model.xbox = value;
                NotifyOfPropertyChange(() => Xbox);
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

        public SettingsManagerViewModel(IConfigurationManager configManager, ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;
            _configManager = configManager;

            _configManager.AddRecord(SettingsConfigName, GetDefaultConfig());
            CurrentSettingsConfig = _configManager.GetConfig<REDSettingsContext>(SettingsConfigName);

            Drive = new DriveSettingsViewModel(CurrentSettingsConfig.Drive, cc.Drive);
            Science = new ScienceSettingsViewModel(CurrentSettingsConfig.Science, cc.Science);
            Xbox = new XboxControllerInputSettingsViewModel(CurrentSettingsConfig.Xbox1, cc.XboxController1);
            GPS = new GPSSettingsViewModel(CurrentSettingsConfig.GPS, cc.GPS, cc.Map);
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
                GPS = GPSSettingsViewModel.DefaultConfig,
                Science = ScienceSettingsViewModel.DefaultConfig
            };
        }
    }
}