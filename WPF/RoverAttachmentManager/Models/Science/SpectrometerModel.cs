using System;
using System.Collections.Generic;
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

        internal SiteManagmentViewModel _siteManagment;

        public PlotModel SpectrometerPlotModel;
        internal System.Net.IPAddress SpectrometerIPAddress;
        internal string SpectrometerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Science Files";
    }
}
