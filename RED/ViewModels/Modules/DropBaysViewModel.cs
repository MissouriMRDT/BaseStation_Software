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
            _router.Send(_idResolver.GetId("DropBayOpen"), index);
            _log.Log("Drop bay #" + (index + 1).ToString() + " opened.");
        }

        public void CloseBay(byte index)
        {
            _router.Send(_idResolver.GetId("DropBayClose"), index);
            _log.Log("Drop bay #" + (index + 1).ToString() + " closed.");
        }
    }
}