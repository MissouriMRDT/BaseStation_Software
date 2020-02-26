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
using System.Net;
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

        public float SpectrometerConcentration
        {
            get
            {
                return _model.SpectrometerConcentration;
            }
            set
            {
                _model.SpectrometerConcentration = value;
                NotifyOfPropertyChange(() => SpectrometerConcentration);
            }
        }

        public float MPPCConcentration
        {
            get
            {
                return _model.MPPCConcentration;
            }
            set
            {
                _model.MPPCConcentration = value;
                NotifyOfPropertyChange(() => MPPCConcentration);
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

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "Spectrometer":
                    SpectrometerConcentration = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)));
                    UpdateGraphs();
                    break;

                case "MPPC":
                    MPPCConcentration = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)));
                    UpdateGraphs();
                    break;

                default:
                    break;
            }
        }


        public void UpdateGraphs()
        {
            if (!Graphing) { return; }

            TimeSpan nowSpan = DateTime.UtcNow.Subtract(StartTime);
            DateTime now = new DateTime(nowSpan.Ticks);

            SpectrometerSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), SpectrometerConcentration));
            MPPCSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), MPPCConcentration));
            SelectedPlots.InvalidatePlot(true);
        }
        public void StartGraphs()
        {
            StartTime = DateTime.UtcNow;
            Graphing = true;
            ClearGraphs();
        }
        public void ClearGraphs()
        {
            SpectrometerSeries.Points.Clear();
            MPPCSeries.Points.Clear();
        }
        public void ExportGraph(PlotModel model, string filename, int height)
        {
            var pngExporter = new PngExporter { Width = 600, Height = height, Background = OxyColors.White };
            pngExporter.ExportToFile(model, filename);
        }






        public void SetUVLed(byte val)
        {
            if (SelectedPlots == SpectrometerPlotModel)
            {
                _rovecomm.SendCommand(Packet.Create("UVLedControl", val));
            }
            else
            {
                _rovecomm.SendCommand(Packet.Create("ScienceLight", val));
            }
        }

    }
}
