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
using System.Net;
using System.Net.Sockets;


namespace RoverAttachmentManager.ViewModels.Science
{
    public class ScienceGraphViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private const int ScrewSpeedScale = 1000;
        private bool screwIncrementPressed = false;


        private const int XYSpeedScale = 1000;

        private readonly ScienceGraphModel _model;


        public PlotModel SpectrometerPlotModel { set; private get; }
        public PlotModel SensorPlotModel { set; private get; }
        public PlotModel MethanePlotModel { set; private get; }
        public OxyPlot.Series.LineSeries SpectrometerSeries;
        public OxyPlot.Series.LineSeries Sensor0Series;
        public OxyPlot.Series.LineSeries Sensor1Series;
        public OxyPlot.Series.LineSeries Sensor4Series;

        public DateTime StartTime;
        public bool Graphing = false;

        public double[] SiteTimes = new double[12];

        public float Sensor0Value
        {
            get
            {
                return _model.Sensor0Value;
            }
            set
            {
                _model.Sensor0Value = value;
                NotifyOfPropertyChange(() => Sensor0Value);
            }
        }
        public float Sensor1Value
        {
            get
            {
                return _model.Sensor1Value;
            }
            set
            {
                _model.Sensor1Value = value;
                NotifyOfPropertyChange(() => Sensor1Value);
            }
        }
        public float Sensor2Value
        {
            get
            {
                return _model.Sensor2Value;
            }
            set
            {
                _model.Sensor2Value = value;
                NotifyOfPropertyChange(() => Sensor2Value);
            }
        }
        public float Sensor3Value
        {
            get
            {
                return _model.Sensor3Value;
            }
            set
            {
                _model.Sensor3Value = value;
                NotifyOfPropertyChange(() => Sensor3Value);
            }
        }
        public float Sensor4Value
        {
            get
            {
                return _model.Sensor4Value;
            }
            set
            {
                _model.Sensor4Value = value;
                NotifyOfPropertyChange(() => Sensor4Value);
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
 
        public ScienceGraphViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceGraphModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            _rovecomm.NotifyWhenMessageReceived(this, "ScienceSensors");

            SpectrometerPlotModel = new PlotModel { Title = "Spectrometer Results" };
            SpectrometerSeries = new OxyPlot.Series.LineSeries();
            SpectrometerPlotModel.Series.Add(SpectrometerSeries);
            SpectrometerPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, Title = "Intensity" });
            SpectrometerPlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Bottom, Title = "Wavelength (nanometers)" });

            MethanePlotModel = new PlotModel { Title = "Methane Data" };
            Sensor4Series = new OxyPlot.Series.LineSeries();
            MethanePlotModel.Series.Add(Sensor4Series);
            MethanePlotModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "mm:ss" });

            SensorPlotModel = new PlotModel { Title = "Temperature & Humidity Data" };
            Sensor0Series = new OxyPlot.Series.LineSeries();
            Sensor1Series = new OxyPlot.Series.LineSeries();
            SensorPlotModel.Series.Add(Sensor0Series);
            SensorPlotModel.Series.Add(Sensor1Series);
            SensorPlotModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "mm:ss" });

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

        public void UpdateSensorGraphs()
        {
            if (!Graphing) { return; }

            TimeSpan nowSpan = DateTime.UtcNow.Subtract(StartTime);
            DateTime now = new DateTime(nowSpan.Ticks);

            Sensor0Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor0Value));
            Sensor1Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor1Value));
            Sensor4Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor4Value));
            SensorPlotModel.InvalidatePlot(true);
            MethanePlotModel.InvalidatePlot(true);


        }


        public void AddSiteAnnotation(double x, string text)
        {
            SensorPlotModel.Annotations.Add(new OxyPlot.Annotations.LineAnnotation { Type = LineAnnotationType.Vertical, X = x, Color = OxyColors.Green, Text = text });
            MethanePlotModel.Annotations.Add(new OxyPlot.Annotations.LineAnnotation { Type = LineAnnotationType.Vertical, X = x, Color = OxyColors.Green, Text = text });

        }


        public void StartSensorGraphs()
        {
            StartTime = DateTime.UtcNow;
            Graphing = true;
            ClearSensorGraphs();
        }


        public void ClearSensorGraphs()
        {
            Sensor0Series.Points.Clear();
            Sensor1Series.Points.Clear();
            Sensor4Series.Points.Clear();
        }

        public void ExportGraph(PlotModel model, string filename, int height)
        {
            var pngExporter = new PngExporter { Width = 600, Height = height, Background = OxyColors.White };
            pngExporter.ExportToFile(model, filename);
        }


        public double AverageValueForSeries(OxyPlot.Series.LineSeries series, string plotTitle, string unitsTitle, int plotMax, string filename)
        {
            List<DataPoint> points = series.Points;

            Predicate<DataPoint> isInRange = dataPoint => dataPoint.X >= SiteTimes[SiteManagment.SiteNumber * 2] && dataPoint.X <= SiteTimes[(SiteManagment.SiteNumber * 2) + 1];

            points = points.FindAll(isInRange);

            PlotModel tempModel = new PlotModel { Title = plotTitle };
            OxyPlot.Series.LineSeries tempSeries = new OxyPlot.Series.LineSeries();
            tempModel.Series.Add(tempSeries);
            tempModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "mm:ss", Title = "Time since task start (minutes:seconds)" });
            tempModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, Title = unitsTitle, Minimum = 0, Maximum = plotMax });

            tempSeries.Points.AddRange(points);

            ExportGraph(tempModel, filename, 300);

            double tally = 0;
            foreach (DataPoint dataPoint in points)
            {
                tally += dataPoint.Y;
            }

            return tally / points.Count;
        }

        private DateTime GetTimeDiff()
        {
            TimeSpan nowSpan = DateTime.UtcNow.Subtract(StartTime);
            return new DateTime(nowSpan.Ticks);
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "ScienceSensors":
                    Sensor0Value = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)) / 100.0);
                    Sensor1Value = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2)) / 100.0);
                    Sensor2Value = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4)) / 100.0);
                    Sensor3Value = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 6)) / 100.0);
                    Sensor4Value = (float)(IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 8)));

                    UpdateSensorGraphs();
                    break;

                default:
                    break;

            }
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }

        public void StartMode() { }

        public void SetValues(Dictionary<string, float> values)
        {
            if ((values["ScrewPosUp"] == 1 || values["ScrewPosDown"] == 1) && !screwIncrementPressed)
            {
                byte screwPosIncrement = (byte)(values["ScrewPosUp"] == 1 ? 1 : values["ScrewPosDown"] == 1 ? -1 : 0);
                _rovecomm.SendCommand(new Packet("ScrewRelativeSetPosition", screwPosIncrement));
                screwIncrementPressed = true;
            }
            else if (values["ScrewPosUp"] == 0 && values["ScrewPosDown"] == 0)
            {
                screwIncrementPressed = false;
            }

            Int16[] screwValue = { (Int16)(values["Screw"] * ScrewSpeedScale) }; //order before we reverse
            byte[] data = new byte[screwValue.Length * sizeof(Int16)];
            Buffer.BlockCopy(screwValue, 0, data, 0, data.Length);
            Array.Reverse(data);
            _rovecomm.SendCommand(new Packet("Screw", data, 1, (byte)DataTypes.INT16_T));

            Int16 xMovement = (Int16)(values["XActuation"] * XYSpeedScale);
            Int16 yMovement = (Int16)(values["YActuation"] * XYSpeedScale);

            Int16[] sendValues = { yMovement, xMovement }; //order before we reverse
            data = new byte[sendValues.Length * sizeof(Int16)];
            Buffer.BlockCopy(sendValues, 0, data, 0, data.Length);
            Array.Reverse(data);
            _rovecomm.SendCommand(new Packet("XYActuation", data, 2, (byte)DataTypes.INT16_T));

        }


        public void StopMode()
        {
            _rovecomm.SendCommand(new Packet("Screw", (Int16)(0)), true);

            Int16[] sendValues = { 0, 0 };
            byte[] data = new byte[sendValues.Length * sizeof(Int16)];
            Buffer.BlockCopy(sendValues, 0, data, 0, data.Length);
            Array.Reverse(data);
            _rovecomm.SendCommand(new Packet("XYActuation", data, 2, (byte)DataTypes.INT16_T));
        }

    }
}
