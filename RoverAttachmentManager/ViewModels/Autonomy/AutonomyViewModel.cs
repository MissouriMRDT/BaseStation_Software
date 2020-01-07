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

        public string SentWaypointsText
        {
            get
            {
                return _model._waypointsText;
            }
            set
            {
                _model._waypointsText = value;
                NotifyOfPropertyChange();
            }
        }
        public InputManagerViewModel InputManager
        {
            get
            {
                return _model.InputManager;
            }
            set
            {
                _model.InputManager = value;
                NotifyOfPropertyChange(() => InputManager);
            }
        }
        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger, MainWindowViewModel parent)
        {
            _model = new AutonomyModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
            _waypointManager = WaypointManager.Instance;
            InputManager = parent.InputManager;

            _rovecomm.NotifyWhenMessageReceived(this, "WaypointReached");
        }

        public void Enable() => _rovecomm.SendCommand(new Packet("AutonomousModeEnable"), true);

        public void Disable() => _rovecomm.SendCommand(new Packet("AutonomousModeDisable"), true);

        public void ClearAllWaypoints()
        {
            _rovecomm.SendCommand(new Packet("WaypointsClearAll"), true);
            string wpclear = "--cleared--\n";
            SentWaypointsText += wpclear;
        }
      


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

        public void AddWaypoint()
        {
            Waypoint waypoint = _waypointManager.SelectedWaypoint;
            byte[] msg = new byte[2 * sizeof(double)];
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Longitude), 0, msg, 0 * sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Latitude), 0, msg, 1 * sizeof(double), sizeof(double));
            Array.Reverse(msg);

            string tempstring = waypoint.Name + " | Longitude: " + waypoint.Longitude.ToString() + " | Latitude: " + waypoint.Latitude.ToString() + "\n";
            SentWaypointsText += tempstring;
            _rovecomm.SendCommand(new Packet("WaypointAdd", msg, 2, (byte)7), true);
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
    }
}
