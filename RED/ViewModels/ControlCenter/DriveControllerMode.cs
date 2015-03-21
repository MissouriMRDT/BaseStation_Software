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

        private int CurrentRawControllerSpeedLeft;
        private int CurrentRawControllerSpeedRight;
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

            #region Normalization of joystick input
            var LX = InputVM.JoyStick1X;
            var LY = InputVM.JoyStick1Y;
            var leftMagnitude = (float)Math.Sqrt(LX * LX + LY * LY);

            if (leftMagnitude > 32767) leftMagnitude = 32767; //clip the magnitude at its expected maximum value
            if (leftMagnitude < Gamepad.LeftThumbDeadZone) leftMagnitude = 0; //if the controller is in the deadzone zero out the magnitude
            else leftMagnitude -= Gamepad.LeftThumbDeadZone; //adjust magnitude relative to the end of the dead zone

            var RX = InputVM.JoyStick2X;
            var RY = InputVM.JoyStick2Y;
            var rightMagnitude = (float)Math.Sqrt(RX * RX + RY * RY);

            if (rightMagnitude > 32767) rightMagnitude = 32767; //clip the magnitude at its expected maximum value
            if (rightMagnitude < Gamepad.RightThumbDeadZone)rightMagnitude = 0; //if the controller is in the deadzone zero out the magnitude
            else rightMagnitude -= Gamepad.RightThumbDeadZone; //adjust magnitude relative to the end of the dead zone

            // Update Working Values
            CurrentRawControllerSpeedLeft = (int)((LY < 0) ? -leftMagnitude : leftMagnitude);
            CurrentRawControllerSpeedRight = (int)((RY < 0) ? -rightMagnitude : rightMagnitude);
            #endregion

            var newSpeedLeft = CurrentRawControllerSpeedLeft / 255 + 128;
            var newSpeedRight = CurrentRawControllerSpeedRight / 255 + 128;

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