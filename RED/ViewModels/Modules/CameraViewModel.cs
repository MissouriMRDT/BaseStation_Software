using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Network;

namespace RED.ViewModels.Modules
{
    public class CameraViewModel : PropertyChangedBase
    {
        private readonly INetworkMessenger _networkMessenger;
        private readonly IDataIdResolver _idResolver;

        public CameraViewModel(INetworkMessenger networkMessenger, IDataIdResolver idResolver)
        {
            _networkMessenger = networkMessenger;
            _idResolver = idResolver;
        }

        public void SetMux1(byte index)
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("CameraMuxChannel1"), index, true);
        }

        public void SetMux2(byte index)
        {
            _networkMessenger.SendOverNetwork(_idResolver.GetId("CameraMuxChannel2"), index, true);
        }
    }
}