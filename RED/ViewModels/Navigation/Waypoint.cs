using Caliburn.Micro;
using RED.Models.Navigation;
using System.Windows.Media;

namespace RED.ViewModels.Navigation
{
    public class Waypoint : PropertyChangedBase
    {
        private readonly WaypointModel _model;

        public string Name
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
        public Color Color
        {
            get
            {
                return _model.Color;
            }
            set
            {
                _model.Color = value;
                NotifyOfPropertyChange(() => Color);
            }
        }
        public bool IsOnMap
        {
            get
            {
                return _model.IsOnMap;
            }
            set
            {
                _model.IsOnMap = value;
                NotifyOfPropertyChange(() => IsOnMap);
            }
        }

        public Waypoint(string name, double latitude, double longitude)
        {
            _model = new WaypointModel();
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Color = Colors.Black;
            IsOnMap = true;
        }
        public Waypoint(double latitude, double longitude)
            : this(System.String.Empty, latitude, longitude)
        { }
    }
}
