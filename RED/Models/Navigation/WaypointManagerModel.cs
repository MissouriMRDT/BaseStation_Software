using RED.Addons;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class WaypointManagerModel
    {
        internal MapViewModel Map;
        internal AutonomyViewModel AutonomyModule;

        internal ObservableCollection<Waypoint> Waypoints;
    }
}
