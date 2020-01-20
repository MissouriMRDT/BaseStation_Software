using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverAttachmentManager.Models.Science;
using System.IO;
using System.Net;

namespace RoverAttachmentManager.ViewModels.Science
{
    public class ScienceSensorsViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;
        private readonly ScienceSensorsModel _model;


        public PlotModel SensorPlotModel { set; private get; }
        public PlotModel MethanePlotModel { set; private get; }
        public OxyPlot.Series.LineSeries Sensor0Series;
        public OxyPlot.Series.LineSeries Sensor1Series;
        public OxyPlot.Series.LineSeries Sensor4Series;


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

        public ScienceSensorsViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log, ScienceViewModel parent)
        {
            _model = new ScienceSensorsModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            _rovecomm.NotifyWhenMessageReceived(this, "ScienceSensors");
            ScienceGraph = parent.ScienceGraph;

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


        public void UpdateSensorGraphs()
        {
            if (!ScienceGraph.Graphing) { return; }

            TimeSpan nowSpan = DateTime.UtcNow.Subtract(ScienceGraph.StartTime);
            DateTime now = new DateTime(nowSpan.Ticks);

            Sensor0Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor0Value));
            Sensor1Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor1Value));
            Sensor4Series.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), Sensor4Value));
            SensorPlotModel.InvalidatePlot(true);
            MethanePlotModel.InvalidatePlot(true);
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }


        public void StartSensorGraphs()
        {
            ScienceGraph.StartTime = DateTime.UtcNow;
            ScienceGraph.Graphing = true;
            ClearSensorGraphs();
        }


        public void ClearSensorGraphs()
        {
            Sensor0Series.Points.Clear();
            Sensor1Series.Points.Clear();
            Sensor4Series.Points.Clear();
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

        public void SaveFileStart()
        {
            SensorDataFile = new FileStream(ScienceGraph.SpectrometerFilePath + "\\REDSensorData-" + DateTime.Now.ToString("yyyyMMdd'-'HHmmss") + ".csv", FileMode.Create);
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
    }
}
