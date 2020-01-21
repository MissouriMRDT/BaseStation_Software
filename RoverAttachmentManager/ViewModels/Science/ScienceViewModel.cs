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
    public class ScienceViewModel : PropertyChangedBase, IRovecommReceiver, IInputMode
    {

        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private const int ScrewSpeedScale = 1000;
        private const int XYSpeedScale = 1000;
        private bool screwIncrementPressed = false;

        public string Name { get; }
        public string ModeType { get; }

        private readonly ScienceModel _model;
        
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

        public int ScrewPosition
        {
            get
            {
                return _model.ScrewPosition;
            }
            set
            {
                _model.ScrewPosition = value;
                NotifyOfPropertyChange(() => ScrewPosition);
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

        public Stream SensorDataFile
        {
            get
            {
                return _model.SensorDataFile;
            }
            set
            {
                _model.SensorDataFile = value;
                NotifyOfPropertyChange(() => SensorDataFile);
            }
        }

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

        public ScienceViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Controls";
            ModeType = "ScienceControls";

            SpectrometerIPAddress = IPAddress.Parse("192.168.1.138");

            _rovecomm.NotifyWhenMessageReceived(this, "ScienceSensors");
            _rovecomm.NotifyWhenMessageReceived(this, "ScrewAtPos");

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
     
            Int16[] sendValues = {yMovement, xMovement }; //order before we reverse
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
            ExportGraph(SpectrometerPlotModel, SpectrometerFilePath + "\\SpectrometerGraph-Site" + SiteNumber + ".png", 400);
        }

        public void SetScrewPosition(byte index)
        {
            _rovecomm.SendCommand(new Packet("ScrewAbsoluteSetPosition", index));
        }

        public void SaveFileStart()
        {
            SensorDataFile = new FileStream(SpectrometerFilePath + "\\REDSensorData-" + DateTime.Now.ToString("yyyyMMdd'-'HHmmss") + ".csv", FileMode.Create);
        }
        public void SaveFileStop()
        {
            if (SensorDataFile.CanWrite)
                SensorDataFile.Close();
        }

        private async void SaveFileWrite(string sensorName, object value)
        {
            if (SensorDataFile == null || !SensorDataFile.CanWrite) return;
            var data = Encoding.UTF8.GetBytes(String.Format("{0:s}, {1}, {2}{3}", DateTime.Now, sensorName, value.ToString(), Environment.NewLine));
            await SensorDataFile.WriteAsync(data, 0, data.Length);
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

            ExportGraph(MethanePlotModel, SpectrometerFilePath + "\\methane.png", 400);
            ExportGraph(SensorPlotModel, SpectrometerFilePath + "\\temphum.png", 400);
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

            SensorPlotModel.InvalidatePlot(true);
            MethanePlotModel.InvalidatePlot(true);
        }

        public void ExportGraph(PlotModel model,string filename, int height)
        {
            var pngExporter = new PngExporter { Width = 600, Height = height, Background = OxyColors.White };
            pngExporter.ExportToFile(model, filename);
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

                    SaveFileWrite("Air Temperature", Sensor0Value);
                    SaveFileWrite("Air Humidity", Sensor1Value);
                    SaveFileWrite("Air Methane", Sensor4Value);

                    UpdateSensorGraphs();
                    break;

                case "ScrewAtPos":
                    ScrewPosition = packet.Data[0];
                    break;
                default:
                    break;
            }
        }

		public void ReceivedRovecommMessageCallback(int index, bool reliable) {
			ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
		}

        public void SetUVLed(byte val)
        {
            _rovecomm.SendCommand(new Packet("UVLedControl", val));
        }

        public void CenterX()
        {
            _rovecomm.SendCommand(new Packet("CenterX"));
        }

        public void CreateSiteAnnotation()
        {
            SensorPlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = SiteTimes[SiteNumber * 2],
                MaximumX = SiteTimes[(SiteNumber * 2) + 1],
                Text = "Site " + SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),

            });
            MethanePlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = SiteTimes[SiteNumber * 2],
                MaximumX = SiteTimes[(SiteNumber * 2) + 1],
                Text = "Site " + SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),
                
            });
        }

        public void ReachedSite()
        {
            double siteTime = OxyPlot.Axes.DateTimeAxis.ToDouble(GetTimeDiff());
            SiteTimes[SiteNumber * 2] = siteTime;
        }

        private async void WriteSiteData(double temp, double humidity, double methane)
        {
            FileStream file = new FileStream(SpectrometerFilePath + "\\REDSensorData-Site" + SiteNumber + ".csv", FileMode.Create);
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
            SiteTimes[(SiteNumber * 2) + 1] = siteTime;

            double methaneAvg = AverageValueForSeries(Sensor4Series, "Methane vs Time", "Methane (parts per billion)", 2000, SpectrometerFilePath + "\\Methane-Site" + SiteNumber + ".png");
            double tempAvg = AverageValueForSeries(Sensor0Series, "Temperature vs Time", "Temperature (Celsius)", 50, SpectrometerFilePath + "\\Temperature-Site" + SiteNumber + ".png");
            double humidityAvg = AverageValueForSeries(Sensor1Series, "Humidity vs Time", "Humidity (%)", 100, SpectrometerFilePath + "\\Humidity-Site" + SiteNumber + ".png");

            WriteSiteData(tempAvg, humidityAvg, methaneAvg);
            
            CreateSiteAnnotation();
            SiteNumber++;
        }

        public double AverageValueForSeries(OxyPlot.Series.LineSeries series, string plotTitle, string unitsTitle, int plotMax, string filename)
        {
            List<DataPoint> points = series.Points;

            Predicate<DataPoint> isInRange = dataPoint => dataPoint.X >= SiteTimes[SiteNumber * 2] && dataPoint.X <= SiteTimes[(SiteNumber * 2) + 1];

            points = points.FindAll(isInRange);

            PlotModel tempModel = new PlotModel { Title = plotTitle };
            OxyPlot.Series.LineSeries tempSeries = new OxyPlot.Series.LineSeries();
            tempModel.Series.Add(tempSeries);
            tempModel.Axes.Add(new OxyPlot.Axes.DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "mm:ss", Title = "Time since task start (minutes:seconds)" });
            tempModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, Title = unitsTitle, Minimum = 0, Maximum = plotMax });

            tempSeries.Points.AddRange(points);

            ExportGraph(tempModel, filename, 300);

            double tally = 0;
            foreach(DataPoint dataPoint in points)
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

        public void StartMode() {}
        
    }
}