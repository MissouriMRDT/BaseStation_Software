using Caliburn.Micro;
using RED.Interfaces;
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

        public AutonomyViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();

            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _logger = logger;

            _rovecomm.NotifyWhenMessageReceived(this, _idResolver.GetId("WaypointReached"));
        }

        public void EnableMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("AutonomousModeEnable"), new byte[0], true);
        }

        public void DisableMode()
        {
            _rovecomm.SendCommand(_idResolver.GetId("AutonomousModeDisable"), new byte[0], true);
        }

        public void ClearAllWaypoints()
        {
            _rovecomm.SendCommand(_idResolver.GetId("WaypointsClearAll"), new byte[0], true);
        }

        public void Calibrate()
        {
            _rovecomm.SendCommand(_idResolver.GetId("AutonomyCalibrate"), new byte[0], true);
        }

        public void ReceivedRovecommMessageCallback(ushort dataId, byte[] data, bool reliable)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "WaypointReached":
                    _logger.Log("Waypoint Reached");
                    break;
            }
        }

        public void AddWaypoint(Waypoint waypoint)
        {
            byte[] msg = new byte[2 * sizeof(double)];
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Latitude), 0, msg, 0 * sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(waypoint.Longitude), 0, msg, 1 * sizeof(double), sizeof(double));

            _rovecomm.SendCommand(_idResolver.GetId("WaypointAdd"), msg, true);
        }
    }
}
