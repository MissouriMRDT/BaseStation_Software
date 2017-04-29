using Caliburn.Micro;
using RED.ViewModels.Modules;
using System.Net;

namespace RED.ViewModels.Settings.Modules
{
    public class ScienceSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private ScienceViewModel _vm;

        public string SpectrometerIPAddress
        {
            get
            {
                return _vm.SpectrometerIPAddress.ToString();
            }
            set
            {
                IPAddress ip;
                _vm.SpectrometerIPAddress = IPAddress.TryParse(value, out ip) ? ip : IPAddress.None;
                _settings.CurrentSettings.ScienceSpectrometerIPAddress = value;
                NotifyOfPropertyChange(() => SpectrometerIPAddress);
            }
        }
        public ushort SpectrometerPortNumber
        {
            get
            {
                return _vm.SpectrometerPortNumber;
            }
            set
            {
                _vm.SpectrometerPortNumber = value;
                _settings.CurrentSettings.ScienceSpectrometerPortNumber = value;
                NotifyOfPropertyChange(() => SpectrometerPortNumber);
            }
        }
        public string SpectrometerFilePath
        {
            get
            {
                return _vm.SpectrometerFilePath;
            }
            set
            {
                _vm.SpectrometerFilePath = value;
                _settings.CurrentSettings.ScienceSpectrometerFilePath = value;
                NotifyOfPropertyChange(() => SpectrometerFilePath);
            }
        }

        public ScienceSettingsViewModel(SettingsManagerViewModel settings, ScienceViewModel server)
        {
            _settings = settings;
            _vm = server;

            IPAddress ip;
            _vm.SpectrometerIPAddress = IPAddress.TryParse(_settings.CurrentSettings.ScienceSpectrometerIPAddress, out ip) ? ip : IPAddress.None;
            _vm.SpectrometerPortNumber = _settings.CurrentSettings.ScienceSpectrometerPortNumber;
            _vm.SpectrometerFilePath = _settings.CurrentSettings.ScienceSpectrometerFilePath;
        }
    }
}