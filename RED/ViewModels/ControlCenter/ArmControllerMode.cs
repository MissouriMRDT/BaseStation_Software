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

        public ArmControllerMode(InputViewModel inputVM, ControlCenterViewModel cc)
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

            if (InputVM.ButtonY)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmReset").Id, 0);
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
                return;
            }

            if (InputVM.JoyStick2X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.WristCounterClockwise;
                return;
            }
            else if (InputVM.JoyStick2X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, ArmDirections.Clockwise);
                CurrentAction = ArmAction.WristClockwise;
                return;
            }
            else if (InputVM.JoyStick2Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, ArmDirections.Down);
                CurrentAction = ArmAction.WristDown;
                return;
            }
            else if (InputVM.JoyStick2Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmWrist").Id, ArmDirections.Up);
                CurrentAction = ArmAction.WristUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if (InputVM.JoyStick1X < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.ElbowCounterClockwise;
                return;
            }
            else if (InputVM.JoyStick1X > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, ArmDirections.Clockwise);
                CurrentAction = ArmAction.ElbowClockwise;
                return;
            }
            else if (InputVM.JoyStick1Y < 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, ArmDirections.Down);
                CurrentAction = ArmAction.ElbowDown;
                return;
            }
            else if (InputVM.JoyStick1Y > 0)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmElbow").Id, ArmDirections.Up);
                CurrentAction = ArmAction.ElbowUp;
                return;
            }
            else
            {
                CurrentAction = ArmAction.Idle;
            }

            if (InputVM.DPadL)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, ArmDirections.Counterclockwise);
                CurrentAction = ArmAction.BaseCounterclockwise;
            }
            else if (InputVM.DPadR)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, ArmDirections.Clockwise);
                CurrentAction = ArmAction.BaseClockwise;
            }
            else if (InputVM.DPadU)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, ArmDirections.Forward);
                CurrentAction = ArmAction.ActuatorForward;
            }
            else if (InputVM.DPadD)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetCommand("ArmBase").Id, ArmDirections.Back);
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
