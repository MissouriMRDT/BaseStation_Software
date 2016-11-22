using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;

namespace RED.ViewModels.Modules
{
    public class SensorViewModel : PropertyChangedBase, ISubscribe
    {
        private SensorModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _log;

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

        public SensorViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger log)
        {
            _model = new SensorModel();
            _router = router;
            _idResolver = idResolver;
            _log = log;

            _router.Subscribe(this, _idResolver.GetId("Voltage"));
            _router.Subscribe(this, _idResolver.GetId("Ultrasonic"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "Ultrasonic":
                    switch (data[0])
                    {
                        case 0: Ultrasonic0 = data[1]; break;
                        case 1: Ultrasonic1 = data[1]; break;
                        case 2: Ultrasonic2 = data[1]; break;
                        case 3: Ultrasonic3 = data[1]; break;
                        default: _log.Log("Unsupported Ultrasonic Sensor (#" + data[0] + ") Telemetry Recieved"); break;
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