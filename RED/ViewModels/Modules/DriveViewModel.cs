using Caliburn.Micro;
using RED.Interfaces;
using RED.Interfaces.Input;
using RED.Models.Modules;
using System.Collections.Generic;
using Math = System.Math;

namespace RED.ViewModels.Modules
{
    public class DriveViewModel : PropertyChangedBase, IInputMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;

        public int SpeedLeft
        {
            get
            {
                return _model.SpeedLeft;
            }
            set
            {
                _model.SpeedLeft = value;
                NotifyOfPropertyChange(() => SpeedLeft);
            }
        }
        public int SpeedRight
        {
            get
            {
                return _model.SpeedRight;
            }
            set
            {
                _model.SpeedRight = value;
                NotifyOfPropertyChange(() => SpeedRight);
            }
        }

        public string Name { get; }
        public string ModeType { get; }

        public int SpeedLimit
        {
            get
            {
                return _model.SpeedLimit;
            }
            set
            {
                _model.SpeedLimit = value;
                NotifyOfPropertyChange(() => SpeedLimit);
            }
        }

        public bool UseLegacyDataIds
        {
            get
            {
                return _model.UseLegacyDataIds;
            }
            set
            {
                _model.UseLegacyDataIds = value;
            }
        }

        public DriveViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver)
        {
            _model = new DriveModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
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
                commandLeft = -1F * r * ScaleVector(theta - (float)Math.PI / 2);
                commandRight = r * ScaleVector(theta);
            }
            else
            {
                commandLeft = values["WheelsLeft"];
                commandRight = values["WheelsRight"];
            }

            float throttle = values.ContainsKey("Throttle") ? values["Throttle"] : 1.0f;

            float speedLimitFactor = (float)SpeedLimit / motorRangeFactor * throttle;
            if (speedLimitFactor > 1F) speedLimitFactor = 1F;
            if (speedLimitFactor < 0F) speedLimitFactor = 0F;

            int newSpeedLeft = (int)(commandLeft * speedLimitFactor * motorRangeFactor);
            int newSpeedRight = (int)(commandRight * speedLimitFactor * motorRangeFactor);

            SpeedLeft = newSpeedLeft;
            SpeedRight = newSpeedRight;

            SendSpeeds(false);
        }

        private void SendSpeeds(bool reliable)
        {
            if (UseLegacyDataIds)
            {
                _rovecomm.SendCommand(_idResolver.GetId("MotorLeftSpeed"), SpeedLeft, reliable);
                _rovecomm.SendCommand(_idResolver.GetId("MotorRightSpeed"), SpeedRight, reliable);
            }
            else
            {
                _rovecomm.SendCommand(_idResolver.GetId("DriveLeftRight"), (ushort)SpeedLeft << 16 | (ushort)SpeedRight, reliable);
            }
        }

        private float ScaleVector(float theta)
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
            SendSpeeds(true);
        }
    }
}