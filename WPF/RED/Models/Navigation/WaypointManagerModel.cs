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
        internal double Latitude;
        internal double Longitude;
        internal double LatitudeD;
        internal double LongitudeD;
        internal double LatitudeM;
        internal double LongitudeM;
        internal double LatitudeS;
        internal double LongitudeS;
    }
}
