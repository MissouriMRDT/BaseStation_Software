using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using Math = System.Math;
using System.Collections.Generic;

namespace RED.ViewModels.Modules
{
    public class DriveViewModel : PropertyChangedBase, IInputMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveModel _model;
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

        public string Name { get; private set; }
        public string ModeType { get; private set; }
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

        public bool UseLegacyDataIds
        {
            get
            {
                return _model.useLegacyDataIds;
            }
            set
            {
                _model.useLegacyDataIds = value;
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

        public DriveViewModel(IInputDevice inputVM, IDataRouter router, IDataIdResolver idResolver)
        {
            _model = new DriveModel();
            _router = router;
            _idResolver = idResolver;
            InputVM = inputVM;
            Name = "Drive";
            ModeType = "Drive";
        }

        public void StartMode()
        {
            SpeedLeft = 0;
            SpeedRight = 0;
        }

        public void SetValues(Dictionary<string, float> values)
        {
            float commandLeft, commandRight;

            if (values.ContainsKey("VectorX") && values.ContainsKey("VectorY"))
            {
                float x = values["VectorX"];
                float y = values["VectorY"];
                float theta = (float)Math.Atan2(y, x);
                float r = (float)Math.Sqrt(x * x + y * y) / (float)Math.Min(Math.Abs(Math.Pow(Math.Sin(theta), -1.0)), Math.Abs(Math.Pow(Math.Cos(theta), -1.0)));
                commandLeft = -1F * r * scaleVector(theta - (float)Math.PI / 2);
                commandRight = r * scaleVector(theta);
            }
            else
            {
                commandLeft = values["WheelsLeft"];
                commandRight = values["WheelsRight"];
            }

            float speedLimitFactor = (float)SpeedLimit / motorRangeFactor;
            if (speedLimitFactor > 1F) speedLimitFactor = 1F;
            if (speedLimitFactor < 0F) speedLimitFactor = 0F;

            int newSpeedLeft = (int)(commandLeft * speedLimitFactor * motorRangeFactor);
            int newSpeedRight = (int)(commandRight * speedLimitFactor * motorRangeFactor);

            SpeedLeft = newSpeedLeft;
            SpeedRight = newSpeedRight;

            SendSpeeds();
        }

        private void SendSpeeds()
        {
            if (UseLegacyDataIds)
            {
                _router.Send(_idResolver.GetId("MotorLeftSpeed"), SpeedLeft);
                _router.Send(_idResolver.GetId("MotorRightSpeed"), SpeedRight);
            }
            else
            {
                _router.Send(_idResolver.GetId("DriveLeftRight"), (ushort)SpeedLeft << 16 | (ushort)SpeedRight);
            }
        }

        private float scaleVector(float theta)
        {
            float pi = (float)Math.PI;
            if (theta < -pi) theta += 2 * pi;

            if (theta >= 0 && theta <= pi / 2) return 4F * theta / pi - 1F;
            else if (theta >= pi / 2 && theta <= pi) return 1F;
            else if (theta >= -pi && theta <= -pi / 2) return -4F / pi * (theta + 5F / 4F * pi) + 2F;
            else if (theta >= -pi / 2 && theta <= 0) return -1F;
            throw new System.ArgumentOutOfRangeException("Theta value is out of range.");
        }

        public void StopMode()
        {
            SpeedLeft = SpeedRight = 0;
            SendSpeeds();
        }
    }
}