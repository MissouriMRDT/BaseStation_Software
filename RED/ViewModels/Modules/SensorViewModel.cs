using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Network;
using RED.Models.Modules;
using System;

namespace RED.ViewModels.Modules
{
    public class SensorViewModel : PropertyChangedBase, ISubscribe
    {
        private readonly SensorModel _model;
        private readonly INetworkMessenger _networkMessenger;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

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

        public SensorViewModel(INetworkMessenger networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new SensorModel();
            _networkMessenger = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            _networkMessenger.Subscribe(this, _idResolver.GetId("IMUTemperature"));
            _networkMessenger.Subscribe(this, _idResolver.GetId("IMUAccelerometer"));
            _networkMessenger.Subscribe(this, _idResolver.GetId("IMUGyroscope"));
            _networkMessenger.Subscribe(this, _idResolver.GetId("IMUMagnetometer"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data, bool reliable)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "IMUTemperature": IMUTemperature = BitConverter.ToSingle(data, 0); break;
                case "IMUAccelerometer":
                    IMUAccelerometerX = BitConverter.ToSingle(data, 0 * sizeof(Single));
                    IMUAccelerometerY = BitConverter.ToSingle(data, 1 * sizeof(Single));
                    IMUAccelerometerZ = BitConverter.ToSingle(data, 2 * sizeof(Single));
                    break;
                case "IMUGyroscope":
                    IMUGyroscopeRoll = BitConverter.ToSingle(data, 0 * sizeof(Single));
                    IMUGyroscopePitch = BitConverter.ToSingle(data, 1 * sizeof(Single));
                    IMUGyroscopeYaw = BitConverter.ToSingle(data, 2 * sizeof(Single));
                    break;
                case "IMUMagnetometer":
                    IMUMagnetometerX = BitConverter.ToSingle(data, 0 * sizeof(Single));
                    IMUMagnetometerY = BitConverter.ToSingle(data, 1 * sizeof(Single));
                    IMUMagnetometerZ = BitConverter.ToSingle(data, 2 * sizeof(Single));
                    break;
            }
        }
    }
}