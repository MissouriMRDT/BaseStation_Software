using Caliburn.Micro;
using Core;
using Core.Models;
using Core.ViewModels;
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

        public ObservableCollection<Waypoint> Waypoints
        {
            get
            {
                return _model.Manager.Waypoints;
            }
            private set
            {
                _model.Manager.Waypoints = value;
                NotifyOfPropertyChange(() => Waypoints);
            }
        }
        public Waypoint SelectedWaypoint
        {
            get
            {
                return _model.Manager.SelectedWaypoint;
            }
            set
            {
                _model.Manager.SelectedWaypoint = value;
                NotifyOfPropertyChange(() => SelectedWaypoint);
            }
        }

        public WaypointManager Manager
        {
            get
            {
                return _model.Manager;
            }
            set
            {
                _model.Manager = value;
                NotifyOfPropertyChange(() => Manager);
            }
        }

        public Waypoint NewPoint
        {
            get
            {
                return _model.NewPoint;
            }
            set
            {
                _model.NewPoint = value;
                NotifyOfPropertyChange(() => NewPoint);
            }
        }
        public String Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                _model.Name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public double Latitude
        {
            get
            {
                return _model.Latitude;
            }
            set
            {
                _model.Latitude = value;
                NotifyOfPropertyChange(() => Latitude);
            }
        }
        public double Longitude
        {
            get
            {
                return _model.Longitude;
            }
            set
            {
                _model.Longitude = value;
                NotifyOfPropertyChange(() => Longitude);
            }
        }
        public double LatitudeD
        {
            get
            {
                return _model.LatitudeD;
            }
            set
            {
                _model.LatitudeD = value;
                NotifyOfPropertyChange(() => LatitudeD);
            }
        }
        public double LatitudeM
        {
            get
            {
                return _model.LatitudeM;
            }
            set
            {
                _model.LatitudeM = value;
                NotifyOfPropertyChange(() => LatitudeM);
            }
        }
        public double LatitudeS
        {
            get
            {
                return _model.LatitudeS;
            }
            set
            {
                _model.LatitudeS = value;
                NotifyOfPropertyChange(() => LatitudeS);
            }
        }
        public double LongitudeD
        {
            get
            {
                return _model.LongitudeD;
            }
            set
            {
                _model.LongitudeD = value;
                NotifyOfPropertyChange(() => LongitudeD);
            }
        }
        public double LongitudeM
        {
            get
            {
                return _model.LongitudeM;
            }
            set
            {
                _model.LongitudeM = value;
                NotifyOfPropertyChange(() => LongitudeM);
            }
        }
        public double LongitudeS
        {
            get
            {
                return _model.LongitudeS;
            }
            set
            {
                _model.LongitudeS = value;
                NotifyOfPropertyChange(() => LongitudeS);
            }
        }
        public WaypointManagerViewModel(MapViewModel map, GPSViewModel gps)
        {
            _model = new WaypointManagerModel();
            Manager = WaypointManager.Instance;
            NewPoint = new Waypoint(0, 0);

            Map = map;
            GPSModule = gps;

            GPSModule.PropertyChanged += GPSModule_PropertyChanged;

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

        public bool AddWaypoint(string name, double latD, double latM, double latS, double longD, double longM, double longS)
        {
            try
            {
                AddWaypoint(new Waypoint(name, (latD + Math.Sign(latD) * ((latM * 1 / 60d) + (latS * 1 / 60d / 60d))), (longD + Math.Sign(longD) * ((longM* 1 / 60d) + (longS* 1 / 60d / 60d)))));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddWaypoint(string name, double latitude, double longitude)
        {
            try
            {
                AddWaypoint(new Waypoint(name, latitude, longitude));
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
            Map.RefreshMap();
        }

        public void RemoveSelectedWaypoint()
        {
            Waypoints.Remove(SelectedWaypoint);
            Map.RefreshMap();
        }

        public void CurrentLocationToWaypoint()
        {
            AddWaypoint(Map.CurrentLocation.Name, Map.CurrentLocation.Latitude, Map.CurrentLocation.Longitude);
        }
    }
}
