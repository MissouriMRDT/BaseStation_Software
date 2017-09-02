using RED.Addons.Navigation;

namespace RED.Models.Modules
{
    internal class GPSModel
    {
        internal bool FixObtained = false;
        internal byte FixQuality = 255;
        internal byte NumberOfSatellites = 255;
        internal GPSCoordinate CurrentLocation = new GPSCoordinate(0, 0);
        internal float CurrentAltitude = -1;
        internal float Speed = -1;
        internal float SpeedAngle = -1;
        internal GPSCoordinate BaseStationLocation = new GPSCoordinate(0, 0);
        internal double AntennaDirectionDeg = 0;
        internal float Heading = 0;
    }
}
