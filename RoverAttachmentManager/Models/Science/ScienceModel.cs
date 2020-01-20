using System;
using System.IO;
using RoverAttachmentManager.ViewModels.Science;


namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceModel
    {
        internal ScienceGraphViewModel _scienceGraph;
        internal SiteManagmentViewModel _siteManagment;
        internal ScienceSensorViewModel _scienceSensor;
        internal int ScrewPosition;
        internal ScienceActuationViewModel _scienceActuation;
        internal SpectrometerViewModel _spectrometer;
        internal int RunCount = 100;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal ushort SpectrometerPortNumber = 11001;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";

        internal Stream SensorDataFile;
    }
}