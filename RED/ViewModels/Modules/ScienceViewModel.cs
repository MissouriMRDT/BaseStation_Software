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

        public float Temperature1Value
        {
            get
            {
                return _model.Temperature1Value;
            }
            set
            {
                _model.Temperature1Value = value;
                NotifyOfPropertyChange(() => Temperature1Value);
            }
        }
        public float Temperature2Value
        {
            get
            {
                return _model.Temperature2Value;
            }
            set
            {
                _model.Temperature2Value = value;
                NotifyOfPropertyChange(() => Temperature2Value);
            }
        }
        public float Temperature3Value
        {
            get
            {
                return _model.Temperature3Value;
            }
            set
            {
                _model.Temperature3Value = value;
                NotifyOfPropertyChange(() => Temperature3Value);
            }
        }
        public float Temperature4Value
        {
            get
            {
                return _model.Temperature4Value;
            }
            set
            {
                _model.Temperature4Value = value;
                NotifyOfPropertyChange(() => Temperature4Value);
            }
        }
        public float Moisture1Value
        {
            get
            {
                return _model.Moisture1Value;
            }
            set
            {
                _model.Moisture1Value = value;
                NotifyOfPropertyChange(() => Moisture1Value);
            }
        }
        public float Moisture2Value
        {
            get
            {
                return _model.Moisture2Value;
            }
            set
            {
                _model.Moisture2Value = value;
                NotifyOfPropertyChange(() => Moisture2Value);
            }
        }
        public float Moisture3Value
        {
            get
            {
                return _model.Moisture3Value;
            }
            set
            {
                _model.Moisture3Value = value;
                NotifyOfPropertyChange(() => Moisture3Value);
            }
        }
        public float Moisture4Value
        {
            get
            {
                return _model.Moisture4Value;
            }
            set
            {
                _model.Moisture4Value = value;
                NotifyOfPropertyChange(() => Moisture4Value);
            }
        }

        public System.Net.IPAddress CCDIPAddress
        {
            get
            {
                return _model.CCDIPAddress;
            }
            set
            {
                _model.CCDIPAddress = value;
                NotifyOfPropertyChange(() => CCDIPAddress);
            }
        }
        public ushort CCDPortNumber
        {
            get
            {
                return _model.CCDPortNumber;
            }
            set
            {
                _model.CCDPortNumber = value;
                NotifyOfPropertyChange(() => CCDPortNumber);
            }
        }
        public string CCDFilePath
        {
            get
            {
                return _model.CCDFilePath;
            }
            set
            {
                _model.CCDFilePath = value;
                NotifyOfPropertyChange(() => CCDFilePath);
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

            _router.Subscribe(this, _idResolver.GetId("Temperature1"));
            _router.Subscribe(this, _idResolver.GetId("Temperature2"));
            _router.Subscribe(this, _idResolver.GetId("Temperature3"));
            _router.Subscribe(this, _idResolver.GetId("Temperature4"));
            _router.Subscribe(this, _idResolver.GetId("Moisture1"));
            _router.Subscribe(this, _idResolver.GetId("Moisture2"));
            _router.Subscribe(this, _idResolver.GetId("Moisture3"));
            _router.Subscribe(this, _idResolver.GetId("Moisture4"));
        }

        public void Temperature1On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp1Enable);
        }
        public void Temperature1Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp1Disable);
        }
        public void Temperature2On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp2Enable);
        }
        public void Temperature2Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp2Disable);
        }
        public void Temperature3On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp3Enable);
        }
        public void Temperature3Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp3Disable);
        }
        public void Temperature4On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp4Enable);
        }
        public void Temperature4Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp4Disable);
        }
        public void Moisture1On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture1Enable);
        }
        public void Moisture1Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture1Disable);
        }
        public void Moisture2On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture2Enable);
        }
        public void Moisture2Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture2Disable);
        }
        public void Moisture3On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture3Enable);
        }
        public void Moisture3Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture3Disable);
        }
        public void Moisture4On()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture4Enable);
        }
        public void Moisture4Off()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture4Disable);
        }

        public void RequestCCD()
        {
            _router.Send(_idResolver.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.CCDRequest);
            _log.Log("CCD data requested");
        }
        public async void DownloadCCD()
        {
            _log.Log("CCD data downloaded started.");
            string filename = Path.Combine(CCDFilePath, "REDCCDData" + DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".dat");
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(CCDIPAddress, CCDPortNumber);
                using (var file = File.Create(filename))
                {
                    await client.GetStream().CopyToAsync(file);
                }
            }
            _log.Log("CCD data downloaded into " + filename + ".");
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
        public void Carousel6()
        {
            _router.Send(_idResolver.GetId("CarouselPosition"), (byte)5);
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
                case "Temperature1":
                    Temperature1Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Temperature1", Temperature1Value);
                    break;
                case "Temperature2":
                    Temperature2Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Temperature2", Temperature2Value);
                    break;
                case "Temperature3":
                    Temperature3Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Temperature3", Temperature3Value);
                    break;
                case "Temperature4":
                    Temperature4Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Temperature4", Temperature4Value);
                    break;
                case "Moisture1":
                    Moisture1Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Moisture1", Moisture1Value);
                    break;
                case "Moisture2":
                    Moisture2Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Moisture2", Moisture2Value);
                    break;
                case "Moisture3":
                    Moisture3Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Moisture3", Moisture3Value);
                    break;
                case "Moisture4":
                    Moisture4Value = BitConverter.ToSingle(data, 0);
                    SaveFileWrite("Moisture4", Moisture4Value);
                    break;
            }
        }

        public enum ScienceRequestTypes : ushort
        {
            Temp1Enable = 1,
            Temp1Disable = 2,
            Temp2Enable = 3,
            Temp2Disable = 4,
            Temp3Enable = 5,
            Temp3Disable = 6,
            Temp4Enable = 7,
            Temp4Disable = 8,
            Moisture1Enable = 9,
            Moisture1Disable = 10,
            Moisture2Enable = 11,
            Moisture2Disable = 12,
            Moisture3Enable = 13,
            Moisture3Disable = 14,
            Moisture4Enable = 15,
            Moisture4Disable = 16,
            CCDRequest = 17,
            LaserOn = 18,
            LaserOff = 19,
            FunnelOpen = 20,
            FunnelClose = 21
        }
    }
}