using System;
using System.IO;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceModel
    {
        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;
        internal int ScrewPosition;
        internal int RunCount = 100;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal ushort SpectrometerPortNumber = 11001;
        internal string SpectrometerFilePath = Environment.CurrentDirectory;

        internal Stream SensorDataFile;
    }
}