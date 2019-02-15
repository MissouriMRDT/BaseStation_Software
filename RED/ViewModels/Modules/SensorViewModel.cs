using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using RED.Models.Network;
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

            _rovecomm.NotifyWhenMessageReceived(this, "IMUTemperature");
            _rovecomm.NotifyWhenMessageReceived(this, "NavPitch");
            _rovecomm.NotifyWhenMessageReceived(this, "NavRoll");
            _rovecomm.NotifyWhenMessageReceived(this, "NavTrueHeading");
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "IMUTemperature": IMUTemperature = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavPitch": Pitch = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavRoll": Roll = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavTrueHeading": TrueHeading = BitConverter.ToSingle(packet.Data, 0); break;
            }
        }
    }
}