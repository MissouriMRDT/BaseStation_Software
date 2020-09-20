using Core;
using Core.Models;
using Core.ViewModels;
using RED.ViewModels.Modules;
using RED.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class WaypointManagerModel
    {
        internal MapViewModel Map;
        internal GPSViewModel GPSModule;

        internal WaypointManager Manager;
        internal Waypoint NewPoint;

        internal string Name;
        internal float Latitude;
        internal float Longitude;
        internal float LatitudeD;
        internal float LongitudeD;
        internal float LatitudeM;
        internal float LongitudeM;
        internal float LatitudeS;
        internal float LongitudeS;
    }
}
