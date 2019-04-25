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
    }
}
