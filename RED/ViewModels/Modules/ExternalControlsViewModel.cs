using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Network;

namespace RED.ViewModels.Modules
{
    public class ExternalControlsViewModel : PropertyChangedBase
    {
        private readonly INetworkMessenger _networkMessenger;
        private readonly IDataIdResolver _idResolver;

        private const byte GimbalDisableCommand = 0x00;
        private const byte GimbalEnableCommand = 0x01;

        private byte resetCode;
        public byte ResetCode
        {
            get
            {
                return resetCode;
            }
            set
            {
                resetCode = value;
                NotifyOfPropertyChange(() => ResetCode);
            }
        }

        public ExternalControlsViewModel(INetworkMessenger networkMessenger, IDataIdResolver idResolver)
        {
            _networkMessenger = networkMessenger;
            _idResolver = idResolver;
        }

        public void EnableAll()
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("GimbalEnableAll"), GimbalEnableCommand, true);
        }
        public void DisableAll()
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("GimbalEnableAll"), GimbalDisableCommand, true);
        }

        public void ExternalControlsReset()
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("ExternalControlsReset"), ResetCode, true);
        }
    }
}
