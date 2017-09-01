using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RED.ViewModels.Modules
{
    public class ScienceViewModel : PropertyChangedBase, ISubscribe
    {
        private readonly ScienceModel _model;
        private readonly IDataRouter _router;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

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

        public ScienceViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _model = new ScienceModel();
            _router = router;
            _idResolver = idResolver;
            _log = log;

            _router.Subscribe(this, _idResolver.GetId("SciSensor0"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor1"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor2"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor3"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor4"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor5"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor6"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor7"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor8"));
            _router.Subscribe(this, _idResolver.GetId("SciSensor9"));
        }

        public void SensorAllOn()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SensorAllEnable, true);
        }
        public void SensorAllOff()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SensorAllDisable, true);
        }
        public void Sensor0On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Enable, true);
        }
        public void Sensor0Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Disable, true);
        }
        public void Sensor1On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Enable, true);
        }
        public void Sensor1Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Disable, true);
        }
        public void Sensor2On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Enable, true);
        }
        public void Sensor2Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Disable, true);
        }
        public void Sensor3On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor3Enable, true);
        }
        public void Sensor3Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor3Disable, true);
        }
        public void Sensor4On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Enable, true);
        }
        public void Sensor4Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Disable, true);
        }
        public void Sensor5On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Enable, true);
        }
        public void Sensor5Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Disable, true);
        }
        public void Sensor6On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Enable, true);
        }
        public void Sensor6Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Disable, true);
        }
        public void Sensor7On()
        {
            _router.Send(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor7Enable, true);
        }
        public void Sensor7Off()
        {
            _router.Send(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor7Disable, true);
        }
        public void Sensor8On()
        {
            _router.Send(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor8Enable, true);
        }
        public void Sensor8Off()
        {
            _router.Send(_idResolver.GetId("ScienceArmCommand"), (ushort)ScienceRequestTypes.Sensor8Disable, true);
        }
        public void Sensor9On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor9Enable, true);
        }
        public void Sensor9Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor9Disable, true);
        }

        public void RequestSpectrometer()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SpectrometerRun, true);
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
                _log.Log("Spectrometer data downloaded into {0}", filename);
            }
            catch (Exception e)
            {
                _log.Log("There was an error downloading the spectrometer data: {0}", e.ToString());
            }
        }

        public void RequestLaserOn()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOn, true);
            _log.Log("Science Laser On requested");
        }
        public void RequestLaserOff()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOff, true);
            _log.Log("Science Laser Off requested");
        }

        public void SetCarouselPosition(byte carouselIndex)
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), carouselIndex);
        }

        public void FunnelOpen()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelOpen, true);
        }
        public void FunnelClose()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelClose, true);
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

        public void ReceiveFromRouter(ushort dataId, byte[] data, bool reliable)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "SciSensor0":
                    Sensor0Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor00", Sensor0Value);
                    break;
                case "SciSensor1":
                    Sensor1Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor01", Sensor1Value);
                    break;
                case "SciSensor2":
                    Sensor2Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor02", Sensor2Value);
                    break;
                case "SciSensor3":
                    Sensor3Value = BitConverter.ToSingle(data, 0);
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
            }
        }

        public enum ScienceRequestTypes : ushort
        {
            SensorAllEnable = 0,
            SensorAllDisable = 1,
            LaserOn = 2,
            LaserOff = 3,
            FunnelOpen = 4,
            FunnelClose = 5,
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