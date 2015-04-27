using Caliburn.Micro;
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

        public DriveSettingsViewModel(SettingsManagerViewModel settings, DriveControllerMode vm)
        {
            _settings = settings;
            _vm = vm;

        }
    }
}
