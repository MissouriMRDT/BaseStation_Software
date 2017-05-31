using Caliburn.Micro;
using RED.Addons;
using RED.ViewModels.Modules;
using RED.Models.Navigation;
using System;
using System.Collections.ObjectModel;

namespace RED.ViewModels.Navigation
{
    public class WaypointManagerViewModel : PropertyChangedBase
    {
        private WaypointManagerModel _model;

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

        public WaypointManagerViewModel(MapViewModel map, AutonomyViewModel autonomy)
        {
            _model = new WaypointManagerModel();

            Map = map;
            AutonomyModule = autonomy;

            Waypoints = new ObservableCollection<Waypoint>();
        }

        public static double ParseCoordinate(string coord)
        {
            string[] input = coord.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
            switch (input.Length)
            {
                case 1:
                    {
                        double value0;
                        if (!Double.TryParse(input[0], out value0))
                            throw new ArgumentException();
                        return value0;
                    }
                case 2:
                    {
                        int value0;
                        double value1;
                        if (!Int32.TryParse(input[0], out value0) || !Double.TryParse(input[1], out value1))
                            throw new ArgumentException();
                        return (value0) + Math.Sign(value0) * (value1 * 1 / 60d);
                    }
                case 3:
                    {
                        int value0, value1;
                        double value2;
                        if (!Int32.TryParse(input[0], out value0) || !Int32.TryParse(input[1], out value1) || !Double.TryParse(input[2], out value2))
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
                AddWaypoint(new Waypoint(name, lat, lon));
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
    }
}
