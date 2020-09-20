using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using RoverAttachmentManager.ViewModels.Science;

namespace RoverAttachmentManager.Models.Science
{
    internal class SpectrometerModel
    {
        internal int SiteNumber = 1;
        internal int RunCount = 100;
        internal ushort SpectrometerPortNumber = 11001;
        internal ushort MPPCPortNumber;

        internal SiteManagmentViewModel _siteManagment;

        internal ObservableCollection<PlotModel> Plots;
        internal PlotModel SelectedPlot;

        public PlotModel SpectrometerPlotModel;
        public PlotModel MPPCPlotModel;
        internal System.Net.IPAddress SpectrometerIPAddress;
        internal System.Net.IPAddress MPPCIPAddress;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";
        internal string MPPCFilePath;
        internal float SpectrometerConcentration;
        internal float MPPCConcentration;
    }
}
