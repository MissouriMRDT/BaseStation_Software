using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;

namespace RED.ViewModels.Modules
{
    public class CameraViewModel : PropertyChangedBase
    {
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public CameraViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _router = router;
            _idResolver = idResolver;
        }

        public void SetMux1(byte index)
        {
            _router.Send(_idResolver.GetId("CameraMuxChannel1"), index, true);
        }

        public void SetMux2(byte index)
        {
            _router.Send(_idResolver.GetId("CameraMuxChannel2"), index, true);
        }
    }
}