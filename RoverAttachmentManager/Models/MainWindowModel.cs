using RoverAttachmentManager.ViewModels.Autonomy;
using RED.ViewModels;
using RED.ViewModels.Network;
using RED.Roveprotocol;
using RED.ViewModels.Navigation;
using RED.ViewModels.Modules;
using AutonomyViewModel = RoverAttachmentManager.ViewModels.Autonomy.AutonomyViewModel;

namespace RoverAttachmentManager.Models
{
    internal class MainWindowModel
    {
        internal ConsoleViewModel _console;
        internal NetworkManagerViewModel _networkManager;
        internal XMLConfigManager _configManager;
        internal MetadataManager _metadataManager;
        internal Rovecomm _rovecomm;
        internal GPSViewModel _GPS;
        internal MapViewModel _map;
        internal WaypointManagerViewModel _waypoint;
        internal AutonomyViewModel _autonomy;
    }
}
