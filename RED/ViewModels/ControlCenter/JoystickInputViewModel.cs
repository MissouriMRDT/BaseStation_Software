namespace RED.ViewModels.ControlCenter
{
    using Annotations;
    using Caliburn.Micro;
    using Interfaces;
    using Models;
    using SharpDX.DirectInput;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Timers;
    using System.Threading.Tasks;

    public class JoystickInputViewModel : PropertyChangedBase, IInputDevice
    {
        private readonly JoystickInputModel Model = new JoystickInputModel();
        private readonly ControlCenterViewModel _controlCenter;
        [CanBeNull]
        // Initialize DirectInput
        private readonly DirectInput directInput = new DirectInput();

        // Find a Joystick Guid
        private readonly Guid joystickGuid = Guid.Empty;

        private readonly Joystick joystick;


        public bool AutoDeadzone
        {
            get
            {
                return Model.AutoDeadzone;
            }
            set
            {
                Model.AutoDeadzone = value;
                NotifyOfPropertyChange(() => AutoDeadzone);
            }
        }
        public int ManualDeadzone
        {
            get
            {
                return Model.ManualDeadzone;
            }
            set
            {
                Model.ManualDeadzone = value;
                NotifyOfPropertyChange(() => ManualDeadzone);
            }
        }

        #region Controller Display Values
        public bool Connected
        {
            get
            {
                return Model.Connected;
            }
            set
            {
                Model.Connected = value;
                NotifyOfPropertyChange(() => Connected);
                _controlCenter.StateManager.ControllerIsConnected = value;
                NotifyOfPropertyChange(() => ConnectionStatus);
            }
        }
        public string ConnectionStatus
        {
            get
            {
                return !Connected ? "Disconnected" : "Connected";
            }
        }
        public float WheelsLeft
        {
            get
            {
                return Model.WheelsLeft;
            }
            set
            {
                Model.WheelsLeft = value;
                NotifyOfPropertyChange(() => WheelsLeft);
            }
        }
        public float WheelsRight
        {
            get
            {
                return Model.WheelsRight;
            }
            set
            {
                Model.WheelsRight = value;
                NotifyOfPropertyChange(() => WheelsRight);
            }
        }
        public float WristTwist
        {
            get
            {
                return Model.WristTwist;
            }
            set
            {
                Model.WristTwist = value;
                NotifyOfPropertyChange(() => WristTwist);
            }
        }
        public float WristBend
        {
            get
            {
                return Model.WristBend;
            }
            set
            {
                Model.WristBend = value;
                NotifyOfPropertyChange(() => WristBend);
            }
        }
        public float ElbowBend
        {
            get
            {
                return Model.ElbowBend;
            }
            set
            {
                Model.ElbowBend = value;
                NotifyOfPropertyChange(() => ElbowBend);
            }
        }
        public float ElbowTwist
        {
            get
            {
                return Model.ElbowTwist;
            }
            set
            {
                Model.ElbowTwist = value;
                NotifyOfPropertyChange(() => ElbowTwist);
            }
        }
        public float GripperOpen
        {
            get
            {
                return Model.GripperOpen;
            }
            set
            {
                Model.GripperOpen = value;
                NotifyOfPropertyChange(() => GripperOpen);
            }
        }
        public float GripperClose
        {
            get
            {
                return Model.GripperClose;
            }
            set
            {
                Model.GripperClose = value;
                NotifyOfPropertyChange(() => GripperClose);
            }
        }
        public bool ButtonA
        {
            get
            {
                return Model.ButtonA;
            }
            set
            {
                DebouncedButtonA = !Model.ButtonA && value;
                Model.ButtonA = value;
                NotifyOfPropertyChange(() => ButtonA);
            }
        }
        public bool ToolNext
        {
            get
            {
                return Model.ToolNext;
            }
            set
            {
                DebouncedToolNext = !Model.ToolNext && value;
                Model.ToolNext = value;
                NotifyOfPropertyChange(() => ToolNext);
            }
        }
        public bool ToolPrev
        {
            get
            {
                return Model.ToolPrev;
            }
            set
            {
                DebouncedToolPrev = !Model.ToolPrev && value;
                Model.ToolPrev = value;
                NotifyOfPropertyChange(() => ToolPrev);
            }
        }
        public bool ArmReset
        {
            get
            {
                return Model.ArmReset;
            }
            set
            {
                DebouncedArmReset = !Model.ArmReset && value;
                Model.ArmReset = value;
                NotifyOfPropertyChange(() => ArmReset);
            }
        }
        public bool DrillClockwise
        {
            get
            {
                return Model.DrillClockwise;
            }
            set
            {
                DebouncedDrillClockwise = !Model.DrillClockwise && value;
                Model.DrillClockwise = value;
                NotifyOfPropertyChange(() => DrillClockwise);
            }
        }
        public bool DrillCounterClockwise
        {
            get
            {
                return Model.DrillCounterClockwise;
            }
            set
            {
                DebouncedDrillCounterClockwise = !Model.DrillCounterClockwise && value;
                Model.DrillCounterClockwise = value;
                NotifyOfPropertyChange(() => DrillCounterClockwise);
            }
        }
        public bool ButtonRs
        {
            get
            {
                return Model.ButtonRs;
            }
            set
            {
                DebouncedButtonRs = !Model.ButtonRs && value;
                Model.ButtonRs = value;
                NotifyOfPropertyChange(() => ButtonRs);
            }
        }
        public bool ButtonLs
        {
            get
            {
                return Model.ButtonLs;
            }
            set
            {
                DebouncedButtonLs = !Model.ButtonLs && value;
                Model.ButtonLs = value;
                NotifyOfPropertyChange(() => ButtonLs);
            }
        }
        public bool ModeNext
        {
            get
            {
                return Model.ModeNext;
            }
            set
            {
                DebouncedModeNext = !Model.ModeNext && value;
                Model.ModeNext = value;
                NotifyOfPropertyChange(() => ModeNext);
            }
        }
        public bool ModePrev
        {
            get
            {
                return Model.ModePrev;
            }
            set
            {
                DebouncedModePrev = !Model.ModePrev && value;
                Model.ModePrev = value;
                NotifyOfPropertyChange(() => ModePrev);
            }
        }
        public bool DebouncedButtonA
        {
            get
            {
                return Model.ButtonADebounced;
            }
            set
            {
                Model.ButtonADebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonA);
            }
        }
        public bool DebouncedToolNext
        {
            get
            {
                return Model.ToolNextDebounced;
            }
            set
            {
                Model.ToolNextDebounced = value;
                NotifyOfPropertyChange(() => DebouncedToolNext);
            }
        }
        public bool DebouncedToolPrev
        {
            get
            {
                return Model.ToolPrevDebounced;
            }
            set
            {
                Model.ToolPrevDebounced = value;
                NotifyOfPropertyChange(() => DebouncedToolPrev);
            }
        }
        public bool DebouncedArmReset
        {
            get
            {
                return Model.ArmResetDebounced;
            }
            set
            {
                Model.ArmResetDebounced = value;
                NotifyOfPropertyChange(() => DebouncedArmReset);

            }
        }
        public bool DebouncedDrillClockwise
        {
            get
            {
                return Model.DrillClockwiseDebounced;
            }
            set
            {
                Model.DrillClockwiseDebounced = value;
                NotifyOfPropertyChange(() => DebouncedDrillClockwise);
            }
        }
        public bool DebouncedDrillCounterClockwise
        {
            get
            {
                return Model.DrillCounterClockwiseDebounced;
            }
            set
            {
                Model.DrillCounterClockwiseDebounced = value;
                NotifyOfPropertyChange(() => DebouncedDrillCounterClockwise);
            }
        }
        public bool DebouncedButtonRs
        {
            get
            {
                return Model.ButtonRsDebounced;
            }
            set
            {
                Model.ButtonRsDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonRs);
            }
        }
        public bool DebouncedButtonLs
        {
            get
            {
                return Model.ButtonLsDebounced;
            }
            set
            {
                Model.ButtonLsDebounced = value;
                NotifyOfPropertyChange(() => DebouncedButtonLs);
            }
        }
        public bool DebouncedModeNext
        {
            get
            {
                return Model.ModeNextDebounced;
            }
            set
            {
                Model.ModeNextDebounced = value;
                NotifyOfPropertyChange(() => DebouncedModeNext);
            }
        }
        public bool DebouncedModePrev
        {
            get
            {
                return Model.ModePrevDebounced;
            }
            set
            {
                Model.ModePrevDebounced = value;
                NotifyOfPropertyChange(() => DebouncedModePrev);
            }
        }
        public bool BaseCounterClockwise
        {
            get
            {
                return Model.BaseCounterClockwise;
            }
            set
            {
                Model.BaseCounterClockwise = value;
                NotifyOfPropertyChange(() => BaseCounterClockwise);
            }
        }
        public bool ActuatorForward
        {
            get
            {
                return Model.ActuatorForward;
            }
            set
            {
                Model.ActuatorForward = value;
                NotifyOfPropertyChange(() => ActuatorForward);
            }
        }
        public bool BaseClockwise
        {
            get
            {
                return Model.BaseClockwise;
            }
            set
            {
                Model.BaseClockwise = value;
                NotifyOfPropertyChange(() => BaseClockwise);
            }
        }
        public bool ActuatorBackward
        {
            get
            {
                return Model.ActuatorBackward;
            }
            set
            {
                Model.ActuatorBackward = value;
                NotifyOfPropertyChange(() => ActuatorBackward);
            }
        }
        #endregion

        public JoystickInputViewModel(ControlCenterViewModel cc)
        {
            _controlCenter = cc;


            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Instantiate the joystick
            Console.WriteLine("HERE");
            joystick = new Joystick(directInput, joystickGuid);
            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();

            var datas = joystick.GetBufferedData();
            foreach (var state in datas)
                Console.WriteLine(state);
        }


        public void Update()
        {
            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Connected = false;
                return;
            }
            Connected = true;
            var datas = joystick.GetBufferedData();
            foreach (var state in datas)
            {
                ModeNext = (state.Offset.ToString() == "Buttons2");
                ModePrev = (state.Offset.ToString() == "Buttons3");
            }

        }
    }
}