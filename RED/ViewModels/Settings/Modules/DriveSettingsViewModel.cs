using Caliburn.Micro;
using RED.ViewModels.Modules;

namespace RED.ViewModels.Settings.Modules
{
    public class DriveSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private DriveViewModel _vm;

        public int SpeedLimit
        {
            get
            {
                return _vm.SpeedLimit;
            }
            set
            {
                _vm.SpeedLimit = value;
                _settings.CurrentSettings.DriveSpeedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
            }
        }

        public bool UseLegacyDataIds
        {
            get
            {
                return _vm.UseLegacyDataIds;
            }
            set
            {
                _vm.UseLegacyDataIds = value;
                _settings.CurrentSettings.DriveUseLegacyDataIds = value;
                NotifyOfPropertyChange(() => UseLegacyDataIds);
            }
        }

        public DriveSettingsViewModel(SettingsManagerViewModel settings, DriveViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SpeedLimit = _settings.CurrentSettings.DriveSpeedLimit;
            _vm.UseLegacyDataIds = _settings.CurrentSettings.DriveUseLegacyDataIds;
        }
    }
}
