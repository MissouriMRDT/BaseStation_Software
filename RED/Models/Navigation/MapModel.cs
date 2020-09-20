using Core;
using Core.ViewModels;
using GMap.NET.WindowsPresentation;
using RED.Addons.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class MapModel
    {
        internal Waypoint CurrentLocation = new Waypoint(0, 0);

        internal WaypointManager Manager;

        internal GPSCoordinate StartPosition;
        internal bool ShowEmptyTiles;

        internal int CachePrefetchStartZoom;
        internal int CachePrefetchStopZoom;
        internal float Heading = 0;

        internal GMapControl MainMap;
    }
}
