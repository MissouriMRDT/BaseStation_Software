using Core;
using Core.Interfaces;
using Core.Models;
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

        public void Enable() => _rovecomm.SendCommand(new Packet("AutonomousModeEnable"), true);

        public void Disable() => _rovecomm.SendCommand(new Packet("AutonomousModeDisable"), true);

        public void ClearAllWaypoints() => _rovecomm.SendCommand(new Packet("WaypointsClearAll"), true);

        public void Calibrate() => _rovecomm.SendCommand(new Packet("AutonomyCalibrate"), true);

        public ControlsViewModel(IRovecomm networkMessenger)
        {
            _model = new ControlsModel();
            _rovecomm = networkMessenger;
            _waypointManager = WaypointManager.Instance;
        }

        public void AddWaypoint()
        {
            Waypoint waypoint = _waypointManager.SelectedWaypoint;
            byte[] msg = new byte[2 * sizeof(double)];
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Longitude), 0, msg, 0 * sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Latitude), 0, msg, 1 * sizeof(double), sizeof(double));
            Array.Reverse(msg);

            _rovecomm.SendCommand(new Packet("WaypointAdd", msg, 2, (byte)7), true);
        }
    }
}
