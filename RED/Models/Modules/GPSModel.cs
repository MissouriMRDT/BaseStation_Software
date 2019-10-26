using RED.Addons.Navigation;

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
        internal float RoverDistanceSession = 0;
        internal float RoverDistanceStart = 0;
        internal float RoverDistanceTraveled = 0;

    }
}
