using RED.Addons;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Models
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

        internal ObservableCollection<GPSCoordinate> waypoints = new ObservableCollection<GPSCoordinate>();
    }
}
