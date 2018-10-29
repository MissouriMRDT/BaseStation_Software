using RoverNetworkManager.Models;
using Caliburn.Micro;

namespace RoverNetworkManager.ViewModels
{
    public class NetworkMapViewModel : PropertyChangedBase
    {
        private readonly NetworkMapModel _model;

        public NetworkMapViewModel()
        {
            _model = new NetworkMapModel();
		}
	}
}
