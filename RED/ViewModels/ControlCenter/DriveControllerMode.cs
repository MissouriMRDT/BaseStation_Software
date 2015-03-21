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

        private int speedLeft;
        private int speedRight;

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

            var newSpeedLeft = InputVM.CurrentRawControllerSpeedLeft / 255 + 128;
            var newSpeedRight = InputVM.CurrentRawControllerSpeedRight / 255 + 128;

            if (newSpeedLeft == 128)
            {
                speedLeft = newSpeedLeft;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("MotorLeftSpeed").Id, speedLeft);
            }
            else if (newSpeedLeft != speedLeft)
            {
                if (!InputVM.IsFullSpeed)
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
                if (!InputVM.IsFullSpeed)
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