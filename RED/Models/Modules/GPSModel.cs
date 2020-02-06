using HelixToolkit.Wpf;
using RED.Addons.Navigation;
using System.Windows.Media.Media3D;

namespace RED.Models.Modules
{
    internal class GPSModel
    {
        internal bool FixObtained = false;
        internal byte FixQuality = 255;
        internal byte NumberOfSatellites = 255;
        internal GPSCoordinate CurrentLocation = new GPSCoordinate(0, 0);
        internal GPSCoordinate RawLocation = new GPSCoordinate(0, 0);
        internal GPSCoordinate Offset = new GPSCoordinate(0, 0);
        internal GPSCoordinate BaseStationLocation = new GPSCoordinate(0, 0);
        internal double AntennaDirectionDeg = 0;
        internal float Heading = 0;



    }
}
