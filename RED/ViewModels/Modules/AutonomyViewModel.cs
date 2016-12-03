using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using System;

namespace RED.ViewModels.Modules
{
    public class AutonomyViewModel : PropertyChangedBase, ISubscribe
    {
        private AutonomyModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;
        private ILogger _logger;

        public AutonomyViewModel(IDataRouter router, IDataIdResolver idResolver, ILogger logger)
        {
            _model = new AutonomyModel();

            _router = router;
            _idResolver = idResolver;
            _logger = logger;

            _router.Subscribe(this, _idResolver.GetId("WaypointReached"));
        }

        public void EnableAutonomousMode()
        {
            _router.Send(_idResolver.GetId("AutonomousModeEnable"), new byte[0]);
        }

        public void DisableAutonomousMode()
        {
            _router.Send(_idResolver.GetId("AutonomousModeDisable"), new byte[0]);
        }

        public void AddWaypoint()
        {
            double lat = 0;
            double lon = 0;

            byte[] msg = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(lat), 0, msg, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(lon), 0, msg, 4, 4);

            _router.Send(_idResolver.GetId("WaypointAdd"), msg);
        }

        public void ClearAllWaypoints()
        {
            _router.Send(_idResolver.GetId("WaypointsClearAll"), new byte[0]);
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_idResolver.GetName(dataId))
            {
                case "WaypointReached":
                    _logger.Log("Waypoint Reached");
                    break;
            }
        }

    }
}
