using Caliburn.Micro;
using Core;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels;
using RoverAttachmentManager.Models.Autonomy;
using System;
using System.Net;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class AutonomyViewModel: PropertyChangedBase, IRovecommReceiver
    {

        private readonly AutonomyModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _logger;
        private readonly WaypointManager _waypointManager;

        public ControlsViewModel Controls
        {
            get
            {
                return _model.Controls;
            }
            set
            {
                _model.Controls = value;
                NotifyOfPropertyChange(() => Controls);
            }
        }


        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();
            Controls = new ControlsViewModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
            _waypointManager = WaypointManager.Instance;

            _rovecomm.NotifyWhenMessageReceived(this, "WaypointReached");
        }

        public void Enable() => _rovecomm.SendCommand(new Packet("AutonomousModeEnable"), true);

        public void Disable() => _rovecomm.SendCommand(new Packet("AutonomousModeDisable"), true);

        public void ClearAllWaypoints() => _rovecomm.SendCommand(new Packet("WaypointsClearAll"), true);

        public void Calibrate() => _rovecomm.SendCommand(new Packet("AutonomyCalibrate"), true);
        
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
