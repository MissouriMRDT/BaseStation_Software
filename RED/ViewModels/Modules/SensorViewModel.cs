using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;

namespace RED.ViewModels.Modules
{
    public class SensorViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly SensorModel _model;
        private readonly IRovecomm _rovecomm;
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
        public float Pitch
        {
            get
            {
                return _model.Pitch;
            }
            set
            {
                _model.Pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }
        public float Roll
        {
            get
            {
                return _model.Roll;
            }
            set
            {
                _model.Roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }
        public float TrueHeading
        {
            get
            {
                return _model.TrueHeading;
            }
            set
            {
                _model.TrueHeading = value;
                NotifyOfPropertyChange(() => TrueHeading);
            }
        }

        public SensorViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new SensorModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;

            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("IMUTemperature"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("NavPitch"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("NavRoll"));
            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("NavTrueHeading"));
        }

        public void ReceivedRovecommMessageCallback(ushort dataId, byte[] data, bool reliable)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "IMUTemperature": IMUTemperature = BitConverter.ToSingle(data, 0); break;
                case "NavPitch": Pitch = BitConverter.ToSingle(data, 0); break;
                case "NavRoll": Roll = BitConverter.ToSingle(data, 0); break;
                case "NavTrueHeading": TrueHeading = BitConverter.ToSingle(data, 0); break;
            }
        }
    }
}