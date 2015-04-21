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

        private ArmAction currentAction;
        public ArmAction CurrentAction
        {
            get { return currentAction; }
            set
            {
                currentAction = value;
                //NotifyOfPropertyChange(() => CurrentAction);
                //NotifyOfPropertyChange(() => CurrentActionDisplay);
            }
        }
        public string CurrentActionDisplay
        {
            get
            {
                var mode = CurrentAction;
                return Enum.GetName(typeof(ArmAction), mode);
            }
        }

        public enum ArmAction : byte
        {
            WristCounterclockwise = 0,
            WristClockwise = 1,
            WristDown = 2,
            WristUp = 3,
            ElbowCounterclockwise = 4,
            ElbowClockwise = 5,
            ElbowDown = 6,
            ElbowUp = 7,
            BaseActuatorBackward = 8,
            BaseActuatorForward = 9,
            BaseServoCounterclockwise = 10,
            BaseServoClockwise = 11,
            Reset = 12,
            Idle = Byte.MaxValue
        }

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

            CurrentAction = ArmAction.Idle;

            if (InputVM.ButtonY)
            {
                CurrentAction = ArmAction.Reset;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmReset"), (UInt16)(0));
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }
            if (InputVM.JoyStick2X < 0)
            {
                CurrentAction = ArmAction.WristCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristCounterclockwise"), (UInt16)(-InputVM.JoyStick2X * 1024));
            }
            else if (InputVM.JoyStick2X > 0)
            {
                CurrentAction = ArmAction.WristClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (UInt16)(InputVM.JoyStick2X * 1024));
            }
            else if (InputVM.JoyStick2Y < 0)
            {
                CurrentAction = ArmAction.WristDown;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristDown"), (UInt16)(InputVM.JoyStick2Y * 1024));
            }
            else if (InputVM.JoyStick2Y > 0)
            {
                CurrentAction = ArmAction.WristUp;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (UInt16)(InputVM.JoyStick2Y * 1024));
            }

            if (InputVM.JoyStick1X < 0)
            {
                CurrentAction = ArmAction.ElbowCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowCounterclockwise"), (UInt16)(InputVM.JoyStick1X * 1024));
            }
            else if (InputVM.JoyStick1X > 0)
            {
                CurrentAction = ArmAction.ElbowClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (UInt16)(InputVM.JoyStick1X * 1024));
            }
            else if (InputVM.JoyStick1Y < 0)
            {
                CurrentAction = ArmAction.ElbowDown;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowDown"), (UInt16)(InputVM.JoyStick1Y * 1024));
            }
            else if (InputVM.JoyStick1Y > 0)
            {
                CurrentAction = ArmAction.ElbowUp;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (UInt16)(InputVM.JoyStick1Y * 1024));
            }

            if (InputVM.DPadL)
            {
                CurrentAction = ArmAction.BaseServoCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoCounterclockwise"), (UInt16)(BaseServoSpeed * 1024));
            }
            else if (InputVM.DPadR)
            {
                CurrentAction = ArmAction.BaseServoClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (UInt16)(BaseServoSpeed * 1024));
            }

            if (InputVM.DPadU)
            {
                CurrentAction = ArmAction.BaseActuatorForward;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (UInt16)(BaseActuatorSpeed * 1024));
            }
            else if (InputVM.DPadD)
            {
                CurrentAction = ArmAction.BaseActuatorBackward;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorBackward"), (UInt16)(BaseActuatorSpeed * 1024));
            }
        }

        public void ExitMode()
        {

        }
    }
}
