using System;
using System.IO;

namespace RED.Models.Modules
{
    internal class ScienceModel
    {
        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;
        internal float Sensor5Value;
        internal float Sensor6Value;
        internal float Sensor7Value;
        internal float Sensor8Value;
        internal float Sensor9Value;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal ushort SpectrometerPortNumber;
        internal string SpectrometerFilePath = Environment.CurrentDirectory;

        internal Stream SensorDataFile;
    }
}