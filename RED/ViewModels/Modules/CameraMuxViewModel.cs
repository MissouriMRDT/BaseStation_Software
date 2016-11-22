using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;

namespace RED.ViewModels.Modules
{
    public class CameraMuxViewModel : PropertyChangedBase
    {
        private CameraMuxModel _model;
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

        public CameraMuxViewModel(IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new CameraMuxModel();
            _router = router;
            _idResolver = idResolver;
        }

        public void SetMux()
        {
            _router.Send(_idResolver.GetId("CameraMuxSet"), MuxIndex);
        }
    }
}