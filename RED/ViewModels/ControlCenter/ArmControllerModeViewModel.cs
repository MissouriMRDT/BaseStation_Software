using Caliburn.Micro;
using RED.Interfaces;
using RED.Models;
using RED.ViewModels.ControlCenter;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.ControlCenter
{
    public class ArmControllerModeViewModel : PropertyChangedBase, IControllerMode
    {
        private const short motorRangeFactor = 1000;
        private readonly EndEffectorModes[] AvailibleEndEffectorModes = { EndEffectorModes.Gripper, EndEffectorModes.Drill };

        private readonly ArmControllerModeModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        public string Name { get; set; }
        public InputViewModel InputVM { get; set; }

        public const float BaseServoSpeed = .5f;
        public const int BaseActuatorSpeed = 127;

        public int CurrentEndEffectorMode
        {
            get
            {
                return _model.CurrentEndEffectorMode;
            }
            set
            {
                _model.CurrentEndEffectorMode = value;
                NotifyOfPropertyChange(() => CurrentEndEffectorMode);
            }
        }

        public ArmControllerModeViewModel(InputViewModel inputVM, ControlCenterViewModel cc)
        {
            _model = new ArmControllerModeModel();
            InputVM = inputVM;
            _controlCenter = cc;
            Name = "Arm";
            CurrentEndEffectorMode = 0;
        }

        public void EnterMode()
        {

        }

        public void EvaluateMode()
        {
            Controller c = InputVM.ControllerOne;
            if (c != null && !c.IsConnected) return;

            if (InputVM.DebouncedButtonY)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmStop"), (Int16)(0));
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }

            if (InputVM.DebouncedButtonX)
                PreviousEndeffectorMode();
            else if (InputVM.DebouncedButtonB)
                NextEndeffectorMode();

            var angle = Math.Atan2(InputVM.JoyStick2Y, InputVM.JoyStick2X);
            if (angle > -Math.PI / 6 && angle < Math.PI / 6) //Joystick Right
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (Int16)(InputVM.JoyStick2X * motorRangeFactor));
            else if (angle > Math.PI / 3 && angle < 2 * Math.PI / 3) //Joystick Up
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(-InputVM.JoyStick2Y * motorRangeFactor));
            else if (angle > 5 * Math.PI / 6 || angle < -5 * Math.PI / 6) //Joystick Left
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (Int16)(InputVM.JoyStick2X * motorRangeFactor));
            else if (angle > -2 * Math.PI / 3 && angle < Math.PI / 3) //Joystick Down
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(-InputVM.JoyStick2Y * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(0));

            angle = Math.Atan2(InputVM.JoyStick1Y, InputVM.JoyStick1X);
            if (angle > -Math.PI / 6 && angle < Math.PI / 6) //Joystick Right
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (Int16)(InputVM.JoyStick1X * motorRangeFactor));
            else if (angle > Math.PI / 3 && angle < 2 * Math.PI / 3) //Joystick Up
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (Int16)(-InputVM.JoyStick1Y * motorRangeFactor));
            else if (angle > 5 * Math.PI / 6 || angle < -5 * Math.PI / 6) //Joystick Left
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (Int16)(InputVM.JoyStick1X * motorRangeFactor));
            else if (angle > -2 * Math.PI / 3 && angle < Math.PI / 3) //Joystick Down
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (Int16)(-InputVM.JoyStick1Y * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(0));

            if (InputVM.DPadU)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(BaseActuatorSpeed));
            else if (InputVM.DPadD)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(-BaseActuatorSpeed));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(0));
            if (InputVM.DPadR)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(BaseServoSpeed / 10f * motorRangeFactor));
            else if (InputVM.DPadL)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(-BaseServoSpeed / 10f * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(0));

            switch (AvailibleEndEffectorModes[CurrentEndEffectorMode])
            {
                case EndEffectorModes.Gripper:
                    if (InputVM.RightTrigger > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(InputVM.RightTrigger * motorRangeFactor));
                    else if (InputVM.LeftTrigger > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(-InputVM.LeftTrigger * motorRangeFactor));
                    else
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(0));
                    break;
                case EndEffectorModes.Drill:
                    int drillCmd, actCmd;
                    if (InputVM.ButtonRb)
                        drillCmd = (int)DrillCommands.Forward;
                    else if (InputVM.ButtonLb)
                        drillCmd = (int)DrillCommands.Reverse;
                    else
                        drillCmd = (int)DrillCommands.Stop;
                    if (InputVM.RightTrigger > 0)
                        actCmd = (int)DrillCommands.Forward;
                    else if (InputVM.LeftTrigger > 0)
                        actCmd = (int)DrillCommands.Reverse;
                    else
                        actCmd = (int)DrillCommands.Stop;

                    byte drillActCombinedCmd8Bit = (byte)(actCmd << 4 | drillCmd);
                    var drillpacket = new byte[4] { drillActCombinedCmd8Bit, drillActCombinedCmd8Bit, drillActCombinedCmd8Bit, drillActCombinedCmd8Bit };
                    //Int16 drillActCombinedCmd16Bit = (Int16)(drillActCombinedCmd8Bit << 8 | drillActCombinedCmd8Bit);
                    //var drillpacket = new Int16[2] { drillActCombinedCmd16Bit, drillActCombinedCmd16Bit };

                    _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("DrillAndActuator"), drillpacket);
                    break;
            }
        }

        public void ExitMode()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmStop"), (Int16)(0));
        }

        public void NextEndeffectorMode()
        {
            CurrentEndEffectorMode = (CurrentEndEffectorMode + 1) % AvailibleEndEffectorModes.Length;
            _controlCenter.Console.WriteToConsole("Switched to Next Endeffector Mode");
        }
        public void PreviousEndeffectorMode()
        {
            CurrentEndEffectorMode = (CurrentEndEffectorMode + 1 + AvailibleEndEffectorModes.Length) % AvailibleEndEffectorModes.Length;
            _controlCenter.Console.WriteToConsole("Switched to Previous Endeffector Mode");
        }
    }

    public enum EndEffectorModes
    {
        None = 0,
        Gripper = 1,
        Drill = 2
    }

    public enum DrillCommands : short
    {
        Stop = 0,
        Forward = 1,
        Reverse = 2
    }
}