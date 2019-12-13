using System;
using System.IO;
using OxyPlot;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceGraphModel
    {
        

        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;
        internal int SiteNumber = 1;
        internal int RunCount = 100;
        internal ushort SpectrometerPortNumber = 11001;


        public PlotModel SpectrometerPlotModel;
        public PlotModel SensorPlotModel;
        public PlotModel MethanePlotModel;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";



    }
}
