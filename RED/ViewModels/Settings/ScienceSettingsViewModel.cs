using Caliburn.Micro;
using RED.ViewModels.ControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Settings
{
    public class ScienceSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private ScienceViewModel _vm;

        public ushort CCDPortNumber
        {
            get
            {
                return _vm.CCDPortNumber;
            }
            set
            {
                _vm.CCDPortNumber = value;
                _settings.CurrentSettings.ScienceCCDPortNumber = value;
                NotifyOfPropertyChange(() => CCDPortNumber);
            }
        }
        public string CCDFilePath
        {
            get
            {
                return _vm.CCDFilePath;
            }
            set
            {
                _vm.CCDFilePath = value;
                _settings.CurrentSettings.ScienceCCDFilePath = value;
                NotifyOfPropertyChange(() => CCDFilePath);
            }
        }

        public ScienceSettingsViewModel(SettingsManagerViewModel settings, ScienceViewModel server)
        {
            _settings = settings;
            _vm = server;

            _vm.CCDPortNumber = _settings.CurrentSettings.ScienceCCDPortNumber;
            _vm.CCDFilePath = _settings.CurrentSettings.ScienceCCDFilePath;
        }
    }
}