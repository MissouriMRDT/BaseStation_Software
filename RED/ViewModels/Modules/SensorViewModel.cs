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

        public float IMUTemperature
        {
            get
            {
                return _model.IMUTemperature;
            }
            set
            {
                _model.IMUTemperature = value;
                NotifyOfPropertyChange(() => IMUTemperature);
            }
        }
        public float IMUAccelerometerX
        {
            get
            {
                return _model.IMUAccelerometerX;
            }
            set
            {
                _model.IMUAccelerometerX = value;
                NotifyOfPropertyChange(() => IMUAccelerometerX);
            }
        }
        public float IMUAccelerometerY
        {
            get
            {
                return _model.IMUAccelerometerY;
            }
            set
            {
                _model.IMUAccelerometerY = value;
                NotifyOfPropertyChange(() => IMUAccelerometerY);
            }
        }
        public float IMUAccelerometerZ
        {
            get
            {
                return _model.IMUAccelerometerZ;
            }
            set
            {
                _model.IMUAccelerometerZ = value;
                NotifyOfPropertyChange(() => IMUAccelerometerZ);
            }
        }
        public float IMUGyroscopeRoll
        {
            get
            {
                return _model.IMUGyroscopeRoll;
            }
            set
            {
                _model.IMUGyroscopeRoll = value;
                NotifyOfPropertyChange(() => IMUGyroscopeRoll);
            }
        }
        public float IMUGyroscopePitch
        {
            get
            {
                return _model.IMUGyroscopePitch;
            }
            set
            {
                _model.IMUGyroscopePitch = value;
                NotifyOfPropertyChange(() => IMUGyroscopePitch);
            }
        }
        public float IMUGyroscopeYaw
        {
            get
            {
                return _model.IMUGyroscopeYaw;
            }
            set
            {
                _model.IMUGyroscopeYaw = value;
                NotifyOfPropertyChange(() => IMUGyroscopeYaw);
            }
        }
        public float IMUMagnetometerX
        {
            get
            {
                return _model.IMUMagnetometerX;
            }
            set
            {
                _model.IMUMagnetometerX = value;
                NotifyOfPropertyChange(() => IMUMagnetometerX);
            }
        }
        public float IMUMagnetometerY
        {
            get
            {
                return _model.IMUMagnetometerY;
            }
            set
            {
                _model.IMUMagnetometerY = value;
                NotifyOfPropertyChange(() => IMUMagnetometerY);
            }
        }
        public float IMUMagnetometerZ
        {
            get
            {
                return _model.IMUMagnetometerZ;
            }
            set
            {
                _model.IMUMagnetometerZ = value;
                NotifyOfPropertyChange(() => IMUMagnetometerZ);
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
            _router.Subscribe(this, _idResolver.GetId("IMUTemperature"));
            _router.Subscribe(this, _idResolver.GetId("IMUAccelerometerX"));
            _router.Subscribe(this, _idResolver.GetId("IMUAccelerometerY"));
            _router.Subscribe(this, _idResolver.GetId("IMUAccelerometerZ"));
            _router.Subscribe(this, _idResolver.GetId("IMUGyroscopeRoll"));
            _router.Subscribe(this, _idResolver.GetId("IMUGyroscopePitch"));
            _router.Subscribe(this, _idResolver.GetId("IMUGyroscopeYaw"));
            _router.Subscribe(this, _idResolver.GetId("IMUMagnetometerX"));
            _router.Subscribe(this, _idResolver.GetId("IMUMagnetometerY"));
            _router.Subscribe(this, _idResolver.GetId("IMUMagnetometerZ"));
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

                case "IMUTemperature": IMUTemperature = BitConverter.ToSingle(data, 0); break;
                case "IMUAccelerometerX": IMUAccelerometerX = BitConverter.ToSingle(data, 0); break;
                case "IMUAccelerometerY": IMUAccelerometerY = BitConverter.ToSingle(data, 0); break;
                case "IMUAccelerometerZ": IMUAccelerometerZ = BitConverter.ToSingle(data, 0); break;
                case "IMUGyroscopeRoll": IMUGyroscopeRoll = BitConverter.ToSingle(data, 0); break;
                case "IMUGyroscopePitch": IMUGyroscopePitch = BitConverter.ToSingle(data, 0); break;
                case "IMUGyroscopeYaw": IMUGyroscopeYaw = BitConverter.ToSingle(data, 0); break;
                case "IMUMagnetometerX": IMUMagnetometerX = BitConverter.ToSingle(data, 0); break;
                case "IMUMagnetometerY": IMUMagnetometerY = BitConverter.ToSingle(data, 0); break;
                case "IMUMagnetometerZ": IMUMagnetometerZ = BitConverter.ToSingle(data, 0); break;
            }
        }
    }
}