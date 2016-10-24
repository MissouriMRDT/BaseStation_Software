using System;
using System.IO;

namespace RED.Models.Modules
{
    internal class ScienceModel
    {
        internal float Temperature1Value;
        internal float Temperature2Value;
        internal float Temperature3Value;
        internal float Temperature4Value;
        internal float Moisture1Value;
        internal float Moisture2Value;
        internal float Moisture3Value;
        internal float Moisture4Value;

        internal System.Net.IPAddress CCDIPAddress;
        internal ushort CCDPortNumber;
        internal string CCDFilePath = Environment.CurrentDirectory;

        internal Stream SensorDataFile;
    }
}