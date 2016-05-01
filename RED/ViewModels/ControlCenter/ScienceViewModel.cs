using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class ScienceViewModel : PropertyChangedBase, ISubscribe
    {
        private ScienceModel _model;
        private ControlCenterViewModel _cc;

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

        public ScienceViewModel(ControlCenterViewModel cc)
        {
            _model = new ScienceModel();
            _cc = cc;

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Temperature1"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Temperature2"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Temperature3"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Temperature4"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Moisture1"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Moisture2"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Moisture3"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Moisture4"));
        }

        public void Temperature1On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp1Enable);
        }
        public void Temperature1Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp1Disable);
        }
        public void Temperature2On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp2Enable);
        }
        public void Temperature2Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp2Disable);
        }
        public void Temperature3On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp3Enable);
        }
        public void Temperature3Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp3Disable);
        }
        public void Temperature4On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp4Enable);
        }
        public void Temperature4Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Temp4Disable);
        }
        public void Moisture1On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture1Enable);
        }
        public void Moisture1Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture1Disable);
        }
        public void Moisture2On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture2Enable);
        }
        public void Moisture2Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture2Disable);
        }
        public void Moisture3On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture3Enable);
        }
        public void Moisture3Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture3Disable);
        }
        public void Moisture4On()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture4Enable);
        }
        public void Moisture4Off()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.Moisture4Disable);
        }

        public void RequestCCD()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (ushort)ScienceRequestTypes.CCDRequest);
            _cc.Console.WriteToConsole("CCD data requested");
        }
        public void RequestLaserOn()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOn);
            _cc.Console.WriteToConsole("Science Laser On requested.");
        }
        public void RequestLaserOff()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (byte)ScienceRequestTypes.LaserOff);
            _cc.Console.WriteToConsole("Science Laser Off requested.");
        }

        public void Carousel1()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)0);
        }
        public void Carousel2()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)1);
        }
        public void Carousel3()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)2);
        }
        public void Carousel4()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)3);
        }
        public void Carousel5()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)4);
        }
        public void Carousel6()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("CarouselPosition"), (byte)5);
        }

        public void FunnelOpen()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelOpen);
        }
        public void FunnelClose()
        {
            _cc.DataRouter.Send(_cc.MetadataManager.GetId("ScienceCommand"), (byte)ScienceRequestTypes.FunnelClose);
        }


        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_cc.MetadataManager.GetTelemetry(dataId).Name)
            {
                case "Temperature1":
                    Temperature1Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Temperature2":
                    Temperature2Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Temperature3":
                    Temperature3Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Temperature4":
                    Temperature4Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Moisture1":
                    Moisture1Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Moisture2":
                    Moisture2Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Moisture3":
                    Moisture3Value = BitConverter.ToSingle(data, 0);
                    break;
                case "Moisture4":
                    Moisture4Value = BitConverter.ToSingle(data, 0);
                    break;
            }
        }

        public async void StartCCDListener()
        {
            var listener = new TcpListener(System.Net.IPAddress.Any, CCDPortNumber);

            listener.Start();
            while (true)
            {
                string filename = Path.Combine(CCDFilePath, "REDCCDData" + DateTime.Now.ToString("s") + ".dat");

                using (var client = await listener.AcceptTcpClientAsync())
                using (var file = File.Create(filename))
                {
                    await client.GetStream().CopyToAsync(file);
                }
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