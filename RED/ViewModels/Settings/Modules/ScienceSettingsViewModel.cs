using Caliburn.Micro;
using RED.Contexts.Modules;
using RED.ViewModels.Modules;
using System.Net;

namespace RED.ViewModels.Settings.Modules
{
    public class ScienceSettingsViewModel : PropertyChangedBase
    {
        private readonly ScienceSettingsContext _settings;
        private readonly ScienceViewModel _vm;

        public string SpectrometerIPAddress
        {
            get
            {
                return _vm.SpectrometerIPAddress.ToString();
            }
            set
            {
                _vm.SpectrometerIPAddress = IPAddress.TryParse(value, out IPAddress ip) ? ip : IPAddress.None;
                _settings.SpectrometerIPAddress = value;
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
                _settings.SpectrometerPortNumber = value;
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
                _settings.SpectrometerFilePath = value;
                NotifyOfPropertyChange(() => SpectrometerFilePath);
            }
        }

        public ScienceSettingsViewModel(ScienceSettingsContext settings, ScienceViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SpectrometerIPAddress = IPAddress.TryParse(_settings.SpectrometerIPAddress, out IPAddress ip) ? ip : IPAddress.None;
            _vm.SpectrometerPortNumber = _settings.SpectrometerPortNumber;
            _vm.SpectrometerFilePath = _settings.SpectrometerFilePath;
        }
    }
}