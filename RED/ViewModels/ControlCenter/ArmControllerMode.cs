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
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }
            if (InputVM.JoyStick2X < 0)
            {
                CurrentAction = ArmAction.WristCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick2X > 0)
            {
                CurrentAction = ArmAction.WristClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick2Y < 0)
            {
                CurrentAction = ArmAction.WristDown;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick2Y > 0)
            {
                CurrentAction = ArmAction.WristUp;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }

            if (InputVM.JoyStick1X < 0)
            {
                CurrentAction = ArmAction.ElbowCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick1X > 0)
            {
                CurrentAction = ArmAction.ElbowClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick1Y < 0)
            {
                CurrentAction = ArmAction.ElbowDown;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.JoyStick1Y > 0)
            {
                CurrentAction = ArmAction.ElbowUp;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }

            if (InputVM.DPadL)
            {
                CurrentAction = ArmAction.BaseServoCounterclockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.DPadR)
            {
                CurrentAction = ArmAction.BaseServoClockwise;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }

            if (InputVM.DPadU)
            {
                CurrentAction = ArmAction.BaseActuatorForward;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
            else if (InputVM.DPadD)
            {
                CurrentAction = ArmAction.BaseActuatorBackward;
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Arm"), (byte)CurrentAction);
            }
        }

        public void ExitMode()
        {

        }
    }
}
