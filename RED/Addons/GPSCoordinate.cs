using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Addons
{
    public struct GPSCoordinate
    {
        public double Longitude;
        public double Latitude;

        public GPSCoordinate(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
