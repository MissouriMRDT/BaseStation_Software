using Core.ViewModels;
using System.Collections.ObjectModel;

namespace Core
{
    public class WaypointManager
    {

        private static WaypointManager instance;

        private WaypointManager()
        {
            Waypoints = new ObservableCollection<Waypoint>();
            SelectedWaypoint = new Waypoint("", 0, 0);
        }

        public static WaypointManager Instance
        {
            get
            {
                if (instance == null) instance = new WaypointManager();
                return instance;
            }
        }

        public ObservableCollection<Waypoint> Waypoints;
        public Waypoint SelectedWaypoint;
        
    }
}
