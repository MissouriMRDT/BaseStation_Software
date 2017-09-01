using Caliburn.Micro;
using RED.Contexts.Network;
using RED.ViewModels.Network;

namespace RED.ViewModels.Settings.Network
{
    public class NetworkManagerSettingsViewModel : PropertyChangedBase
    {
        private readonly NetworkManagerSettingsContext _settings;
        private readonly NetworkManagerViewModel _vm;

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
    }
}
