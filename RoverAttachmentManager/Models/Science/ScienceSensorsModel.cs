using System;
using System.IO;
using OxyPlot;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceSensorsModel
    {
        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;

        public PlotModel SensorPlotModel;
        public PlotModel MethanePlotModel;
    }
}
