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
    }
}
