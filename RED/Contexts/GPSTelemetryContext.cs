using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.Contexts
{
    public struct GPSTelemetryContext
    {
        bool Fix;
        int Longitude;
        int Latitude;
        float Altitude;
        float Speed;
        float Angle;
        byte FixQuality;
        byte NumberOfSatellites;
    }
}