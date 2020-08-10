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

        internal float MethaneConcentration;
        internal float MethaneTemperature;
        internal float CO2Concentration;
        internal float O2PartialPressure;
        internal float O2Temperature;
        internal float O2Concentration;
        internal float O2BarometricPressure;

        internal DateTime StartTime;

        internal int RunCount = 100;
        internal ushort SpectrometerPortNumber = 11001;

        internal ObservableCollection<PlotModel> Plots;
        internal PlotModel SelectedPlot;

        internal System.Net.IPAddress SpectrometerIPAddress;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";



    }
}
