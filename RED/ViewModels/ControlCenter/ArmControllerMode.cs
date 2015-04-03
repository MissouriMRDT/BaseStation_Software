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

        public enum ArmAction
        {
            Idle,
            WristCounterClockwise,
            WristClockwise,
            WristDown,
            WristUp,
            ElbowCounterClockwise,
            ElbowClockwise,
            ElbowDown,
            ElbowUp,
            ActuatorBack,
            ActuatorForward,
            BaseClockwise,
            BaseCounterclockwise
        }
        public enum ArmDirections
        {
            Back = 0,
            Forward = 1,
            Counterclockwise = 0,
            Clockwise = 1,
            Down = 2,
            Up = 3
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

            if (InputVM.ButtonY)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmReset"), 0);
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }

            else if (InputVM.JoyStick2X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWrist"), ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.WristCounterClockwise;
            }
            else if (InputVM.JoyStick2X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWrist"), ArmDirections.Clockwise);
                CurrentAction = ArmAction.WristClockwise;
            }
            else if (InputVM.JoyStick2Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWrist"), ArmDirections.Down);
                CurrentAction = ArmAction.WristDown;
            }
            else if (InputVM.JoyStick2Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWrist"), ArmDirections.Up);
                CurrentAction = ArmAction.WristUp;
            }

            if (InputVM.JoyStick1X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbow"), ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.ElbowCounterClockwise;
            }
            else if (InputVM.JoyStick1X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbow"), ArmDirections.Clockwise);
                CurrentAction = ArmAction.ElbowClockwise;
            }
            else if (InputVM.JoyStick1Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbow"), ArmDirections.Down);
                CurrentAction = ArmAction.ElbowDown;
            }
            else if (InputVM.JoyStick1Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbow"), ArmDirections.Up);
                CurrentAction = ArmAction.ElbowUp;
            }

            if (InputVM.DPadL)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBase"), ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.BaseCounterclockwise;
            }
            else if (InputVM.DPadR)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBase"), ArmDirections.Clockwise);
                CurrentAction = ArmAction.BaseClockwise;
            }
            else if (InputVM.DPadU)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBase"), ArmDirections.Forward);
                CurrentAction = ArmAction.ActuatorForward;
            }
            else if (InputVM.DPadD)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBase"), ArmDirections.Back);
                CurrentAction = ArmAction.ActuatorBack;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }
        }

        public void ExitMode()
        {

        }
    }
}
