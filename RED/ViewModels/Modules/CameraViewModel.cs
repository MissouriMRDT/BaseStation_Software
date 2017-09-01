using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class CameraViewModel : PropertyChangedBase
    {
        private readonly IDataRouter _router;
        private readonly IDataIdResolver _idResolver;

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