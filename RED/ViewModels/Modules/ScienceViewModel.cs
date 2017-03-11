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
        private ScienceModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

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
        }

        public void Sensor0On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Enable);
        }
        public void Sensor0Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor0Disable);
        }
        public void Sensor1On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Enable);
        }
        public void Sensor1Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor1Disable);
        }
        public void Sensor2On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Enable);
        }
        public void Sensor2Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor2Disable);
        }
        public void Sensor3On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Enable);
        }
        public void Sensor3Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Disable);
        }
        public void Sensor4On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Enable);
        }
        public void Sensor4Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor4Disable);
        }
        public void Sensor5On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Enable);
        }
        public void Sensor5Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor5Disable);
        }
        public void Sensor6On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Enable);
        }
        public void Sensor6Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Sensor6Disable);
        }

        public void RequestLaserOn()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOn);
            _log.Log("Science Laser On requested.");
        }
        public void RequestLaserOff()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOff);
            _log.Log("Science Laser Off requested.");
        }
        public void SpectrometerRun()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.SpectrometerRun);
            _log.Log("Spectrometer Run command sent");
        }

        public void Carousel1()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)0);
        }
        public void Carousel2()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)1);
        }
        public void Carousel3()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)2);
        }
        public void Carousel4()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)3);
        }
        public void Carousel5()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)4);
        }

        public void FunnelOpen()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelOpen);
        }
        public void FunnelClose()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelClose);
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
            if (!SensorDataFile.CanWrite) return;
            var data = Encoding.UTF8.GetBytes(String.Format("{0:s} {1} {2}{3}", DateTime.Now, sensorName, value.ToString(), Environment.NewLine));
            await SensorDataFile.WriteAsync(data, 0, data.Length);
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "SciSensor0":
                    Sensor0Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor0", Sensor0Value);
                    break;
                case "SciSensor1":
                    Sensor1Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor1", Sensor1Value);
                    break;
                case "SciSensor2":
                    Sensor2Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor2", Sensor2Value);
                    break;
                case "SciSensor3":
                    Sensor3Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor3", Sensor3Value);
                    break;
                case "SciSensor4":
                    Sensor4Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor4", Sensor4Value);
                    break;
                case "SciSensor5":
                    Sensor5Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor5", Sensor5Value);
                    break;
                case "SciSensor6":
                    Sensor6Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Sensor6", Sensor6Value);
                    break;
            }
        }

        public enum ScienceRequestTypes : ushort
        {
            Sensor0Enable = 1,
            Sensor0Disable = 2,
            Sensor1Enable = 3,
            Sensor1Disable = 4,
            Sensor2Enable = 5,
            Sensor2Disable = 6,
            Sensor3Enable = 7,
            Sensor3Disable = 8,
            Sensor4Enable = 9,
            Sensor4Disable = 10,
            Sensor5Enable = 11,
            Sensor5Disable = 12,
            Sensor6Enable = 13,
            Sensor6Disable = 14,
            Sensor7Enable = 15,
            Sensor7Disable = 16,
            CCDRequest = 17,
            LaserOn = 18,
            LaserOff = 19,
            FunnelOpen = 20,
            FunnelClose = 21,
            SpectrometerRun = 22
        }
    }
}