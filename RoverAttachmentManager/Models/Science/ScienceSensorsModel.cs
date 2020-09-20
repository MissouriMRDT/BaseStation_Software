using System;
using System.IO;
using OxyPlot;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceSensorsModel
    {
        internal ScienceGraphViewModel _scienceGraph;
        internal float MethaneConcentration;
        internal float MethaneTemperature;
        internal float CO2Concentration;
        internal float O2PartialPressure;
        internal float O2Temperature;
        internal float O2Concentration;
        internal float O2BarometricPressure;

        internal Stream SensorDataFile;
    }
}
