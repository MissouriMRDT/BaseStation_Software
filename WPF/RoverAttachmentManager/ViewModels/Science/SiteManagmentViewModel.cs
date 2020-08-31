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
                return _model.SiteNumber;
            }
            set
            {
                _model.SiteNumber = value;
                NotifyOfPropertyChange(() => SiteNumber);
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

        }

        public void ReachedSite()
        {
            double siteTime = OxyPlot.Axes.DateTimeAxis.ToDouble(GetTimeDiff());
            ScienceGraph.SiteTimes[SiteNumber * 2] = siteTime;
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
            ScienceGraph.SiteTimes[(SiteNumber * 2) + 1] = siteTime;

            double methaneAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor4Series, "Methane vs Time", "Methane (parts per billion)", 2000, ScienceGraph.SpectrometerFilePath + "\\Methane-Site" + SiteNumber + ".png");
            double tempAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor0Series, "Temperature vs Time", "Temperature (Celsius)", 50, ScienceGraph.SpectrometerFilePath + "\\Temperature-Site" + SiteNumber + ".png");
            double humidityAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor1Series, "Humidity vs Time", "Humidity (%)", 100, ScienceGraph.SpectrometerFilePath + "\\Humidity-Site" + SiteNumber + ".png");

            WriteSiteData(tempAvg, humidityAvg, methaneAvg);

            ScienceGraph.CreateSiteAnnotation();
            SiteNumber++;
        }

    }
}
