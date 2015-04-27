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
        private DriveControllerMode _vm;

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

        public bool ScalingMode
        {
            get
            {
                return _vm.ParabolicScaling;
            }
            set
            {
                _vm.ParabolicScaling = value;
                _settings.CurrentSettings.DriveScalingMode = value;
                NotifyOfPropertyChange(() => ScalingMode);
            }
        }

        public DriveSettingsViewModel(SettingsManagerViewModel settings, DriveControllerMode vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.SpeedLimit = _settings.CurrentSettings.DriveSpeedLimit;
            _vm.ParabolicScaling = _settings.CurrentSettings.DriveScalingMode;
        }
    }
}
