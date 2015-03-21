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
            if (leftMagnitude > Gamepad.LeftThumbDeadZone)
            {
                //clip the magnitude at its expected maximum value
                if (leftMagnitude > 32767) leftMagnitude = 32767;

                //adjust magnitude relative to the end of the dead zone
                leftMagnitude -= Gamepad.LeftThumbDeadZone;
            }
            else //if the controller is in the deadzone zero out the magnitude
            {
                leftMagnitude = 0;
            }

            var RX = InputVM.JoyStick2X;
            var RY = InputVM.JoyStick2Y;
            var rightMagnitude = (float)Math.Sqrt(RX * RX + RY * RY);
            if (rightMagnitude > Gamepad.RightThumbDeadZone)
            {
                //clip the magnitude at its expected maximum value
                if (rightMagnitude > 32767) rightMagnitude = 32767;

                //adjust magnitude relative aoeuaoeuaoeuto the end of the dead zone
                rightMagnitude -= Gamepad.RightThumbDeadZone;
            }
            else //if the controller is in the deadzone zero out the magnitude
            {
                rightMagnitude = 0;
            }

            // Update Working Values
            if (LY < 0)
            {
                CurrentRawControllerSpeedLeft = (int)-leftMagnitude;
            }
            else
            {
                CurrentRawControllerSpeedLeft = (int)leftMagnitude;
            }
            if (RY < 0)
            {
                CurrentRawControllerSpeedRight = (int)-rightMagnitude;
            }
            else
            {
                CurrentRawControllerSpeedRight = (int)rightMagnitude;
            }
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