using System;
using System.IO;
using OxyPlot;
using RoverAttachmentManager.ViewModels.Science;
using System.Collections.ObjectModel;
using Core.Interfaces;

namespace RoverAttachmentManager.Models.Science
{
    internal class ScienceGraphModel
    {
        internal SiteManagmentViewModel _siteManagment;
        internal IInputMode Mode;

        internal float Sensor0Value;
        internal float Sensor1Value;
        internal float Sensor2Value;
        internal float Sensor3Value;
        internal float Sensor4Value;
        internal int RunCount = 100;
        internal ushort SpectrometerPortNumber = 11001;

        internal ObservableCollection<PlotModel> Plots;
        internal PlotModel SelectedPlot;

        public PlotModel SpectrometerPlotModel;
        public PlotModel SensorPlotModel;
        public PlotModel MethanePlotModel;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";



    }
}
