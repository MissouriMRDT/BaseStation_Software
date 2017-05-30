using Caliburn.Micro;
using RED.Addons;
using RED.Models.Navigation;
using System.Collections.ObjectModel;

namespace RED.ViewModels.Navigation
{
    public class MapViewModel : PropertyChangedBase
    {
        MapModel _model;

        public GPSCoordinate CurrentLocation
        {
            get
            {
                return _model.currentLocation;
            }
            set
            {
                _model.currentLocation = value;
                NotifyOfPropertyChange(() => CurrentLocation);
            }
        }
        public ObservableCollection<GPSCoordinate> Waypoints
        {
            get
            {
                return _model.waypoints;
            }
            set
            {
                _model.waypoints = value;
                NotifyOfPropertyChange(() => Waypoints);
            }
        }

        public MapViewModel()
        {
            _model = new MapModel();
            Waypoints.Add(new GPSCoordinate(37.951631, -91.777713)); //Rolla
            Waypoints.Add(new GPSCoordinate(37.850025, -91.701845)); //Fugitive Beach
            Waypoints.Add(new GPSCoordinate(38.406426, -110.791919)); //Mars Desert Research Station
        }
    }
}
