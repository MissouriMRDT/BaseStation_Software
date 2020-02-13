using Core.Contexts.Input;

namespace Core.Configurations.Input
{
    public static class InputManagerConfig
    {
        public static InputMappingsContext DefaultInputMappings = new InputMappingsContext(new[] {
            new InputMappingContext("Tank Drive (Traditional)", "Xbox", "Drive", 100, new[] {
                new InputChannelContext("JoyStick1Y", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Diagonal Drive", "FlightStick", "Drive", 100, new[] {
                new InputChannelContext("X", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Vector Drive", "FlightStick", "Drive", 100, new[] {
                new InputChannelContext("X", "VectorX"),
                new InputChannelContext("Y", "VectorY"),
                new InputChannelContext("Button8", "ForwardBump"),
                new InputChannelContext("Button10", "BackwardBump"),
                new InputChannelContext("Slider0", "Throttle"),
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Arm (Traditional)", "Xbox", "Arm", 100, new[] {
                new InputChannelContext("JoyStick1Y", "IKYIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "ElbowBend"){},
                new InputChannelContext("JoyStick1X", "IKXIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick1X", "ElbowTwist"){},
                new InputChannelContext("JoyStick2Y", "IKPitchIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WristBend"){},
                new InputChannelContext("JoyStick2X", "IKYawIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick2X", "WristTwist"){},
                new InputChannelContext("ButtonDPadUDebounced", "UseOpenLoop"),
                new InputChannelContext("ButtonDPadDDebounced", "UseAngular"),
                new InputChannelContext("ButtonDPadRDebounced", "UseRoverPerspectiveIK"),
                new InputChannelContext("ButtonDPadLDebounced", "UseWristPerspectiveIK"),
                new InputChannelContext("ButtonY", "Nipper"),
                new InputChannelContext("LeftTrigger", "IKZMagnitude"),
                new InputChannelContext("RightTrigger", "BaseBendMagnitude"),
                new InputChannelContext("RightTrigger", "IKRollMagnitude"),
                new InputChannelContext("LeftTrigger", "BaseTwistMagnitude"),
                new InputChannelContext("ButtonRb", "IKRollDirection"),
                new InputChannelContext("ButtonLb", "BaseTwistDirection"),
                new InputChannelContext("ButtonLb", "IKZDirection"),
                new InputChannelContext("ButtonRb", "BaseBendDirection"),
                new InputChannelContext("ButtonB", "GripperOpen"),
                new InputChannelContext("ButtonX", "GripperClose"),
                new InputChannelContext("ButtonA", "SwitchTool"),
                new InputChannelContext("ButtonBackDebounced", "GripperSwap"),
                new InputChannelContext("ButtonStartDebounced", "LaserToggle") }),

            new InputMappingContext("Xbox Science Controls", "Xbox", "ScienceControls", 100, new[] {
                new InputChannelContext("JoyStick1Y", "Screw"){ Parabolic = true },
                new InputChannelContext("DPadU", "ScrewPosUp"),
                new InputChannelContext("DPadD", "ScrewPosDown"),
                new InputChannelContext("ButtonA","VacuumPulse"),
                new InputChannelContext("ButtonX", "Chem1"),
                new InputChannelContext("ButtonY", "Chem2"),
                new InputChannelContext("ButtonB", "Chem3"),
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Gimbal", "Xbox", "Gimbal", 100, new[] {
                new InputChannelContext("JoyStick1X", "Pan"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "Tilt"){ Parabolic = true },
                new InputChannelContext("LeftTrigger", "GimbalMastTiltMagnitude"){ Parabolic = true },
                new InputChannelContext("ButtonLb", "GimbalMastTiltDirection"),
                new InputChannelContext("JoyStick2X", "Roll"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "Zoom"){ Parabolic = true },
                new InputChannelContext("DPadU", "MainGimbalSwitch"){ Parabolic = true },
                new InputChannelContext("DPadD", "DriveGimbalSwitch"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Key Drive", "Keyboard", "Drive", 100, new[] {
                new InputChannelContext("WS", "WheelsLeft"),
                new InputChannelContext("IK", "WheelsRight"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
            new InputMappingContext("Key Gimbal", "Keyboard", "Gimbal", 100, new[] {
                new InputChannelContext("AD", "Pan"),
                new InputChannelContext("WS", "Tilt"),
                new InputChannelContext("I", "ZoomIn"),
                new InputChannelContext("K", "ZoomOut"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
        });

        public static InputSelectionsContext DefaultInputSelections = new InputSelectionsContext(new[] {
            new InputSelectionContext("Drive", "Xbox 1", "Tank Drive (Traditional)", true),
            new InputSelectionContext("Arm", "Xbox 1", "Arm (Traditional)", false),
            new InputSelectionContext("Main Gimbal", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Science Controls", "Xbox 1", "Xbox Science Controls", false)
        });
    }
}
