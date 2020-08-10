using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using RoverAttachmentManager.Models.Autonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class AutonomyTelemetryViewModel: PropertyChangedBase, IRovecommReceiver
    {
        private readonly AutonomyTelemetryModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _logger;

        public string CurrentState
        {
            get
            {
                return _model.CurrentState;
            }
            set
            {
                _model.CurrentState = value;
                NotifyOfPropertyChange(() => CurrentState);
            }
        }
        public short Left
        {
            get
            {
                return _model.Left;
            }
            set
            {
                _model.Left = value;
                NotifyOfPropertyChange(() => Left);
            }
        }
        public short Right
        {
            get
            {
                return _model.Right;
            }
            set
            {
                _model.Right = value;
                NotifyOfPropertyChange(() => Right);
            }
        }
        public float CurrentLatitude
        {
            get
            {
                return _model.CurrentLatitude;
            }
            set
            {
                _model.CurrentLatitude = value;
                NotifyOfPropertyChange(() => CurrentLatitude);
            }
        }
        public float CurrentLongitude
        {
            get
            {
                return _model.CurrentLongitude;
            }
            set
            {
                _model.CurrentLongitude = value;
                NotifyOfPropertyChange(() => CurrentLongitude);
            }
        }
        public float TargetLatitude
        {
            get
            {
                return _model.TargetLatitude;
            }
            set
            {
                _model.TargetLatitude = value;
                NotifyOfPropertyChange(() => TargetLatitude);
            }
        }
        public float TargetLongitude
        {
            get
            {
                return _model.TargetLongitude;
            }
            set
            {
                _model.TargetLongitude = value;
                NotifyOfPropertyChange(() => TargetLongitude);
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
        public float Heading
        {
            get
            {
                return _model.Heading;
            }
            set
            {
                _model.Heading = value;
                NotifyOfPropertyChange(() => Heading);
            }
        }
        public float TargetHeading
        {
            get
            {
                return _model.TargetHeading;
            }
            set
            {
                _model.TargetHeading = value;
                NotifyOfPropertyChange(() => TargetHeading);
            }
        }
        public float LiDAR
        {
            get
            {
                return _model.LiDAR;
            }
            set
            {
                _model.LiDAR = value;
                NotifyOfPropertyChange(() => LiDAR);
            }
        }
        public AutonomyTelemetryViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyTelemetryModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
        }

        public void ReceivedRovecommMessageCallback(Packet packet, bool reliable)
        {
            switch (packet.Name)
            {
                case "WaypointReached":
                    _logger.Log("Waypoint Reached");
                    break;
            }
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
    }
}
