using RED.Interfaces;
using RED.Models;
using RED.ViewModels.ControlCenter;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class DriveControllerMode : IControllerMode
    {
        private const int motorRangeFactor = 1000;

        private readonly DriveControllerModeModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        private int speedLeft;
        private int speedRight;

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public int SpeedLimit
        {
            get
            {
                return _model.speedLimit;
            }
            set
            {
                _model.speedLimit = value;
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

        public DriveControllerMode(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            _model = new DriveControllerModeModel();
            _controlCenter = cc;
            InputVM = inputVM;
            Name = "Drive";
        }

        public void EnterMode()
        {
            speedLeft = 0;
            speedRight = 0;
        }

        public void EvaluateMode()
        {
            Controller c = InputVM.ControllerOne;
            if (c != null && !c.IsConnected) return;

            int newSpeedLeft;
            int newSpeedRight;

            #region Normalization of joystick input
            {
                float LX = InputVM.JoyStick1X, LY = InputVM.JoyStick1Y;
                var leftMagnitude = Math.Sqrt(LX * LX + LY * LY);

                float RX = InputVM.JoyStick2X, RY = InputVM.JoyStick2Y;
                var rightMagnitude = Math.Sqrt(RX * RX + RY * RY);

                // Update Working Values
                var CurrentRawControllerSpeedLeft = (LY < 0) ? -leftMagnitude : leftMagnitude;
                var CurrentRawControllerSpeedRight = (RY < 0) ? -rightMagnitude : rightMagnitude;

                //Scaling
                if (ParabolicScaling) //Squares the value (0..1)
                {
                    CurrentRawControllerSpeedLeft *= CurrentRawControllerSpeedLeft;
                    CurrentRawControllerSpeedRight *= CurrentRawControllerSpeedRight;
                }

                double speedLimitFactor = SpeedLimit / motorRangeFactor;
                if (speedLimitFactor > 1) speedLimitFactor = 1;
                if (speedLimitFactor < 0) speedLimitFactor = 0;
                CurrentRawControllerSpeedLeft *= speedLimitFactor;
                CurrentRawControllerSpeedRight *= speedLimitFactor;

                newSpeedLeft = (int)(CurrentRawControllerSpeedLeft * motorRangeFactor);
                newSpeedRight = (int)(CurrentRawControllerSpeedRight * motorRangeFactor);
            }
            #endregion

            if (newSpeedLeft == 0)
            {
                speedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorLeftSpeed"), speedLeft);
            }
            else if (newSpeedLeft != speedLeft)
            {
                speedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorLeftSpeed"), speedLeft);
            }
            if (newSpeedRight == 0)
            {
                speedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorRightSpeed"), speedRight);
            }
            else if (newSpeedRight != speedRight)
            {
                speedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("MotorRightSpeed"), speedRight);
            }
        }

        public void ExitMode()
        {

        }
    }
}