using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class WaypointManagerModel
    {
        internal MapViewModel Map;
        internal GPSViewModel GPSModule;

        internal ObservableCollection<Waypoint> Waypoints;
        internal Waypoint SelectedWaypoint;
    }
}
