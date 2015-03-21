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
        private readonly ControlCenterViewModel _controlCenter;

        private int speedLeft = 128;
        private int speedRight = 128;
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

                newSpeedLeft = (int)(CurrentRawControllerSpeedLeft * 128) + 128;
                newSpeedRight = (int)(CurrentRawControllerSpeedRight * 128) + 128;
            }
            #endregion

            if (newSpeedLeft == 128)
            {
                speedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, speedLeft);
            }
            else if (newSpeedLeft != speedLeft)
            {
                if (!isFullSpeed)
                {
                    if (newSpeedLeft > 150)
                        speedLeft = 150;
                    else if (newSpeedLeft < 106)
                        speedLeft = 106;
                    else
                        speedLeft = newSpeedLeft;
                }
                else
                {
                    speedLeft = newSpeedLeft;
                }
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, speedLeft);
            }
            if (newSpeedRight == 128)
            {
                speedRight = newSpeedRight;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, speedRight);
            }
            else if (newSpeedRight != speedRight)
            {
                if (!isFullSpeed)
                {
                    if (newSpeedRight > 150)
                        speedRight = 150;
                    else if (newSpeedRight < 106)
                        speedRight = 106;
                    else
                        speedRight = newSpeedRight;
                }
                else
                {
                    speedRight = newSpeedRight;
                }
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorCommandedSpeedRight").Id, speedRight);
            }
        }

        public void ExitMode()
        {

        }
    }
}