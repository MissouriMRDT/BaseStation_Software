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