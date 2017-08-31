using Caliburn.Micro;
using RED.Contexts.Modules;
using RED.ViewModels.Modules;

namespace RED.ViewModels.Settings.Modules
{
    public class PowerSettingsViewModel : PropertyChangedBase
    {
        private PowerSettingsContext _settings;
        private PowerViewModel _vm;

        public bool AutoStartLog
        {
            get
            {
                return _vm.AutoStartLog;
            }
            set
            {
                _vm.AutoStartLog = value;
                _settings.AutoStartLog = value;
                NotifyOfPropertyChange(() => AutoStartLog);
            }
        }

        public PowerSettingsViewModel(PowerSettingsContext settings, PowerViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.AutoStartLog = settings.AutoStartLog;
        }

        public static PowerSettingsContext DefaultConfig = new PowerSettingsContext()
        {
            AutoStartLog = false
        };
    }
}
