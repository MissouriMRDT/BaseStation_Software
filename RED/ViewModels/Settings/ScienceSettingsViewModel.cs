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

        public short CCDPixelCount
        {
            get
            {
                return _vm.CCDPixelCount;
            }
            set
            {
                _vm.CCDPixelCount = value;
                _settings.CurrentSettings.ScienceCCDPixelCount = value;
                NotifyOfPropertyChange(() => CCDPixelCount);
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

            _vm.CCDPixelCount = _settings.CurrentSettings.ScienceCCDPixelCount;
            _vm.CCDFilePath = _settings.CurrentSettings.ScienceCCDFilePath;
        }
    }
}