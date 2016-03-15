using Caliburn.Micro;
using RED.Models;
using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RED.Interfaces;

namespace RED.ViewModels.Settings
{
    public class InputSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private InputManagerViewModel _manager;

        public int SerialReadSpeed
        {
            get
            {
                return _manager.SerialReadSpeed;
            }
            set
            {
                _manager.SerialReadSpeed = value;
                _settings.CurrentSettings.InputSerialReadSpeed = value;
                NotifyOfPropertyChange(() => SerialReadSpeed);
            }
        }

        public bool AutoDeadzone
        {
            get
            {
                return _manager.Input.AutoDeadzone;
            }
            set
            {
                _manager.Input.AutoDeadzone = value;
                _settings.CurrentSettings.InputAutoDeadzone = value;
                NotifyOfPropertyChange(() => AutoDeadzone);
            }
        }
        public int ManualDeadzone
        {
            get
            {
                return _manager.Input.ManualDeadzone;
            }
            set
            {
                _manager.Input.ManualDeadzone = value;
                _settings.CurrentSettings.InputManualDeadzone = value;
                NotifyOfPropertyChange(() => ManualDeadzone);
            }
        }

        public List<string> DeviceType
        {
            get
            {
                return new List<string> { "Xbox", "Keyboard", "Flight Stick" };
            }
        }

        public string SelectedDevice
        {
            get
            {
                return _manager.SelectedDevice;
            }

            set
            {
                _manager.SelectedDevice = value;
                _manager.NotifyOfPropertyChange(() => _manager.SelectedDevice);
                System.Console.Write("SELECTED: ");
                System.Console.WriteLine(_manager.SelectedDevice);
            }
        }

        public InputSettingsViewModel(SettingsManagerViewModel settings, InputManagerViewModel manager)
        {
            _settings = settings;
            _manager = manager;

            _manager.SerialReadSpeed = _settings.CurrentSettings.InputSerialReadSpeed;
            _manager.Input.AutoDeadzone = _settings.CurrentSettings.InputAutoDeadzone;
            _manager.Input.ManualDeadzone = _settings.CurrentSettings.InputManualDeadzone;
            _manager.SelectedDevice = _settings.CurrentSettings.InputSelectedDevice;
        }
    }
}
