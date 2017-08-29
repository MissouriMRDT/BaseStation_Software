﻿using GMap.NET;
using RED.Addons;
using RED.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace RED.Models.Navigation
{
    internal class MapModel
    {
        internal Waypoint currentLocation = new Waypoint(0, 0);
        internal ObservableCollection<Waypoint> waypoints = new ObservableCollection<Waypoint>();

        internal GPSCoordinate StartPosition;

        internal int CachePrefetchStartZoom;
        internal int CachePrefetchStopZoom;
    }
}