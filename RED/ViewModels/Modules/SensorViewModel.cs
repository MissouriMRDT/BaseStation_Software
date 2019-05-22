using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
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
            _rovecomm.NotifyWhenMessageReceived(this, "NavPitch");
            _rovecomm.NotifyWhenMessageReceived(this, "NavRoll");
            _rovecomm.NotifyWhenMessageReceived(this, "NavTrueHeading");
            _rovecomm.NotifyWhenMessageReceived(this, "PitchHeadingRoll");
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {

                case "Lidar":
                    if(packet.Data[1] != 5)
                    {
                        Lidar = (float)(packet.Data[0] / 10.0);
                    }
                    break;
                case "NavPitch": Pitch = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavRoll": Roll = BitConverter.ToSingle(packet.Data, 0); break;
                case "NavTrueHeading":
                    TrueHeading = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0)); break;
                case "PitchHeadingRoll":
                    Pitch = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 0));
                    TrueHeading = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 2));
                    Roll = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet.Data, 4));
                    break;
            }
        }

		public void ReceivedRovecommMessageCallback(int index, bool reliable) {
			ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
		}
	}
}