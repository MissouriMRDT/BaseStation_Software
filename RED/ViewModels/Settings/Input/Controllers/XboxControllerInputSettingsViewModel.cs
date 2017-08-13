using Caliburn.Micro;
using RED.Contexts;
using RED.ViewModels.Input.Controllers;

namespace RED.ViewModels.Settings.Input.Controllers
{
    public class XboxControllerInputSettingsViewModel : PropertyChangedBase
    {
        private XboxControllerSettingsContext _settings;
        private XboxControllerInputViewModel _vm;

        public bool AutoDeadzone
        {
            get
            {
                return _vm.AutoDeadzone;
            }
            set
            {
                _vm.AutoDeadzone = value;
                _settings.AutoDeadzone = value;
                NotifyOfPropertyChange(() => AutoDeadzone);
            }
        }
        public int ManualDeadzone
        {
            get
            {
                return _vm.ManualDeadzone;
            }
            set
            {
                _vm.ManualDeadzone = value;
                _settings.ManualDeadzone = value;
                NotifyOfPropertyChange(() => ManualDeadzone);
            }
        }

        public XboxControllerInputSettingsViewModel(XboxControllerSettingsContext settings, XboxControllerInputViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.AutoDeadzone = _settings.AutoDeadzone;
            _vm.ManualDeadzone = _settings.ManualDeadzone;
        }

        public static XboxControllerSettingsContext DefaultConfig = new XboxControllerSettingsContext()
        {
            AutoDeadzone = false,
            ManualDeadzone = 5000
        };
    }
}
