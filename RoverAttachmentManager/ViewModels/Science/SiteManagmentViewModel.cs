using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using RoverAttachmentManager.Models.Science;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RoverAttachmentManager.ViewModels.Science
{
    public class SiteManagmentViewModel : PropertyChangedBase
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private readonly SiteManagmentModel _model;

        

        public ScienceGraphViewModel ScienceGraph
        {
            get
            {
                return _model._scienceGraph;
            }
            set
            {
                _model._scienceGraph = value;
                NotifyOfPropertyChange(() => ScienceGraph);
            }
        }

        public int SiteNumber
        {
            get
            {
                return _model._science.SiteNumber;
            }
            set
            {
                _model._science.SiteNumber = value;
                NotifyOfPropertyChange(() => SiteNumber);
            }
        }

        public ScienceViewModel Science
        {
            get
            {
                return _model._science;
            }
            set
            {
                _model._science = value;
                NotifyOfPropertyChange(() => Science);
            }
        }

        private DateTime GetTimeDiff()
        {
            TimeSpan nowSpan = DateTime.UtcNow.Subtract(ScienceGraph.StartTime);
            return new DateTime(nowSpan.Ticks);
        }

        public SiteManagmentViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, ScienceViewModel parent)
        {
            _model = new SiteManagmentModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            ScienceGraph = parent.ScienceGraph;
            Science = parent;

        }

        public void ReachedSite()
        {

            if (SiteNumber == 6)
            {
                SiteNumber = 1;
            }
            else
            {
                SiteNumber++;
            }

            double siteTime = OxyPlot.Axes.DateTimeAxis.ToDouble(GetTimeDiff());
            ScienceGraph.SiteTimes[(SiteNumber - 1) * 2] = siteTime;


        }

        private async void WriteSiteData(double temp, double humidity, double methane)
        {
            FileStream file = new FileStream(ScienceGraph.SpectrometerFilePath + "\\REDSensorData-Site" + SiteNumber + ".csv", FileMode.Create);
            if (!file.CanWrite) return;

            var data = Encoding.UTF8.GetBytes(String.Format("Temperature, {0}, Humidity, {1}, Methane, {2}{3}", temp, humidity, methane, Environment.NewLine));
            await file.WriteAsync(data, 0, data.Length);

            if (file.CanWrite)
            {
                file.Close();
            }

        }

        public void LeftSite()
        {
            double siteTime = OxyPlot.Axes.DateTimeAxis.ToDouble(GetTimeDiff());
            ScienceGraph.SiteTimes[((SiteNumber-1) * 2) + 1] = siteTime;

            double methaneAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.MethaneConcentrationSeries, "Methane vs Time", "Methane (parts per million)", 50, ScienceGraph.SpectrometerFilePath + "\\Methane-Site" + SiteNumber + ".png");
            double CO2Avg = ScienceGraph.AverageValueForSeries(ScienceGraph.CO2ConcentrationSeries, "CO2 vs Time", "CO2 (parts per million)", 100, ScienceGraph.SpectrometerFilePath + "\\CO2-Site" + SiteNumber + ".png");
            double O2Avg = ScienceGraph.AverageValueForSeries(ScienceGraph.O2ConcentrationSeries, "O2 vs Time", "O2 (parts per million)", 2000, ScienceGraph.SpectrometerFilePath + "\\O2-Site" + SiteNumber + ".png");

            WriteSiteData(methaneAvg, CO2Avg, O2Avg);

            ScienceGraph.CreateSiteAnnotation();
            
        }

    }
}
