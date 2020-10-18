using RoverOverviewNetwork.Models;
using Caliburn.Micro;

namespace RoverOverviewNetwork.ViewModels
{
    public class NetworkMapViewModel : PropertyChangedBase
    {
        private readonly NetworkMapModel _model;

        public PingToolViewModel PingTool
        {
            get
            {
                return _model.PingTool;
            }
            private set
            {
                _model.PingTool = value;
                NotifyOfPropertyChange(() => PingTool);
            }
        }

        public NetworkMapViewModel(PingToolViewModel pingTool)
        {
            _model = new NetworkMapModel();
            PingTool = pingTool;
		}
	}
}
