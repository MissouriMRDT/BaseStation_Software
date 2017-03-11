using Caliburn.Micro;
using RED.ViewModels.Modules;
using System.Net;

namespace RED.ViewModels.Settings.Modules
{
    public class ScienceSettingsViewModel : PropertyChangedBase
    {
        private SettingsManagerViewModel _settings;
        private ScienceViewModel _vm;

        public ScienceSettingsViewModel(SettingsManagerViewModel settings, ScienceViewModel server)
        {
            _settings = settings;
            _vm = server;
        }
    }
}