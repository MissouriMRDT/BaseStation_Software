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
        public string TBD
         {
            get
            {
                return _model.TBD;
            }
            set
            {
                _model.TBD = value;
                NotifyOfPropertyChange(() => TBD);
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
            //continue.
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
