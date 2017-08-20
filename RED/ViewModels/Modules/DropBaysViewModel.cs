using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class DropBaysViewModel : PropertyChangedBase
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

        public DropBaysViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _router = router;
            _idResolver = idResolver;
            _log = log;
        }

        public void OpenBay(byte index)
        {
            _router.Send(_idResolver.GetId("DropBayOpen"), index, true);
            _log.Log("Drop bay #{0} opened.", index + 1);
        }

        public void CloseBay(byte index)
        {
            _router.Send(_idResolver.GetId("DropBayClose"), index, true);
            _log.Log("Drop bay #{0} closed.", index + 1);
        }
    }
}