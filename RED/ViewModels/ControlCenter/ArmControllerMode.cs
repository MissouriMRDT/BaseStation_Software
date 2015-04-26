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
    public class ArmControllerMode : IControllerMode
    {
        private readonly ControlCenterViewModel _controlCenter;

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public const float BaseServoSpeed = .5f;
        public const float BaseActuatorSpeed = .5f;

        public ArmControllerMode(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            InputVM = inputVM;
            _controlCenter = cc;
            Name = "Arm";
        }

        public void EnterMode()
        {

        }

        public void EvaluateMode()
        {
            Controller c = InputVM.ControllerOne;
            if (c != null && !c.IsConnected) return;


            if (InputVM.ButtonY)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmStop"), (Int16)(0));
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (Int16)(InputVM.JoyStick2X * 1024));
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(InputVM.JoyStick2Y * 1024));

            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (Int16)(InputVM.JoyStick1X * 1024));
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (Int16)(InputVM.JoyStick1Y * 1024));

            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(BaseActuatorSpeed * 1024));
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(BaseServoSpeed * 1024));
        }

        public void ExitMode()
        {

        }
    }
}