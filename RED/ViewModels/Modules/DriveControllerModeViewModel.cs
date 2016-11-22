using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;

namespace RED.ViewModels.Modules
{
    public class DriveControllerModeViewModel : PropertyChangedBase, IControllerMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveControllerModeModel _model;
        private IDataRouter _router;
        private IDataIdResolver _idResolver;

        public int SpeedLeft
        {
            get
            {
                return _model.speedLeft;
            }
            set
            {
                _model.speedLeft = value;
                NotifyOfPropertyChange(() => SpeedLeft);
            }
        }
        public int SpeedRight
        {
            get
            {
                return _model.speedRight;
            }
            set
            {
                _model.speedRight = value;
                NotifyOfPropertyChange(() => SpeedRight);
            }
        }

        public string Name { get; set; }
        public IInputDevice InputVM { get; set; }

        public int SpeedLimit
        {
            get
            {
                return _model.speedLimit;
            }
            set
            {
                _model.speedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
            }
        }

        public bool ParabolicScaling
        {
            get
            {
                return _model.parabolicScaling;
            }
            set
            {
                _model.parabolicScaling = value;
            }
        }

        public DriveControllerModeViewModel(IInputDevice inputVM, IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new DriveControllerModeModel();
            _router = router;
            _idResolver = idResolver;
            InputVM = inputVM;
            Name = "Drive";
        }

        public void EnterMode()
        {
            SpeedLeft = 0;
            SpeedRight = 0;
        }

        public void EvaluateMode()
        {
            int newSpeedLeft;
            int newSpeedRight;

            #region Normalization of joystick input
            {
                float CurrentRawControllerSpeedLeft = InputVM.WheelsLeft;
                float CurrentRawControllerSpeedRight = InputVM.WheelsRight;

                //Scaling
                if (ParabolicScaling) //Squares the value (0..1)
                {
                    CurrentRawControllerSpeedLeft *= CurrentRawControllerSpeedLeft * (CurrentRawControllerSpeedLeft >= 0 ? 1 : -1);
                    CurrentRawControllerSpeedRight *= CurrentRawControllerSpeedRight * (CurrentRawControllerSpeedRight >= 0 ? 1 : -1);
                }

                float speedLimitFactor = (float)SpeedLimit / motorRangeFactor;
                if (speedLimitFactor > 1) speedLimitFactor = 1;
                if (speedLimitFactor < 0) speedLimitFactor = 0;
                CurrentRawControllerSpeedLeft *= speedLimitFactor;
                CurrentRawControllerSpeedRight *= speedLimitFactor;

                newSpeedLeft = (int)(CurrentRawControllerSpeedLeft * motorRangeFactor);
                newSpeedRight = (int)(CurrentRawControllerSpeedRight * motorRangeFactor);
            }
            #endregion

            SpeedLeft = newSpeedLeft;
            _router.Send(_idResolver.GetId("MotorLeftSpeed"), SpeedLeft);

            SpeedRight = newSpeedRight;
            _router.Send(_idResolver.GetId("MotorRightSpeed"), SpeedRight);
        }

        public void ExitMode()
        {
            int speed = 0;
            _router.Send(_idResolver.GetId("MotorLeftSpeed"), speed);
            _router.Send(_idResolver.GetId("MotorRightSpeed"), speed);
        }
    }
}