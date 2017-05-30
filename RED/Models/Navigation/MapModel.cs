using RED.Addons;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class MapModel
    {
        internal GPSCoordinate currentLocation = new GPSCoordinate(0, 0);
        internal ObservableCollection<GPSCoordinate> waypoints = new ObservableCollection<GPSCoordinate>();
    }
}
