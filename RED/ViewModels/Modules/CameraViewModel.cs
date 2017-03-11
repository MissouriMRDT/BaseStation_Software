using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;

namespace RED.ViewModels.Modules
{
    public class CameraViewModel : PropertyChangedBase
    {
        private CameraModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public byte MuxIndex
        {
            get
            {
                return _model.MuxIndex;
            }
            set
            {
                _model.MuxIndex = value;
                NotifyOfPropertyChange(() => MuxIndex);
            }
        }

        public CameraViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new CameraModel();
            _router = router;
            _idResolver = idResolver;
        }

        public void SetMux()
        {
            _router.Send(_idResolver.GetId("CameraMuxSet"), MuxIndex);
        }
    }
}