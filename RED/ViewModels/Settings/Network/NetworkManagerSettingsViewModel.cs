﻿using Caliburn.Micro;
using RED.Contexts;
using RED.ViewModels.Network;

namespace RED.ViewModels.Settings.Network
{
    public class NetworkManagerSettingsViewModel : PropertyChangedBase
    {
        private NetworkManagerSettingsContext _settings;
        private NetworkManagerViewModel _vm;

        public bool EnableReliablePackets
        {
            get
            {
                return _vm.EnableReliablePackets;
            }
            set
            {
                _vm.EnableReliablePackets = value;
                _settings.EnableReliablePackets = value;
                NotifyOfPropertyChange(() => EnableReliablePackets);
            }
        }

        public NetworkManagerSettingsViewModel(NetworkManagerSettingsContext settings, NetworkManagerViewModel vm)
        {
            _settings = settings;
            _vm = vm;

            _vm.EnableReliablePackets = _settings.EnableReliablePackets;
        }

        public static NetworkManagerSettingsContext DefaultConfig = new NetworkManagerSettingsContext()
        {
            EnableReliablePackets = false
        };
    }
}