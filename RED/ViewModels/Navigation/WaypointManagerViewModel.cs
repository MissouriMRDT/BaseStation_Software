using Caliburn.Micro;
using RED.Models.Navigation;
using RED.ViewModels.Modules;
using System;
using System.Collections.ObjectModel;

namespace RED.ViewModels.Navigation
{
    public class WaypointManagerViewModel : PropertyChangedBase
    {
        private readonly WaypointManagerModel _model;

        public MapViewModel Map
        {
            get
            {
                return _model.Map;
            }
            private set
            {
                _model.Map = value;
                NotifyOfPropertyChange(() => Map);
            }
        }
        public GPSViewModel GPSModule
        {
            get
            {
                return _model.GPSModule;
            }
            private set
            {
                _model.GPSModule = value;
                NotifyOfPropertyChange(() => GPSModule);
            }
        }
        public AutonomyViewModel AutonomyModule
        {
            get
            {
                return _model.AutonomyModule;
            }
            private set
            {
                _model.AutonomyModule = value;
                NotifyOfPropertyChange(() => AutonomyModule);
            }
        }

        public ObservableCollection<Waypoint> Waypoints
        {
            get
            {
                return _model.Waypoints;
            }
            private set
            {
                _model.Waypoints = value;
                NotifyOfPropertyChange(() => Waypoints);
            }
        }
        public Waypoint SelectedWaypoint
        {
            get
            {
                return _model.SelectedWaypoint;
            }
            set
            {
                _model.SelectedWaypoint = value;
                NotifyOfPropertyChange(() => SelectedWaypoint);
            }
        }

        public WaypointManagerViewModel(MapViewModel map, GPSViewModel gps, AutonomyViewModel autonomy)
        {
            _model = new WaypointManagerModel();

            Map = map;
            GPSModule = gps;
            AutonomyModule = autonomy;

            GPSModule.PropertyChanged += GPSModule_PropertyChanged;

            Waypoints = new ObservableCollection<Waypoint>();

            AddWaypoint(new Waypoint("SDELC", 37.951631, -91.777713));
            AddWaypoint(new Waypoint("Fugitive Beach", 37.850025, -91.701845));
            AddWaypoint(new Waypoint("MDRS", 38.406426, -110.791919));
        }

        void GPSModule_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentLocation")
            {
                var loc = GPSModule.CurrentLocation;
                Map.CurrentLocation.Latitude = loc.Latitude;
                Map.CurrentLocation.Longitude = loc.Longitude;
                Map.RefreshMap();
            }
        }

        public static double ParseCoordinate(string coord)
        {
            string[] input = coord.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
            switch (input.Length)
            {
                case 1:
                    {
                        if (!Double.TryParse(input[0], out double value0))
                            throw new ArgumentException();
                        return value0;
                    }
                case 2:
                    {
                        if (!Int32.TryParse(input[0], out int value0) || !Double.TryParse(input[1], out double value1))
                            throw new ArgumentException();
                        return (value0) + Math.Sign(value0) * (value1 * 1 / 60d);
                    }
                case 3:
                    {
                        if (!Int32.TryParse(input[0], out int value0) || !Int32.TryParse(input[1], out int value1) || !Double.TryParse(input[2], out double value2))
                            throw new ArgumentException();
                        return (value0) + Math.Sign(value0) * ((value1 * 1 / 60d) + (value2 * 1 / 60d / 60d));
                    }
                default:
                    throw new ArgumentException();
            }
        }

        public bool AddWaypoint(string name, string latitude, string longitude)
        {
            try
            {
                double lat = WaypointManagerViewModel.ParseCoordinate(latitude);
                double lon = WaypointManagerViewModel.ParseCoordinate(longitude);
                AddWaypoint(new Waypoint(name, lat, lon) { Color = System.Windows.Media.Colors.Red });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddWaypoint(Waypoint waypoint)
        {
            Waypoints.Add(waypoint);
            Map.Waypoints.Add(waypoint);
            Map.RefreshMap();
        }

        public void RemoveSelectedWaypoint()
        {
            Map.Waypoints.Remove(SelectedWaypoint);
            Map.RefreshMap();
            Waypoints.Remove(SelectedWaypoint);
        }

        public void CurrentLocationToWaypoint()
        {
            AddWaypoint(Map.CurrentLocation);
        }

        public void SendSelectedToAutonomy()
        {
            if (SelectedWaypoint != null)
                AutonomyModule.AddWaypoint(SelectedWaypoint);
        }
    }
}
