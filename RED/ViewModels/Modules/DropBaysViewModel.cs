using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class DropBaysViewModel : PropertyChangedBase
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public DropBaysViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _router = router;
            _idResolver = idResolver;
        }

        public void OpenBay(byte index)
        {
            _router.Send(_idResolver.GetId("DropBayOpen"), index);
        }

        public void CloseBay(byte index)
        {
            _router.Send(_idResolver.GetId("DropBayClose"), index);
        }
    }
}