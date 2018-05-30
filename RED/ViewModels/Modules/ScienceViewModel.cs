using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RED.ViewModels.Modules
{
    public class ScienceViewModel : PropertyChangedBase, IRovecommReceiver, IInputMode
    {

        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        private const int ScrewSpeedScale = 1000;
        private const int DrillSpeedScale = 1000;
        private const int GenevaSpeedScale = 666;

        public string Name { get; }
        public string ModeType { get; }

        private readonly ScienceModel _model;

        public string ControlState
        {
            get
            {
                return _model.ControlState;
            }
            set
            {
                _model.ControlState = value;
                NotifyOfPropertyChange(() => ControlState);
            }
        }
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
        public int Sensor1Value
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
        public int Sensor3Value
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
        public float Sensor5Value
        {
            get
            {
                return _model.Sensor5Value;
            }
            set
            {
                _model.Sensor5Value = value;
                NotifyOfPropertyChange(() => Sensor5Value);
            }
        }
        public float Sensor6Value
        {
            get
            {
                return _model.Sensor6Value;
            }
            set
            {
                _model.Sensor6Value = value;
                NotifyOfPropertyChange(() => Sensor6Value);
            }
        }
        public float Sensor7Value
        {
            get
            {
                return _model.Sensor7Value;
            }
            set
            {
                _model.Sensor7Value = value;
                NotifyOfPropertyChange(() => Sensor7Value);
            }
        }
        public float Sensor8Value
        {
            get
            {
                return _model.Sensor8Value;
            }
            set
            {
                _model.Sensor8Value = value;
                NotifyOfPropertyChange(() => Sensor8Value);
            }
        }
        public float Sensor9Value
        {
            get
            {
                return _model.Sensor9Value;
            }
            set
            {
                _model.Sensor9Value = value;
                NotifyOfPropertyChange(() => Sensor9Value);
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

        public ScienceViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            Name = "Science Controls";
            ModeType = "ScienceControls";

            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor0"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor1"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor2"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor3"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor4"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor5"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor6"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor7"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor8"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("SciSensor9"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("Temperature"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("Moisture"));
        }

        public void StartMode()
        {
            ControlState = "OpenLoop";
        }

        public void UpdateControlState(Dictionary<string, float> values)
        {
            if(values["ClosedLoop"] == 1)
            {
                ControlState = "ClosedLoop";
            }
            else if(values["OpenLoop"] == 1)
            {
                ControlState = "OpenLoop";
            }
        }

        public void SetValues(Dictionary<string, float> values)
        {
            UpdateControlState(values);

            float screwMovement = values["Screw"];
            float drillSpeed = values["Drill"];
            float genevaSpeed = values["GenevaLeft"] == 1 ? 1 : values["GenevaRight"] == 1 ? -1 : 0;
            _rovecomm.SendCommand(_idResolver.GetId("Drill"), (Int16)(drillSpeed * DrillSpeedScale));

            if (ControlState == "OpenLoop")
            {
                _rovecomm.SendCommand(_idResolver.GetId("Screw"), (Int16)(screwMovement * ScrewSpeedScale));
                _rovecomm.SendCommand(_idResolver.GetId("Geneva"), (Int16)(genevaSpeed * GenevaSpeedScale));
            }
        }

        public void StopMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("Screw"), (Int16)(0), true);
            _rovecomm.SendCommand(_idResolver.GetId("Drill"), (Int16)(0), true);
        }

        public void SensorAllOn()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SensorAllEnable, true);
        }
        public void SensorAllOff()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SensorAllDisable, true);
        }
        public void Sensor0On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Enable, true);
        }
        public void Sensor0Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Disable, true);
        }
        public void Sensor1On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Enable, true);
        }
        public void Sensor1Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Disable, true);
        }
        public void Sensor2On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Enable, true);
        }
        public void Sensor2Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Disable, true);
        }
        public void Sensor3On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor3Enable, true);
        }
        public void Sensor3Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor3Disable, true);
        }
        public void Sensor4On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Enable, true);
        }
        public void Sensor4Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Disable, true);
        }
        public void Sensor5On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Enable, true);
        }
        public void Sensor5Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Disable, true);
        }
        public void Sensor6On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Enable, true);
        }
        public void Sensor6Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Disable, true);
        }
        public void Sensor7On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor7Enable, true);
        }
        public void Sensor7Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor7Disable, true);
        }
        public void Sensor8On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor8Enable, true);
        }
        public void Sensor8Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor8Disable, true);
        }
        public void Sensor9On()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor9Enable, true);
        }
        public void Sensor9Off()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor9Disable, true);
        }

        public void RequestSpectrometer()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SpectrometerRun, true);
            _log.Log("Spectrometer data requested");
        }
        public async void DownloadSpectrometer()
        {
            _log.Log("Spectrometer data download started");
            string filename = Path.Combine(SpectrometerFilePath, "REDSpectrometerData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".dat");
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(SpectrometerIPAddress, SpectrometerPortNumber);
                    _log.Log("Spectrometer connection established");
                    using (var file = File.Create(filename))
                    {
                        await client.GetStream().CopyToAsync(file);
                    }
                }
                _log.Log($"Spectrometer data downloaded into {filename}");
            }
            catch (Exception e)
            {
                _log.Log("There was an error downloading the spectrometer data:{0}{1}", Environment.NewLine, e);
            }
        }

        public void RequestLaserOn()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOn, true);
            _log.Log("Science Laser On requested");
        }
        public void RequestLaserOff()
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOff, true);
            _log.Log("Science Laser Off requested");
        }

        public void SetGenevaPosition(byte index)
        {
            _rovecomm.SendCommand(_idResolver.GetId("GenevaPosition"), index);
        }

        public void SetScrewPosition(byte index)
        {
            _rovecomm.SendCommand(_idResolver.GetId("ScrewPosition"), index);
        }

        public void SaveFileStart()
        {
            SensorDataFile = new FileStream("REDSensorData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".dat", FileMode.Create);
        }
        public void SaveFileStop()
        {
            if (SensorDataFile.CanWrite)
                SensorDataFile.Close();
        }

        private async void SaveFileWrite(string sensorName, object value)
        {
            if (SensorDataFile == null || !SensorDataFile.CanWrite) return;
            var data = Encoding.UTF8.GetBytes(String.Format("{0:s} {1} {2}{3}", DateTime.Now, sensorName, value.ToString(), Environment.NewLine));
            await SensorDataFile.WriteAsync(data, 0, data.Length);
        }

        public void ReceivedRovecommMessageCallback(ushort dataId, byte[] data, bool reliable)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "SciSensor0":
                    Sensor0Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor00", Sensor0Value);
                    break;
                case "Temperature":
                    Sensor1Value = BitConverter.ToInt16(data, 0);
                    SaveFileWrite("Sensor01", Sensor1Value);
                    break;
                case "SciSensor2":
                    Sensor2Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor02", Sensor2Value);
                    break;
                case "Moisture":
                    Sensor3Value = BitConverter.ToInt16(data, 0);
                    SaveFileWrite("Sensor03", Sensor3Value);
                    break;
                case "SciSensor4":
                    Sensor4Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor04", Sensor4Value);
                    break;
                case "SciSensor5":
                    Sensor5Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor05", Sensor5Value);
                    break;
                case "SciSensor6":
                    Sensor6Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor06", Sensor6Value);
                    break;
                case "SciSensor7":
                    Sensor7Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor07", Sensor7Value);
                    break;
                case "SciSensor8":
                    Sensor8Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor08", Sensor8Value);
                    break;
                case "SciSensor9":
                    Sensor9Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor09", Sensor9Value);
                    break;
                default:
                    break;
            }
        }

        public enum ScienceRequestTypes : ushort
        {
            SensorAllEnable = 0,
            SensorAllDisable = 1,
            LaserOn = 2,
            LaserOff = 3,
            SpectrometerRun = 6,
            Sensor0Enable = 16,
            Sensor0Disable = 17,
            Sensor1Enable = 18,
            Sensor1Disable = 19,
            Sensor2Enable = 20,
            Sensor2Disable = 21,
            Sensor3Enable = 22,
            Sensor3Disable = 23,
            Sensor4Enable = 24,
            Sensor4Disable = 25,
            Sensor5Enable = 26,
            Sensor5Disable = 27,
            Sensor6Enable = 28,
            Sensor6Disable = 29,
            Sensor7Enable = 30,
            Sensor7Disable = 31,
            Sensor8Enable = 32,
            Sensor8Disable = 33,
            Sensor9Enable = 34,
            Sensor9Disable = 35
        }
    }
}