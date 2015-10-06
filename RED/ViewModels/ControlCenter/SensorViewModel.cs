using Caliburn.Micro;
using RED.Models;
using RED.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class SensorViewModel : PropertyChangedBase, ISubscribe
    {
        private SensorModel _model;
        private ControlCenterViewModel _cc;

        public float Voltage
        {
            get
            {
                return _model.Voltage;
            }
            set
            {
                _model.Voltage = value;
                NotifyOfPropertyChange(() => Voltage);
            }
        }

        public byte Ultrasonic0
        {
            get
            {
                return _model.Ultrasonic0;
            }
            set
            {
                _model.Ultrasonic0 = value;
                NotifyOfPropertyChange(() => Ultrasonic0);
            }
        }
        public byte Ultrasonic1
        {
            get
            {
                return _model.Ultrasonic1;
            }
            set
            {
                _model.Ultrasonic1 = value;
                NotifyOfPropertyChange(() => Ultrasonic1);
            }
        }
        public byte Ultrasonic2
        {
            get
            {
                return _model.Ultrasonic2;
            }
            set
            {
                _model.Ultrasonic2 = value;
                NotifyOfPropertyChange(() => Ultrasonic2);
            }
        }
        public byte Ultrasonic3
        {
            get
            {
                return _model.Ultrasonic3;
            }
            set
            {
                _model.Ultrasonic3 = value;
                NotifyOfPropertyChange(() => Ultrasonic3);
            }
        }

        public SensorViewModel(ControlCenterViewModel cc)
        {
            _model = new SensorModel();
            _cc = cc;

            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Voltage"));
            _cc.DataRouter.Subscribe(this, _cc.MetadataManager.GetId("Ultrasonic"));
        }

        public void ReceiveFromRouter(byte dataId, byte[] data)
        {
            switch (_cc.MetadataManager.GetTelemetry(dataId).Name)
            {
                case "Ultrasonic":
                    switch (data[0])
                    {
                        case 0: Ultrasonic0 = data[1]; break;
                        case 1: Ultrasonic1 = data[1]; break;
                        case 2: Ultrasonic2 = data[1]; break;
                        case 3: Ultrasonic3 = data[1]; break;
                        default: _cc.Console.WriteToConsole("Unsupported Ultrasonic Sensor (#" + data[0] + ") Telemetry Recieved"); break;
                    }
                    break;
                case "Voltage":
                    /* 0    ->  -30V
                     * 512  ->    0V
                     * 1023 ->  +30V */
                    Voltage = (BitConverter.ToInt16(data, 0) - 512) * (30f / 512);
                    break;
            }
        }
    }
}