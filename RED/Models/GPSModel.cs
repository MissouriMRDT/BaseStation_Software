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
        internal GPSCoordinate currentLocation;
        internal float currentAltitude;
        internal bool fixObtained;
        internal ObservableCollection<GPSCoordinate> waypoints;
    }
}
