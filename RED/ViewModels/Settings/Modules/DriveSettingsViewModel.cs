using Caliburn.Micro;
using RED.Contexts.Modules;
using RED.ViewModels.Modules;

namespace RED.ViewModels.Settings.Modules
{
    public class DriveSettingsViewModel : PropertyChangedBase
    {
        private readonly DriveSettingsContext _settings;
        private readonly DriveViewModel _vm;

        public int SpeedLimit
        {
            get
            {
                return _vm.SpeedLimit;
            }
            set
            {
                _vm.SpeedLimit = value;
                _settings.SpeedLimit = value;
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
                _settings.UseLegacyDataIds = value;
                NotifyOfPropertyChange(() => UseLegacyDataIds);
            }
        }

        public DriveSettingsViewModel(DriveSettingsContext settings, DriveViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SpeedLimit = _settings.SpeedLimit;
            _vm.UseLegacyDataIds = _settings.UseLegacyDataIds;
        }
    }
}
