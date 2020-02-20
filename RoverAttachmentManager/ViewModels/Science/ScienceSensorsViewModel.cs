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

        public float MethaneConcentration
        {
            get
            {
                return _model.MethaneConcentration;
            }
            set
            {
                _model.MethaneConcentration = value;
                NotifyOfPropertyChange(() => MethaneConcentration);
            }
        }
        public float MethaneTemperature
        {
            get
            {
                return _model.MethaneTemperature;
            }
            set
            {
                _model.MethaneTemperature = value;
                NotifyOfPropertyChange(() => MethaneTemperature);
            }
        }
        public float CO2Concentration
        {
            get
            {
                return _model.CO2Concentration;
            }
            set
            {
                _model.CO2Concentration = value;
                NotifyOfPropertyChange(() => CO2Concentration);
            }
        }
        public float O2PartialPressure
        {
            get
            {
                return _model.O2PartialPressure;
            }
            set
            {
                _model.O2PartialPressure = value;
                NotifyOfPropertyChange(() => O2PartialPressure);
            }
        }
        public float O2Temperature
        {
            get
            {
                return _model.O2Temperature;
            }
            set
            {
                _model.O2Temperature = value;
                NotifyOfPropertyChange(() => O2Temperature);
            }
        }
        public float O2Concentration
        {
            get
            {
                return _model.O2Concentration;
            }
            set
            {
                _model.O2Concentration = value;
                NotifyOfPropertyChange(() => O2Concentration);
            }
        }
        public float O2BarometricPressure
        {
            get
            {
                return _model.O2BarometricPressure;
            }
            set
            {
                _model.O2BarometricPressure = value;
                NotifyOfPropertyChange(() => O2BarometricPressure);
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
        }


        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "Methane":
                    float[] MethaneData = packet.GetDataArray<float>();
                    MethaneConcentration = (float)(MethaneData[0]);
                    MethaneTemperature = (float)(MethaneData[1]);
                    break;

                case "CO2":
                    CO2Concentration = (float)(packet.GetData<float>());
                    break;

                case "O2":
                    float[] O2Data = packet.GetDataArray<float>();
                    O2PartialPressure = (float)(O2Data[0]);
                    O2Temperature = (float)(O2Data[1]);
                    O2Concentration = (float)(O2Data[2]);
                    O2BarometricPressure = (float)(O2Data[3]);
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
