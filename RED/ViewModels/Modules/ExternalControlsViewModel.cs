using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class ExternalControlsViewModel : PropertyChangedBase
    {
        private readonly IDataRouter _router;
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

        public ExternalControlsViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _router = router;
            _idResolver = idResolver;
        }

        public void EnableAll()
        {
            _router.Send(_idResolver.GetId("GimbalEnableAll"), GimbalEnableCommand, true);
        }
        public void DisableAll()
        {
            _router.Send(_idResolver.GetId("GimbalEnableAll"), GimbalDisableCommand, true);
        }

        public void ExternalControlsReset()
        {
            _router.Send(_idResolver.GetId("ExternalControlsReset"), ResetCode, true);
        }
    }
}
