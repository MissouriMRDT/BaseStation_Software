using Caliburn.Micro;
using Core.Interfaces;
using Core.RoveProtocol;
using RED.Models.Modules;
using RED.Models.Network;
using System;
using System.Net;

namespace RED.ViewModels.Modules
{
    public class SensorViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly SensorModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public float Lidar
        {
            get
            {
                return _model.Lidar;
            }
            set
            {
                _model.Lidar = value;
                NotifyOfPropertyChange(() => Lidar);
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

            _rovecomm.NotifyWhenMessageReceived(this, "Lidar");
            _rovecomm.NotifyWhenMessageReceived(this, "PitchHeadingRoll");
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {

                case "Lidar":
                    Byte[] data = packet.GetDataArray<Byte>();
                    if(data[1] != 5)
                    {
                        Lidar = (float)(data[0] / 10.0);
                    }
                    break;

                case "PitchHeadingRoll":
                    Int16[] d = packet.GetDataArray<Int16>();
                    Pitch = d[0];
                    TrueHeading = d[1];
                    Roll = d[2];
                    break;
            }
        }
	}
}