using Caliburn.Micro;
using Core.Interfaces;
using Core.Models;
using Core.RoveProtocol;
using RED.Models.Modules;
using System;
using System.Collections.Generic;
using System.Net;
using Math = System.Math;

namespace RED.ViewModels.Modules
{
    public class DriveViewModel : PropertyChangedBase, IInputMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveModel _model;
        private readonly IRovecomm _rovecomm;
        private readonly IDataIdResolver _idResolver;
        private readonly ILogger _log;

        public short SpeedLeft
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
        public short SpeedRight
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

		public string Channel {
			get {
				return _model.Channel;
			}
			set {
				_model.Channel = value;
				NotifyOfPropertyChange(() => Channel);
			}
		}

		public DriveViewModel(IRovecomm networkMessenger, IDataIdResolver idResolver, ILogger log)
        {
            _model = new DriveModel();
            _rovecomm = networkMessenger;
            _idResolver = idResolver;
            _log = log;
            Name = "Drive";
            ModeType = "Drive";
			Channel = "1";
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

            short newSpeedLeft = (short)(commandLeft * speedLimitFactor * motorRangeFactor);
            short newSpeedRight = (short)(commandRight * speedLimitFactor * motorRangeFactor);

            SpeedLeft = newSpeedLeft;
            SpeedRight = newSpeedRight;

            if (values.ContainsKey("ForwardBump"))
            {
                if (values["ForwardBump"] > 0)
                {
                    SpeedLeft = 50;
                    SpeedRight = 50;
                }
                else if (values["BackwardBump"] > 0)
                {
                    SpeedLeft = -50;
                    SpeedRight = -50;
                }
            }

            SendSpeeds(false);
        }

        private void SendSpeeds(bool reliable)
        {
            short[] sendValues = { IPAddress.HostToNetworkOrder(SpeedLeft), IPAddress.HostToNetworkOrder(SpeedRight) };
            _rovecomm.SendCommand(Packet.Create("DriveLeftRight", sendValues), reliable);
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