using RED.Addons;

namespace RED.Models.Modules
{
    internal class GPSModel
    {
        internal bool fixObtained = false;
        internal byte fixQuality = 255;
        internal byte numberOfSatellites = 255;
        internal GPSCoordinate currentLocation = new GPSCoordinate(0, 0);
        internal float currentAltitude = -1;
        internal float speed = -1;
        internal float speedAngle = -1;
        internal GPSCoordinate baseStationLocation = new GPSCoordinate(0, 0);
        internal double antennaDirectionDeg = 0;
        internal float heading = 0;
    }
}
