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
        internal bool fixObtained;
        internal byte fixQuality;
        internal byte numberOfSatellites;
        internal GPSCoordinate currentLocation;
        internal float currentAltitude;
        internal float speed;
        internal float speedAngle;
        internal GPSCoordinate baseStationLocation;

        internal ObservableCollection<GPSCoordinate> waypoints=new ObservableCollection<GPSCoordinate>();
    }
}
