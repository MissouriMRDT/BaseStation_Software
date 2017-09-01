using Caliburn.Micro;
using RED.Contexts.Input.Controllers;
using RED.ViewModels.Input.Controllers;

namespace RED.ViewModels.Settings.Input.Controllers
{
    public class XboxControllerInputSettingsViewModel : PropertyChangedBase
    {
        private readonly XboxControllerSettingsContext _settings;
        private readonly XboxControllerInputViewModel _vm;

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }
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

        public XboxControllerInputSettingsViewModel(XboxControllerSettingsContext settings, XboxControllerInputViewModel vm, int index)
        {
            _settings = settings;
            _vm = vm;
            Index = index;

            _vm.AutoDeadzone = _settings.AutoDeadzone;
            _vm.ManualDeadzone = _settings.ManualDeadzone;
        }
    }
}
