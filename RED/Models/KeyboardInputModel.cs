using RED.Interfaces;
using System.Collections.ObjectModel;

namespace RED.Models
{
    internal class KeyboardInputModel
    {
        internal int SerialReadSpeed;
        internal int ManualDeadzone;
        internal bool AutoDeadzone = false;

        internal float speedMultiplier = 1.0f;

        internal ObservableCollection<IControllerMode> ControllerModes = new ObservableCollection<IControllerMode>();
        internal int CurrentModeIndex;

        internal bool Connected;
        internal string ConnectionStatus = "Disconnected";

        internal float WheelsLeft; // Key Q / Key A
        internal float WheelsRight; // Keys E / Key D
        internal float ElbowBend; // JoyStick1Y
        internal float ElbowTwist; // JoyStick1X
        internal float WristBend; // JoyStick2Y
        internal float WristTwist; // JoyStick2X
        internal float GripperOpen; // LeftTrigger
        internal float GripperClose; // RightTrigger
        internal bool ToolNext; // ButtonB
        internal bool ToolPrev; // ButtonX
        internal bool ArmReset; // ButtonY
        internal bool DrillClockwise; // ButtonRb
        internal bool DrillCounterClockwise; // ButtonLb
        internal bool ModeNext; // Unused
        internal bool ModePrev; // Unused
        internal bool ToolNextDebounced; // Unused
        internal bool ToolPrevDebounced; // Unused
        internal bool ArmResetDebounced; // Unused
        internal bool DrillClockwiseDebounced; // Unused
        internal bool DrillCounterClockwiseDebounced; // Unused
        internal bool ModeNextDebounced; // Unused
        internal bool ModePrevDebounced; // Unused
        internal bool BaseCounterClockwise; // DPadL
        internal bool BaseClockwise; // DPadR
        internal bool ActuatorForward; // DPadU
        internal bool ActuatorBackward; // DPadD
    }
}