using Caliburn.Micro;
using RED.Contexts;
using RED.ViewModels.Modules;
using System.Net;

namespace RED.ViewModels.Settings.Modules
{
    public class ScienceSettingsViewModel : PropertyChangedBase
    {
        private ScienceSettingsContext _settings;
        private ScienceViewModel _vm;

        public IPAddress SpectrometerIPAddress
        {
            get
            {
                return _vm.SpectrometerIPAddress;
            }
            set
            {
                IPAddress ip;
                _vm.SpectrometerIPAddress = value;
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

        public ScienceSettingsViewModel(ScienceSettingsContext settings, ScienceViewModel server)
        {
            _settings = settings;
            _vm = server;

            _vm.SpectrometerIPAddress = _settings.SpectrometerIPAddress;
            _vm.SpectrometerPortNumber = _settings.SpectrometerPortNumber;
            _vm.SpectrometerFilePath = _settings.SpectrometerFilePath;
        }

        public static ScienceSettingsContext DefaultConfig = new ScienceSettingsContext()
        {
            SpectrometerIPAddress = new IPAddress(new byte[] { 192, 168, 1, 135 }),
            SpectrometerPortNumber = 11001,
            SpectrometerFilePath = System.String.Empty
        };
    }
}