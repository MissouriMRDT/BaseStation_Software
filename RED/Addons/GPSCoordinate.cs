using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Addons
{
    public struct GPSCoordinate
    {
        float Longitude;
        float Latitude;

        public GPSCoordinate(float longitude, float latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
