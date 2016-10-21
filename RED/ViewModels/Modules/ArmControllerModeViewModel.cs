using Caliburn.Micro;
using RED.Interfaces;
using RED.Models.Modules;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RED.ViewModels.Modules
{
    public class ArmControllerModeViewModel : PropertyChangedBase, IControllerMode, ISubscribe
    {
        private const byte ArmDisableCommand = 0x00;
        private const byte ArmEnableCommand = 0x01;

        private const short motorRangeFactor = 1000;
        private readonly EndEffectorModes[] AvailibleEndEffectorModes = { EndEffectorModes.Gripper, EndEffectorModes.Drill, EndEffectorModes.RegulatorDetach };
        private readonly string[] EndEffectorModeNames = { "Gripper", "Drill", "Regulator Detachment" };

        private readonly ArmControllerModeModel _model;
        private readonly ControlCenterViewModel _controlCenter;

        public string Name { get; set; }
        public IInputDevice InputVM { get; set; }

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
                NotifyOfPropertyChange(() => CurrentEndEffectorModeName);
            }
        }
        public string CurrentEndEffectorModeName
        {
            get
            {
                return EndEffectorModeNames[CurrentEndEffectorMode];
            }
        }

        public float AngleJ1
        {
            get
            {
                return _model.AngleJ1;
            }
            set
            {
                _model.AngleJ1 = value;
                NotifyOfPropertyChange(() => AngleJ1);
            }
        }
        public float AngleJ2
        {
            get
            {
                return _model.AngleJ2;
            }
            set
            {
                _model.AngleJ2 = value;
                NotifyOfPropertyChange(() => AngleJ2);
            }
        }
        public float AngleJ3
        {
            get
            {
                return _model.AngleJ3;
            }
            set
            {
                _model.AngleJ3 = value;
                NotifyOfPropertyChange(() => AngleJ3);
            }
        }
        public float AngleJ4
        {
            get
            {
                return _model.AngleJ4;
            }
            set
            {
                _model.AngleJ4 = value;
                NotifyOfPropertyChange(() => AngleJ4);
            }
        }
        public float AngleJ5
        {
            get
            {
                return _model.AngleJ5;
            }
            set
            {
                _model.AngleJ5 = value;
                NotifyOfPropertyChange(() => AngleJ5);
            }
        }
        public float AngleJ6
        {
            get
            {
                return _model.AngleJ6;
            }
            set
            {
                _model.AngleJ6 = value;
                NotifyOfPropertyChange(() => AngleJ6);
            }
        }

        public int EndeffectorSpeedLimit
        {
            get
            {
                return _model.EndeffectorSpeedLimit;
            }
            set
            {
                _model.EndeffectorSpeedLimit = value;
                NotifyOfPropertyChange(() => EndeffectorSpeedLimit);
            }
        }

        public ArmControllerModeViewModel(IInputDevice inputVM, ControlCenterViewModel cc)
        {
            _model = new ArmControllerModeModel();
            InputVM = inputVM;
            _controlCenter = cc;
            Name = "Arm";
            CurrentEndEffectorMode = 0;

            _controlCenter.DataRouter.Subscribe(this, _controlCenter.MetadataManager.GetId("ArmCurrentPosition"));
        }

        public void ReceiveFromRouter(ushort dataId, byte[] data)
        {
            switch (_controlCenter.MetadataManager.GetTelemetry(dataId).Name)
            {
                case "ArmCurrentPosition":
                    AngleJ1 = BitConverter.ToSingle(data, 0);
                    AngleJ2 = BitConverter.ToSingle(data, 4);
                    AngleJ3 = BitConverter.ToSingle(data, 8);
                    AngleJ4 = BitConverter.ToSingle(data, 12);
                    AngleJ5 = BitConverter.ToSingle(data, 16);
                    AngleJ6 = BitConverter.ToSingle(data, 20);
                    break;
            }
        }

        public void EnterMode()
        {

        }

        public void EvaluateMode()
        {
            if (InputVM.DebouncedArmReset)
            {
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmStop"), (Int16)(0));
                _controlCenter.Console.WriteToConsole("Robotic Arm Resetting...");
            }

            if (InputVM.DebouncedToolPrev)
                PreviousEndeffectorMode();
            else if (InputVM.DebouncedToolNext)
                NextEndeffectorMode();

            var angle = Math.Atan2(InputVM.WristBend, InputVM.WristTwist);
            if (angle > -Math.PI / 6 && angle < Math.PI / 6) //Joystick Right
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (Int16)(InputVM.WristTwist * motorRangeFactor));
            else if (angle > Math.PI / 3 && angle < 2 * Math.PI / 3) //Joystick Up
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(-InputVM.WristBend * motorRangeFactor));
            else if (angle > 5 * Math.PI / 6 || angle < -5 * Math.PI / 6) //Joystick Left
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristClockwise"), (Int16)(InputVM.WristTwist * motorRangeFactor));
            else if (angle > -2 * Math.PI / 3 && angle < Math.PI / 3) //Joystick Down
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(-InputVM.WristBend * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(0));

            angle = Math.Atan2(InputVM.ElbowBend, InputVM.ElbowTwist);
            if (angle > -Math.PI / 6 && angle < Math.PI / 6) //Joystick Right
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (Int16)(InputVM.ElbowTwist * motorRangeFactor));
            else if (angle > Math.PI / 3 && angle < 2 * Math.PI / 3) //Joystick Up
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (Int16)(-InputVM.ElbowBend * motorRangeFactor));
            else if (angle > 5 * Math.PI / 6 || angle < -5 * Math.PI / 6) //Joystick Left
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowClockwise"), (Int16)(InputVM.ElbowTwist * motorRangeFactor));
            else if (angle > -2 * Math.PI / 3 && angle < Math.PI / 3) //Joystick Down
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmElbowUp"), (Int16)(-InputVM.ElbowBend * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmWristUp"), (Int16)(0));

            if (InputVM.ActuatorForward)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(BaseActuatorSpeed));
            else if (InputVM.ActuatorBackward)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(-BaseActuatorSpeed));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseActuatorForward"), (Int16)(0));
            if (InputVM.BaseClockwise)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(BaseServoSpeed / 10f * motorRangeFactor));
            else if (InputVM.BaseCounterClockwise)
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(-BaseServoSpeed / 10f * motorRangeFactor));
            else
                _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmBaseServoClockwise"), (Int16)(0));

            switch (AvailibleEndEffectorModes[CurrentEndEffectorMode])
            {
                case EndEffectorModes.Gripper:
                    if (InputVM.GripperClose > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(InputVM.GripperClose * EndeffectorSpeedLimit));
                    else if (InputVM.GripperOpen > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(-InputVM.GripperOpen * EndeffectorSpeedLimit));
                    else
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(0));
                    break;
                case EndEffectorModes.Drill:
                    if (InputVM.DrillClockwise)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Drill"), (short)DrillCommands.Forward);
                    else if (InputVM.DrillCounterClockwise)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Drill"), (short)DrillCommands.Reverse);
                    else
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Drill"), (short)DrillCommands.Stop);
                    break;

                case EndEffectorModes.RegulatorDetach:
                    if (InputVM.GripperClose > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(InputVM.GripperClose * EndeffectorSpeedLimit));
                    else if (InputVM.GripperOpen > 0)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(-InputVM.GripperOpen * EndeffectorSpeedLimit));
                    else
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("Gripper"), (Int16)(0));

                    if (InputVM.DrillClockwise)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("RegulatorDetach"), (short)DrillCommands.Forward);
                    else if (InputVM.DrillCounterClockwise)
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("RegulatorDetach"), (short)DrillCommands.Reverse);
                    else
                        _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("RegulatorDetach"), (short)DrillCommands.Stop);
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

        public void EnableAll()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableAll"), ArmEnableCommand);
        }
        public void DisableAll()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableAll"), ArmDisableCommand);
        }
        public void EnableMain()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableMain"), ArmEnableCommand);
        }
        public void DisableMain()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableMain"), ArmDisableCommand);
        }
        public void EnableJ1()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ1"), ArmEnableCommand);
        }
        public void DisableJ1()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ1"), ArmDisableCommand);
        }
        public void EnableJ2()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ2"), ArmEnableCommand);
        }
        public void DisableJ2()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ2"), ArmDisableCommand);
        }
        public void EnableJ34()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ34"), ArmEnableCommand);
        }
        public void DisableJ34()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ34"), ArmDisableCommand);
        }
        public void EnableJ56()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ56"), ArmEnableCommand);
        }
        public void DisableJ56()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableJ56"), ArmDisableCommand);
        }
        public void EnableEndeff()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableEndeff"), ArmEnableCommand);
        }
        public void DisableEndeff()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmEnableEndeff"), ArmDisableCommand);
        }

        public void GetPosition()
        {
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmGetPosition"), new byte[0]);
        }
        public void SetPosition()
        {
            float[] angles = { AngleJ1, AngleJ2, AngleJ3, AngleJ4, AngleJ5, AngleJ6 };
            byte[] data = new byte[angles.Length * sizeof(float)];
            Buffer.BlockCopy(angles, 0, data, 0, data.Length);
            _controlCenter.DataRouter.Send(_controlCenter.MetadataManager.GetId("ArmAbsoluteAngle"), data);
        }
    }

    public enum EndEffectorModes
    {
        None = 0,
        Gripper = 1,
        Drill = 2,
        RegulatorDetach = 3
    }

    public enum DrillCommands : short
    {
        Stop = 0,
        Forward = 1,
        Reverse = 2
    }
}