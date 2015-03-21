using RED.Interfaces;
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

        private readonly ControlCenterViewModel _controlCenter;

        private int speedLeft;
        private int speedRight;
        private bool isFullSpeed = false;

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public DriveControllerMode(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            InputVM = inputVM;
            _controlCenter = cc;
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

                newSpeedLeft = (int)(CurrentRawControllerSpeedLeft * motorRangeFactor);
                newSpeedRight = (int)(CurrentRawControllerSpeedRight * motorRangeFactor);
            }
            #endregion

            if (newSpeedLeft == 0)
            {
                speedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, "!G " + speedLeft.ToString());
            }
            else if (newSpeedLeft != speedLeft)
            {
                if (isFullSpeed)
                    speedLeft = newSpeedLeft;
                else
                    if (newSpeedLeft > 0.2 * motorRangeFactor)
                        speedLeft = (int)(0.2 * motorRangeFactor);
                    else if (newSpeedLeft < -0.2 * motorRangeFactor)
                        speedLeft = (int)(-0.2 * motorRangeFactor);
                    else
                        speedLeft = newSpeedLeft;

                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, "!G " + speedLeft.ToString());
            }
            if (newSpeedRight == 0)
            {
                speedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, "!G " + speedRight.ToString());
            }
            else if (newSpeedRight != speedRight)
            {
                if (isFullSpeed)
                    speedRight = newSpeedRight;
                else
                    if (newSpeedRight > 0.2 * motorRangeFactor)
                        speedRight = (int)(0.2 * motorRangeFactor);
                    else if (newSpeedRight < -0.2 * motorRangeFactor)
                        speedRight = (int)(-0.2 * motorRangeFactor);
                    else
                        speedRight = newSpeedRight;

                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, "!G " + speedRight.ToString());
            }
        }

        public void ExitMode()
        {

        }
    }
}