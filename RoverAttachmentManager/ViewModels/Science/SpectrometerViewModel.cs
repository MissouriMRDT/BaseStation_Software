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
using System.Collections.ObjectModel;
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
        private const int XYSpeedScale = 1000;
        private readonly SpectrometerModel _model;


        public PlotModel SpectrometerPlotModel { set; private get; }
        public OxyPlot.Series.LineSeries SpectrometerSeries;

        public PlotModel MPPCPlotModel { set; private get; }
        public OxyPlot.Series.LineSeries MPPCSeries;

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




        public string MPPCFilePath
        {
            get
            {
                return _model.MPPCFilePath;
            }
            set
            {
                _model.MPPCFilePath = value;
                NotifyOfPropertyChange(() => MPPCFilePath);
            }
        }
        public ushort MPPCPortNumber
        {
            get
            {
                return _model.MPPCPortNumber;
            }
            set
            {
                _model.MPPCPortNumber = value;
                NotifyOfPropertyChange(() => MPPCPortNumber);
            }
        }
        public System.Net.IPAddress MPPCIPAddress
        {
            get
            {
                return _model.MPPCIPAddress;
            }
            set
            {
                _model.MPPCIPAddress = value;
                NotifyOfPropertyChange(() => MPPCIPAddress);
            }
        }

        public ObservableCollection<PlotModel> Plots
        {
            get
            {
                return _model.Plots;

            }
            set
            {
                _model.Plots = value;
                NotifyOfPropertyChange(() => Plots);
            }
        }

        public PlotModel SelectedPlots
        {
            get
            {
                return _model.SelectedPlot;
            }
            set
            {
                _model.SelectedPlot = value;
                NotifyOfPropertyChange(() => SelectedPlots);
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

            MPPCPlotModel = new PlotModel { Title = "MPPC Results" };
            MPPCSeries = new OxyPlot.Series.LineSeries();
            MPPCPlotModel.Series.Add(MPPCSeries);
            MPPCPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, Title = "Photons" });
            MPPCPlotModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "mm:ss", Title = "Time" });

            Plots = new ObservableCollection<PlotModel>() { SpectrometerPlotModel, MPPCPlotModel };
            SelectedPlots = SpectrometerPlotModel;

        }

        public async void DownloadSpectrometer()
        {
            //this likely will change a lot with official tcp implementations
            string filename = Path.Combine(SpectrometerFilePath, "REDSpectrometerData-" + DateTime.Now.ToString("yyyyMMdd'-'HHmmss") + ".csv");
            try
            {
                using (var client = new TcpClient())
                {
                    _log.Log("Connecting to Spectrometer...");
                    await client.ConnectAsync(SpectrometerIPAddress, SpectrometerPortNumber);
                    _log.Log("Spectrometer connection established");

                    // Request the data
                    _rovecomm.SendCommand(Packet.Create("RunSpectrometer", (byte)RunCount), true);

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








        public async void DownloadMPPC()
        {
            //this likely will change a lot with official tcp implementations
            string filename = Path.Combine(MPPCFilePath, "REDSMPPCData-" + DateTime.Now.ToString("yyyyMMdd'-'HHmmss") + ".csv");
            try
            {
                using (var client = new TcpClient())
                {
                    _log.Log("Connecting to MPPC...");
                    await client.ConnectAsync(MPPCIPAddress, MPPCPortNumber);
                    _log.Log("Spectrometer connection established");

                    // Request the data
                    _rovecomm.SendCommand(Packet.Create("RunMPPC", (byte)RunCount), true);

                    _log.Log("Awaiting data...");
                    using (var file = File.Create(filename))
                    {
                        await client.GetStream().CopyToAsync(file);
                    }
                }
                _log.Log($"MPPC data downloaded into {filename}");
                GraphMPPCData(filename);
            }
            catch (Exception e)
            {
                _log.Log("There was an error downloading the MPPC data:{0}{1}", Environment.NewLine, e);
            }
        }


        public void GraphMPPCData(string filename)
        {
            MPPCSeries.Points.Clear();

            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    MPPCSeries.Points.Add(new DataPoint((Int16.Parse(values[0]) / 10.0) + 389, Double.Parse(values[1])));
                }
            }
            MPPCPlotModel.InvalidatePlot(true);
            ExportGraph(MPPCPlotModel, MPPCFilePath + "\\MPPCGraph-Site" + SiteManagment.SiteNumber + ".png", 400);
        }





        public void SetUVLed(byte val)
        {
            if (SelectedPlots == SpectrometerPlotModel)
            {
                _rovecomm.SendCommand(Packet.Create("UVLedControl", val));
            }
            else
            {
                _rovecomm.SendCommand(Packet.Create("", val));
            }
        }

        public void ExportGraph(PlotModel model, string filename, int height)
        {
            var pngExporter = new PngExporter { Width = 600, Height = height, Background = OxyColors.White };
            pngExporter.ExportToFile(model, filename);
        }
    }
}
