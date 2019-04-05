using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RED.Models.Modules;
using RED.ViewModels.Navigation;
using System;

namespace RED.ViewModels.Modules
{
    public class AutonomyViewModel : PropertyChangedBase, IRovecommReceiver
    {
        private readonly AutonomyModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _logger;
        private readonly WaypointManagerViewModel _waypointManager;

        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger, WaypointManagerViewModel waypointManager)
        {
            _model = new AutonomyModel();

            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;
            _waypointManager = waypointManager;

            _rovecomm.NotifyWhenMessageReceived(this, "WaypointReached");
        }

        public void EnableMode()
        {
            _rovecomm.SendCommand(new Packet("AutonomousModeEnable"), true);
        }

        public void DisableMode()
        {
            _rovecomm.SendCommand(new Packet("AutonomousModeDisable"), true);
        }

        public void AddWaypoint()
        {
            AddWaypoint(_waypointManager.SelectedWaypoint);
        }

        public void ClearAllWaypoints()
        {
            _rovecomm.SendCommand(new Packet("WaypointsClearAll"), true);
        }

        public void Calibrate()
        {
            _rovecomm.SendCommand(new Packet("AutonomyCalibrate"), true);
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

		public void ReceivedRovecommMessageCallback(int index, bool reliable) {
			ReceivedRovecommMessageCallback(_rovecomm.GetPacketByID(index), false);
		}

		private void AddWaypoint(Waypoint waypoint)
        {
            byte[] msg = new byte[2 * sizeof(Int64)];
            _logger.Log(waypoint.Latitude.ToString());
            Int64 lat = (Int64.Parse((waypoint.Latitude * 10E7).ToString()));
            Int64 lon = (Int64.Parse((waypoint.Longitude * 10E7).ToString()));
            _logger.Log(lat.ToString());


            Buffer.BlockCopy(BitConverter.GetBytes(lat), 0, msg, 0 * sizeof(Int64), sizeof(Int64));
            Buffer.BlockCopy(BitConverter.GetBytes(lon), 0, msg, 1 * sizeof(Int64), sizeof(Int64));

            // TODO: Figure this out
            _rovecomm.SendCommand(new Packet("WaypointAdd", msg, 2, (byte)DataTypes.INT64_T), true);
        }
    }
}
