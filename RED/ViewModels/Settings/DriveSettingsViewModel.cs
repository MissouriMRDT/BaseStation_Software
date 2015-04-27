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
    public class DriveSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private DriveControllerModeViewModel _vm;

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

        public bool ParabolicScaling
        {
            get
            {
                return _vm.ParabolicScaling;
            }
            set
            {
                _vm.ParabolicScaling = value;
                _settings.CurrentSettings.DriveParabolicScaling = value;
                NotifyOfPropertyChange(() => ParabolicScaling);
            }
        }

        public DriveSettingsViewModel(SettingsManagerViewModel settings, DriveControllerModeViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SpeedLimit = _settings.CurrentSettings.DriveSpeedLimit;
            _vm.ParabolicScaling = _settings.CurrentSettings.DriveParabolicScaling;
        }
    }
}
