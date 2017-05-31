using System.Windows.Media;

namespace RED.ViewModels.Navigation
{
    public class Waypoint
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Color Color { get; set; }
        public bool IsOnMap { get; set; }

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
