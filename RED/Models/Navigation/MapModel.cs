using GMap.NET.WindowsPresentation;
using RED.Addons.Navigation;
using RED.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class MapModel
    {
        internal Waypoint CurrentLocation = new Waypoint(0, 0);
        internal ObservableCollection<Waypoint> Waypoints = new ObservableCollection<Waypoint>();

        internal GPSCoordinate StartPosition;
        internal bool ShowEmptyTiles;

        internal int CachePrefetchStartZoom;
        internal int CachePrefetchStopZoom;

        internal GMapControl MainMap;
    }
}
