using RED.Interfaces;
using System.Collections.ObjectModel;

namespace RED.Models
{
    internal class XboxControllerInputModel
    {
        internal int SerialReadSpeed;
        internal int ManualDeadzone;
        internal bool AutoDeadzone = false;

        internal ObservableCollection<IControllerMode> ControllerModes = new ObservableCollection<IControllerMode>();
        internal int CurrentModeIndex;

        internal bool Connected;
        internal string ConnectionStatus = "Disconnected";

        internal float WheelsLeft; // JoyStick1X
        internal float WheelsRight; // JoyStick1Y
        internal float ElbowBend; // JoyStick2X
        internal float ElbowTwist; // JoyStick2Y
        internal float GripperOpen; // LeftTrigger
        internal float GripperClose; // RightTrigger
        internal bool ButtonA; // Unused
        internal bool ToolNext; // ButtonB
        internal bool ToolPrev; // ButtonX
        internal bool ArmReset; // ButtonY
        internal bool DrillClockwise; // ButtonRb
        internal bool DrillCounterClockwise; // ButtonLb
        internal bool ButtonLs; // Unused
        internal bool ButtonRs; // Unused
        internal bool ModeNext; // ButtonStart
        internal bool ModePrev; // ButtonBack
        internal bool ButtonADebounced; // Unused
        internal bool ButtonBDebounced; // Unused
        internal bool ButtonXDebounced; // Unused
        internal bool ButtonYDebounced; // Unused
        internal bool ButtonRbDebounced; // Unused
        internal bool ButtonLbDebounced; // Unused
        internal bool ButtonLsDebounced; // Unused
        internal bool ButtonRsDebounced; // Unused
        internal bool ButtonStartDebounced; // Unused
        internal bool ButtonBackDebounced; // Unused
        internal bool BaseCounterClockwise; // DPadL
        internal bool BaseClockwise; // DPadR
        internal bool ActuatorForward; // DPadU
        internal bool ActuatorBackward; // DPadD
    }
}