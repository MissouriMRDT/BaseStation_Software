using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using RED.ViewModels.Navigation;
using System;

namespace RED.ViewModels.Modules
{
    public class AutonomyViewModel : PropertyChangedBase, ISubscribe
    {
        private readonly AutonomyModel _model;
        private readonly IDataRouter _router;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _logger;

        public AutonomyViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();

            _router = router;
            _idResolver = idResolver;
            _logger = logger;

            _router.Subscribe(this, _idResolver.GetId("WaypointReached"));
        }

        public void EnableMode()
        {
            _router.Send(_idResolver.GetId("AutonomousModeEnable"), new byte[0], true);
        }

        public void DisableMode()
        {
            _router.Send(_idResolver.GetId("AutonomousModeDisable"), new byte[0], true);
        }

        public void ClearAllWaypoints()
        {
            _router.Send(_idResolver.GetId("WaypointsClearAll"), new byte[0], true);
        }

        public void Calibrate()
        {
            _router.Send(_idResolver.GetId("AutonomyCalibrate"), new byte[0], true);
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data, bool reliable)
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

            _router.Send(_idResolver.GetId("WaypointAdd"), msg, true);
        }
    }
}
