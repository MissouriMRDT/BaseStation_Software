using Caliburn.Micro;
using System.Windows.Media;

namespace RED.ViewModels.Navigation
{
    public class Waypoint : PropertyChangedBase
    {
        private string _name;
        private double _longitude;
        private double _latitude;
        private Color _color;
        private bool _isOnMap;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                NotifyOfPropertyChange(() => Longitude);
            }
        }
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                NotifyOfPropertyChange(() => Latitude);
            }
        }
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                NotifyOfPropertyChange(() => Color);
            }
        }
        public bool IsOnMap
        {
            get
            {
                return _isOnMap;
            }
            set
            {
                _isOnMap = value;
                NotifyOfPropertyChange(() => IsOnMap);
            }
        }

        public Waypoint(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Color = Colors.Black;
            IsOnMap = true;
        }
        public Waypoint(double latitude, double longitude)
            : this("", latitude, longitude)
        { }
    }
}
