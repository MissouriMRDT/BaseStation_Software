using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using RoverAttachmentManager.Models.Science;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Science
{
    public class SpectrometerViewModel : PropertyChangedBase
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private const int ScrewSpeedScale = 1000;
        private bool screwIncrementPressed = false;
        private const int XYSpeedScale = 1000;
        private readonly SpectrometerModel _model;


        public PlotModel SpectrometerPlotModel { set; private get; }
        public OxyPlot.Series.LineSeries SpectrometerSeries;

        public PlotModel SensorPlotModel { set; private get; }
        public PlotModel MethanePlotModel { set; private get; }

        public DateTime StartTime;
        public bool Graphing = false;
        public double[] SiteTimes = new double[12];


        public string SpectrometerFilePath
        {
            get
            {
                return _model.SpectrometerFilePath;
            }
            set
            {
                _model.SpectrometerFilePath = value;
                NotifyOfPropertyChange(() => SpectrometerFilePath);
            }
        }
        public ushort SpectrometerPortNumber
        {
            get
            {
                return _model.SpectrometerPortNumber;
            }
            set
            {
                _model.SpectrometerPortNumber = value;
                NotifyOfPropertyChange(() => SpectrometerPortNumber);
            }
        }
        public System.Net.IPAddress SpectrometerIPAddress
        {
            get
            {
                return _model.SpectrometerIPAddress;
            }
            set
            {
                _model.SpectrometerIPAddress = value;
                NotifyOfPropertyChange(() => SpectrometerIPAddress);
            }
        }
        public int RunCount
        {
            get
            {
                return _model.RunCount;
            }
            set
            {
                _model.RunCount = value;
                NotifyOfPropertyChange(() => RunCount);
            }
        }
        public SiteManagmentViewModel SiteManagment
        {
            get
            {
                return _model._siteManagment;
            }
            set
            {
                _model._siteManagment = value;
                NotifyOfPropertyChange(() => SiteManagment);
            }
        }

        public SpectrometerViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, ScienceViewModel parent)
        {
            _model = new SpectrometerModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            SiteManagment = parent.SiteManagment;

            SpectrometerPlotModel = new PlotModel { Title = "Spectrometer Results" };
            SpectrometerSeries = new OxyPlot.Series.LineSeries();
            SpectrometerPlotModel.Series.Add(SpectrometerSeries);
            SpectrometerPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, Title = "Intensity" });
            SpectrometerPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Bottom, Title = "Wavelength (nanometers)" });

        }



        public async void DownloadSpectrometer()
        {
            string filename = Path.Combine(SpectrometerFilePath, "REDSpectrometerData-" + DateTime.Now.ToString("yyyyMMdd'-'HHmmss") + ".csv");
            try
            {
                using (var client = new TcpClient())
                {
                    _log.Log("Connecting to Spectrometer...");
                    await client.ConnectAsync(SpectrometerIPAddress, SpectrometerPortNumber);
                    _log.Log("Spectrometer connection established");

                    // Request the data
                    _rovecomm.SendCommand(new Packet("RunSpectrometer", (byte)RunCount), true);

                    _log.Log("Awaiting data...");
                    using (var file = File.Create(filename))
                    {
                        await client.GetStream().CopyToAsync(file);
                    }
                }
                _log.Log($"Spectrometer data downloaded into {filename}");
                GraphSpectrometerData(filename);
            }
            catch (Exception e)
            {
                _log.Log("There was an error downloading the spectrometer data:{0}{1}", Environment.NewLine, e);
            }
        }


        public void GraphSpectrometerData(string filename)
        {
            SpectrometerSeries.Points.Clear();

            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    SpectrometerSeries.Points.Add(new DataPoint((Int16.Parse(values[0]) / 10.0) + 389, Double.Parse(values[1])));
                }
            }
            SpectrometerPlotModel.InvalidatePlot(true);
            ExportGraph(SpectrometerPlotModel, SpectrometerFilePath + "\\SpectrometerGraph-Site" + SiteManagment.SiteNumber + ".png", 400);
        }

        public void UpdateSensorGraphs()
        {
            if (!Graphing) { return; }

            TimeSpan nowSpan = DateTime.UtcNow.Subtract(StartTime);
            DateTime now = new DateTime(nowSpan.Ticks);

            SensorPlotModel.InvalidatePlot(true);
            MethanePlotModel.InvalidatePlot(true);

            ExportGraph(MethanePlotModel, SpectrometerFilePath + "\\methane.png", 400);
            ExportGraph(SensorPlotModel, SpectrometerFilePath + "\\temphum.png", 400);
        }

        public void ExportGraph(PlotModel model, string filename, int height)
        {
            var pngExporter = new PngExporter { Width = 600, Height = height, Background = OxyColors.White };
            pngExporter.ExportToFile(model, filename);
        }

        public void CreateSiteAnnotation()
        {
            SensorPlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = SiteTimes[SiteManagment.SiteNumber * 2],
                MaximumX = SiteTimes[(SiteManagment.SiteNumber * 2) + 1],
                Text = "Site " + SiteManagment.SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),

            });
            MethanePlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = SiteTimes[SiteManagment.SiteNumber * 2],
                MaximumX = SiteTimes[(SiteManagment.SiteNumber * 2) + 1],
                Text = "Site " + SiteManagment.SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),

            });
        }
        public void AddSiteAnnotation(double x, string text)
        {
            SensorPlotModel.Annotations.Add(new OxyPlot.Annotations.LineAnnotation { Type = LineAnnotationType.Vertical, X = x, Color = OxyColors.Green, Text = text });
            MethanePlotModel.Annotations.Add(new OxyPlot.Annotations.LineAnnotation { Type = LineAnnotationType.Vertical, X = x, Color = OxyColors.Green, Text = text });

        }

        public void ClearSensorGraphs()
        {
            SensorPlotModel.InvalidatePlot(true);
            MethanePlotModel.InvalidatePlot(true);
        }

        public void StartSensorGraphs()
        {
            StartTime = DateTime.UtcNow;
            Graphing = true;
            ClearSensorGraphs();
        }

    }
}
