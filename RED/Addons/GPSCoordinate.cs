using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Addons
{
    public struct GPSCoordinate
    {
        public double Latitude;
        public double Longitude;

        public GPSCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
