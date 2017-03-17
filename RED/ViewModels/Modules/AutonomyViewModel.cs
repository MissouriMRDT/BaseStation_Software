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

        public double LatitudeInput
        {
            get
            {
                return _model.latitudeInput;
            }
            set
            {
                _model.latitudeInput = value;
                NotifyOfPropertyChange(() => LatitudeInput);
            }
        }
        public double LongitudeInput
        {
            get
            {
                return _model.longitudeInput;
            }
            set
            {
                _model.longitudeInput = value;
                NotifyOfPropertyChange(() => LongitudeInput);
            }
        }

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
            _router.Send(_idResolver.GetId("AutonomousModeEnable"), new byte[0]);
        }

        public void DisableMode()
        {
            _router.Send(_idResolver.GetId("AutonomousModeDisable"), new byte[0]);
        }

        public void AddWaypoint()
        {
            byte[] msg = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(LatitudeInput), 0, msg, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(LongitudeInput), 0, msg, 8, 8);

            _router.Send(_idResolver.GetId("WaypointAdd"), msg);
        }

        public void ClearAllWaypoints()
        {
            _router.Send(_idResolver.GetId("WaypointsClearAll"), new byte[0]);
        }

        public void Calibrate()
        {
            _router.Send(_idResolver.GetId("AutonomyCalibrate"), new byte[0]);
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
