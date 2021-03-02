using Core;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using Core.ViewModels;
using RoverAttachmentManager.Models.Autonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class ControlsViewModel
    {
        private readonly IRovecomm _rovecomm;

        private readonly ControlsModel _model;

        private readonly WaypointManager _waypointManager;

        public SentWaypointsViewModel SentWaypoints
        {
            get
            {
                return _model.SentWaypoints;
            }
            set
            {
                _model.SentWaypoints = value;
            }
        }

        public void Enable()
        {
            _rovecomm.SendCommand(Packet.Create("AutonomousModeEnable"), false);
        }

        public void Disable() => _rovecomm.SendCommand(Packet.Create("AutonomousModeDisable"), false);

        public ControlsViewModel(IRovecomm networkMessenger, AutonomyViewModel parent)
        {
            _model = new ControlsModel();
            _rovecomm = networkMessenger;
            _waypointManager = WaypointManager.Instance;
            SentWaypoints = parent.SentWaypoints;
        }

        public void AddWaypoint()
        {
            Waypoint waypoint = _waypointManager.SelectedWaypoint;
            float[] sendValues = { (float)waypoint.Longitude, (float)waypoint.Latitude };
            _rovecomm.SendCommand(Packet.Create("WaypointAdd", sendValues), false);
            SentWaypoints.SentWaypoints(waypoint);
        }

        public void ClearAllWaypoints()
        {
          _rovecomm.SendCommand(Packet.Create("WaypointsClearAll"), false);
          SentWaypoints.ClearAllWaypoints();
        }
    }
}
