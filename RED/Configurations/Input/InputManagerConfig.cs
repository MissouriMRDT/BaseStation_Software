using RED.Contexts.Input;

namespace RED.Configurations.Input
{
    internal static class InputManagerConfig
    {
        internal static InputMappingsContext DefaultInputMappings = new InputMappingsContext(new[] {
            new InputMappingContext("Tank Drive (Traditional)", "Xbox", "Drive", 30, new[] {
                new InputChannelContext("JoyStick1Y", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Diagonal Drive", "FlightStick", "Drive", 30, new[] {
                new InputChannelContext("X", "WheelsLeft"){ Parabolic = true },
                new InputChannelContext("Y", "WheelsRight"){ Parabolic = true },
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Vector Drive", "FlightStick", "Drive", 30, new[] {
                new InputChannelContext("X", "VectorX"),
                new InputChannelContext("Y", "VectorY"),
                new InputChannelContext("Slider0", "Throttle"),
                new InputChannelContext("Button7Debounced", "ModeCycle") }),
            new InputMappingContext("Arm (Traditional)", "Xbox", "Arm", 100, new[] {
                new InputChannelContext("JoyStick1Y", "IKYIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "ElbowBend"){ Parabolic = true },
                new InputChannelContext("JoyStick1X", "IKXIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick1X", "ElbowTwist"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "IKPitchIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WristBend"){ Parabolic = true},
                new InputChannelContext("JoyStick2X", "IKYawIncrement"){ Parabolic = true },
                new InputChannelContext("JoyStick2X", "WristTwist"){ Parabolic = true },
                new InputChannelContext("ButtonDPadUDebounced", "UseOpenLoop"),
                new InputChannelContext("ButtonDPadDDebounced", "UseAngular"),
                new InputChannelContext("ButtonDPadRDebounced", "UseRoverPerspectiveIK"),
                new InputChannelContext("ButtonDPadLDebounced", "UseWristPerspectiveIK"),
                new InputChannelContext("ButtonY", "NipperClose"),
                new InputChannelContext("ButtonA", "NipperOpen"),
                new InputChannelContext("LeftTrigger", "IKZMagnitude"),
                new InputChannelContext("LeftTrigger", "BaseBendMagnitude"),
                new InputChannelContext("RightTrigger", "IKRollMagnitude"),
                new InputChannelContext("RightTrigger", "BaseTwistMagnitude"),
                new InputChannelContext("ButtonRb", "IKRollDirection"),
                new InputChannelContext("ButtonRb", "BaseTwistDirection"),
                new InputChannelContext("ButtonLb", "IKZDirection"),
                new InputChannelContext("ButtonLb", "BaseBendDirection"),
                new InputChannelContext("ButtonB", "GripperOpen"),
                new InputChannelContext("ButtonX", "GripperClose"),
                new InputChannelContext("ButtonBackDebounced", "GripperSwap"),
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Science Controls", "Xbox", "ScienceControls", 100, new[] {
                new InputChannelContext("JoyStick1Y", "Screw"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "Drill"){ Parabolic = true },
                new InputChannelContext("DPadL", "GenevaLeft"){ Parabolic = true },
                new InputChannelContext("DPadR", "GenevaRight"){ Parabolic = true },
                new InputChannelContext("DPadU", "OpenLoop"){ Parabolic = true },
                new InputChannelContext("DPadD", "ClosedLoop"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Gimbal", "Xbox", "Gimbal", 100, new[] {
                new InputChannelContext("JoyStick1X", "Pan"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "Tilt"){ Parabolic = true },
                new InputChannelContext("LeftTrigger", "GimbalMastTiltMagnitude"){ Parabolic = true },
                new InputChannelContext("ButtonLb", "GimbalMastTiltDirection"),
                new InputChannelContext("JoyStick2X", "Roll"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "Zoom"){ Parabolic = true },
                new InputChannelContext("DPadU", "MainGimbalSwitch"){ Parabolic = true },
                new InputChannelContext("DPadD", "SubGimbalSwitch"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Key Drive", "Keyboard", "Drive", 30, new[] {
                new InputChannelContext("WS", "WheelsLeft"),
                new InputChannelContext("IK", "WheelsRight"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
            new InputMappingContext("Key Gimbal", "Keyboard", "Gimbal", 30, new[] {
                new InputChannelContext("AD", "Pan"),
                new InputChannelContext("WS", "Tilt"),
                new InputChannelContext("I", "ZoomIn"),
                new InputChannelContext("K", "ZoomOut"),
                new InputChannelContext("RDebounced", "ModeCycle") }),
        });

        internal static InputSelectionsContext DefaultInputSelections = new InputSelectionsContext(new[] {
            new InputSelectionContext("Drive", "Xbox 1", "Tank Drive (Traditional)", true),
            new InputSelectionContext("Arm", "Xbox 1", "Arm (Traditional)", false),
            new InputSelectionContext("Main Gimbal", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Science Controls", "Xbox 1", "Xbox Science Controls", false)
        });
    }
}
