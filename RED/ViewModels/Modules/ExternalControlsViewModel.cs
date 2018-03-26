using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class ExternalControlsViewModel : PropertyChangedBase
    {
        private readonly IRovecomm _rovecomm;
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

        public ExternalControlsViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver)
        {
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
        }

        public void EnableAll()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalEnableAll"), GimbalEnableCommand, true);
        }
        public void DisableAll()
        {
            _rovecomm.SendCommand(_idResolver.GetId("GimbalEnableAll"), GimbalDisableCommand, true);
        }

        public void ExternalControlsReset()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ExternalControlsReset"), ResetCode, true);
        }
    }
}
