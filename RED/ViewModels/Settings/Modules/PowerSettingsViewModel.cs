﻿using Caliburn.Micro;
using RED.Contexts.Modules;
using RED.ViewModels.Modules;

namespace RED.ViewModels.Settings.Modules
{
    public class PowerSettingsViewModel : PropertyChangedBase
    {
        private readonly PowerSettingsContext _settings;
        private readonly PowerViewModel _vm;

        public bool AutoStartLog
        {
            get
            {
                return _vm.AutoStartLog;
            }
            set
            {
                _vm.AutoStartLog = value;
                _settings.AutoStartLog = value;
                NotifyOfPropertyChange(() => AutoStartLog);
            }
        }

        public byte RebootTime
        {
            get
            {
                return _vm.RebootTime;
            }
            set
            {
                _vm.RebootTime = value;
                _settings.RebootTime = value;
                NotifyOfPropertyChange(() => RebootTime);
            }
        }

        public PowerSettingsViewModel(PowerSettingsContext settings, PowerViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.AutoStartLog = settings.AutoStartLog;
            _vm.RebootTime = settings.RebootTime;
        }
    }
}
