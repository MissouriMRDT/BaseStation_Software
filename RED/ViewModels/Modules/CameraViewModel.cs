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

        public byte Mux1Index
        {
            get
            {
                return _model.Mux1Index;
            }
            set
            {
                _model.Mux1Index = value;
                NotifyOfPropertyChange(() => Mux1Index);
            }
        }
        public byte Mux2Index
        {
            get
            {
                return _model.Mux2Index;
            }
            set
            {
                _model.Mux2Index = value;
                NotifyOfPropertyChange(() => Mux2Index);
            }
        }

        public CameraViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new CameraModel();
            _router = router;
            _idResolver = idResolver;
        }

        public void SetMux1()
        {
            _router.Send(_idResolver.GetId("CameraMuxChannel1"), Mux1Index);
        }

        public void SetMux2()
        {
            _router.Send(_idResolver.GetId("CameraMuxChannel2"), Mux1Index);
        }
    }
}