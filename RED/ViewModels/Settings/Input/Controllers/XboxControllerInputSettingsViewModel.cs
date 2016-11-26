using Caliburn.Micro;
using RED.ViewModels.Input.Controllers;

namespace RED.ViewModels.Settings.Input.Controllers
{
    public class XboxControllerInputSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
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
                _settings.CurrentSettings.InputAutoDeadzone = value;
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
                _settings.CurrentSettings.InputManualDeadzone = value;
                NotifyOfPropertyChange(() => ManualDeadzone);
            }
        }

        public XboxControllerInputSettingsViewModel(SettingsManagerViewModel settings, XboxControllerInputViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.AutoDeadzone = _settings.CurrentSettings.InputAutoDeadzone;
            _vm.ManualDeadzone = _settings.CurrentSettings.InputManualDeadzone;
        }
    }
}
