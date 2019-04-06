using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RoverAttachmentManager.Models.Autonomy;
using System;

namespace RoverAttachmentManager.ViewModels.Autonomy
{
    public class AutonomyViewModel: PropertyChangedBase, IRovecommReceiver
    {

        private readonly AutonomyModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _logger;
        //private readonly WaypointManagerViewModel _waypointManager;

        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
            //_waypointManager = waypointManager;

            _rovecomm.NotifyWhenMessageReceived(this, "WaypointReached");
        }

        public void EnableMode() => _rovecomm.SendCommand(new Packet("AutonomousModeEnable"), true);

        public void DisableMode() => _rovecomm.SendCommand(new Packet("AutonomousModeDisable"), true);

        public void AddWaypoint() => AddWaypoint(0);

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

        private void AddWaypoint(int val)
        {
            // TODO add waypoint to this
            byte[] msg = new byte[2 * sizeof(double)];
            Buffer.BlockCopy(BitConverter.GetBytes(val), 0, msg, 0 * sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(val), 0, msg, 1 * sizeof(double), sizeof(double));

            _rovecomm.SendCommand(new Packet("WaypointAdd", msg, 2, (byte)DataTypes.INT16_T), true);
        }

        public void ReceivedRovecommMessageCallback(int index, bool reliable)
        {
            ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
        }
    }
}
