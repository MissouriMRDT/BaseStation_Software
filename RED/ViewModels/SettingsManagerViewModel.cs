using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.Settings.Input;
using RED.ViewModels.Settings.Modules;

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

        public SettingsManagerViewModel(ControlCenterViewModel cc)
        {
            _model = new SettingsManagerModel();
            _controlCenter = cc;

            Drive = new DriveSettingsViewModel(this, (Modules.DriveControllerModeViewModel)cc.IManager.Input.ControllerModes[0]);
            Science = new ScienceSettingsViewModel(this, cc.Science);
            Input = new InputSettingsViewModel(this, cc.IManager.Input);
            GPS = new GPSSettingsViewModel(this, cc.GPS);
        }

        public void SaveSettings()
        {
            CurrentSettings.Save();
        }
    }
}