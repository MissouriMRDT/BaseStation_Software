using Caliburn.Micro;
using Core;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels;
using RoverAttachmentManager.Models.Autonomy;
using RoverAttachmentManager.ViewModels.Autonomy;
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
        public StateControlViewModel StateControl
        {
            get
            {
                return _model._stateControl;
            }
            set
            {
                _model._stateControl = value;
                NotifyOfPropertyChange(() => StateControl);
            }
        }
        public SentWaypointsViewModel SentWaypoints
        {
            get
            {
                return _model._sentWaypoints;
            }

            set
            {
                _model._sentWaypoints = value;
                NotifyOfPropertyChange(() => SentWaypoints);
            }
        }
        public AutonomyTelemetryViewModel Telemetry
        {
            get
            {
                return _model._telemetry;
            }

            set
            {
                _model._telemetry = value;
                NotifyOfPropertyChange(() => Telemetry);
            }
        }
        public CameraViewModel Camera1
        {
            get
            {
                return _model._camera1;
            }
            set
            {
                _model._camera1 = value;
                NotifyOfPropertyChange(() => Camera1);
            }
        }
        public CameraViewModel Camera2
        {
            get
            {
                return _model._camera2;
            }
            set
            {
                _model._camera2 = value;
                NotifyOfPropertyChange(() => Camera2);
            }
        }

        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();
            StateControl = new StateControlViewModel();
            SentWaypoints = new SentWaypointsViewModel();
            Telemetry = new AutonomyTelemetryViewModel(networkMessenger, idResolver, logger);
            Controls = new ControlsViewModel(networkMessenger, this);

            Camera1 = new CameraViewModel(Core.CommonLog.Instance);
            Camera2 = new CameraViewModel(Core.CommonLog.Instance);

            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
            _waypointManager = WaypointManager.Instance;

            _rovecomm.NotifyWhenMessageReceived(this, "WaypointReached");
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
    }
}
