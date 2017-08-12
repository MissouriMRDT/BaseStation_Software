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

        private const string SettingsFileName = "GeneralSettings";

        public REDSettingsContext CurrentSettingsConfig
        {
            get;
            private set;
        }
        public Properties.Settings CurrentSettings
        {
            get
            {
                return Properties.Settings.Default;
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
        public InputSettingsViewModel Input
        {
            get
            {
                return _model.input;
            }
            set
            {
                _model.input = value;
                NotifyOfPropertyChange(() => Input);
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

            _configManager.AddRecord(SettingsFileName, GetDefaultConfig());
            CurrentSettingsConfig = _configManager.GetConfig<REDSettingsContext>(SettingsFileName);

            Drive = new DriveSettingsViewModel(this, cc.Drive);
            Science = new ScienceSettingsViewModel(CurrentSettingsConfig.Science, cc.Science);
            Input = new InputSettingsViewModel(this, cc.InputManager);
            Xbox = new XboxControllerInputSettingsViewModel(this, cc.XboxController1);
            GPS = new GPSSettingsViewModel(this, cc.GPS);
        }

        public void SaveSettings()
        {
            CurrentSettings.Save();
            _configManager.SetConfig(SettingsFileName, CurrentSettingsConfig);
        }

        static REDSettingsContext GetDefaultConfig()
        {
            return new REDSettingsContext()
            {
                Science = ScienceSettingsViewModel.DefaultConfig
            };
        }
    }
}