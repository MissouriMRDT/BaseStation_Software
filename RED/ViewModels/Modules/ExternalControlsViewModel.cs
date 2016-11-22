using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class ExternalControlsViewModel
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        private const byte GimbalDisableCommand = 0x00;
        private const byte GimbalEnableCommand = 0x01;

        public ExternalControlsViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _router = router;
            _idResolver = idResolver;
        }

        public void EnableAll()
        {
            _router.Send(_idResolver.GetId("GimbalEnableAll"), GimbalEnableCommand);
        }
        public void DisableAll()
        {
            _router.Send(_idResolver.GetId("GimbalEnableAll"), GimbalDisableCommand);
        }
    }
}
