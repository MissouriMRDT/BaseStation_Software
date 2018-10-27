using RoverNetworkManager.Models;
using RoverNetworkManager.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Controls;

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
