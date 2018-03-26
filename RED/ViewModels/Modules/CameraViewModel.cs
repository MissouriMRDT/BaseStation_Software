using Caliburn.Micro;
using RED.Interfaces;

namespace RED.ViewModels.Modules
{
    public class CameraViewModel : PropertyChangedBase
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;

        public CameraViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver)
        {
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
        }

        public void SetMux1(byte index)
        {
            _rovecomm.SendCommand(_idResolver.GetId("CameraMuxChannel1"), index, true);
        }

        public void SetMux2(byte index)
        {
            _rovecomm.SendCommand(_idResolver.GetId("CameraMuxChannel2"), index, true);
        }
    }
}