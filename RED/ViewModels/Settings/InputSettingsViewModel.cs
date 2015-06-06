using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Settings
{
    public class InputSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private InputViewModel _vm;

        public int SerialReadSpeed
        {
            get
            {
                return _vm.SerialReadSpeed;
            }
            set
            {
                _vm.SerialReadSpeed = value;
                _settings.CurrentSettings.InputSerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
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

        public InputSettingsViewModel(SettingsManagerViewModel settings, InputViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SerialReadSpeed = _settings.CurrentSettings.InputSerialReadSpeed;
            _vm.AutoDeadzone = _settings.CurrentSettings.InputAutoDeadzone;
            _vm.ManualDeadzone = _settings.CurrentSettings.InputManualDeadzone;
        }
    }
}
