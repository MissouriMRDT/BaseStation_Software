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
        private DateTime GetTimeDiff()
        {
            TimeSpan nowSpan = DateTime.UtcNow.Subtract(ScienceGraph.StartTime);
            return new DateTime(nowSpan.Ticks);
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

        public ScienceViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceModel();
            ScienceGraph = new ScienceGraphViewModel(networkMessenger, idResolver, log);
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Controls";
            ModeType = "ScienceControls";

            SpectrometerIPAddress = IPAddress.Parse("192.168.1.138");

            _rovecomm.NotifyWhenMessageReceived(this, "ScienceSensors");
            _rovecomm.NotifyWhenMessageReceived(this, "ScrewAtPos");


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

                    ScienceGraph.UpdateSensorGraphs();
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
            ScienceGraph.SensorPlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = ScienceGraph.SiteTimes[SiteNumber * 2],
                MaximumX = ScienceGraph.SiteTimes[(SiteNumber * 2) + 1],
                Text = "Site " + SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),

            });
            ScienceGraph.MethanePlotModel.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation
            {
                MinimumX = ScienceGraph.SiteTimes[SiteNumber * 2],
                MaximumX = ScienceGraph.SiteTimes[(SiteNumber * 2) + 1],
                Text = "Site " + SiteNumber,
                Fill = OxyColor.FromAColor(50, OxyColors.DarkOrange),
                
            });
        }

        public void ReachedSite()
        {
            double siteTime = OxyPlot.Axes.DateTimeAxis.ToDouble(GetTimeDiff());
            ScienceGraph.SiteTimes[SiteNumber * 2] = siteTime;
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
            ScienceGraph.SiteTimes[(SiteNumber * 2) + 1] = siteTime;

            double methaneAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor4Series, "Methane vs Time", "Methane (parts per billion)", 2000, SpectrometerFilePath + "\\Methane-Site" + SiteNumber + ".png");
            double tempAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor0Series, "Temperature vs Time", "Temperature (Celsius)", 50, SpectrometerFilePath + "\\Temperature-Site" + SiteNumber + ".png");
            double humidityAvg = ScienceGraph.AverageValueForSeries(ScienceGraph.Sensor1Series, "Humidity vs Time", "Humidity (%)", 100, SpectrometerFilePath + "\\Humidity-Site" + SiteNumber + ".png");

            WriteSiteData(tempAvg, humidityAvg, methaneAvg);
            
            CreateSiteAnnotation();
            SiteNumber++;
        }

        public void StartMode() {}

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