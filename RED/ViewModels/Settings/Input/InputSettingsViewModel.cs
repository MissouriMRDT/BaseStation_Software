using Caliburn.Micro;
using RED.ViewModels.Input;

namespace RED.ViewModels.Settings.Input
{
    public class InputSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private InputManagerViewModel _vm;

        public int SerialReadSpeed
        {
            get
            {
                return _vm.DefaultSerialReadSpeed;
            }
            set
            {
                _vm.DefaultSerialReadSpeed = value;
                _settings.CurrentSettings.InputSerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
            }
        }

        public InputSettingsViewModel(SettingsManagerViewModel settings, InputManagerViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.DefaultSerialReadSpeed = _settings.CurrentSettings.InputSerialReadSpeed;
        }
    }
}
