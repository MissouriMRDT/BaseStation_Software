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
            new InputMappingContext("Arm (Traditional)", "Xbox", "Arm", 200, new[] { 
                new InputChannelContext("JoyStick1Y", "ElbowBend"){ Parabolic = true },
                new InputChannelContext("JoyStick1X", "ElbowTwist"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "WristTwist"){ Parabolic = true, LinearScaling = -1 },
                new InputChannelContext("JoyStick2X", "WristBend"){ Parabolic = true },
                new InputChannelContext("DPadU", "ShoulderBendForward"),
                new InputChannelContext("DPadD", "ShoulderBendBackward"),
                new InputChannelContext("DPadR", "ShoulderTwistForward"),
                new InputChannelContext("DPadL", "ShoulderTwistBackward"),
                new InputChannelContext("ButtonY", "DebouncedArmReset"),
                new InputChannelContext("LeftTrigger", "GripperOpen"),
                new InputChannelContext("RightTrigger", "GripperClose"),
                new InputChannelContext("ButtonRb", "ServoClockwise"),
                new InputChannelContext("ButtonLb", "ServoCounterClockwise"),
                new InputChannelContext("ButtonB", "TowRopeOut"),
                new InputChannelContext("ButtonX", "TowRopeIn"),
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Science Arm", "Xbox", "ScienceArm", 30, new[] { 
                new InputChannelContext("JoyStick1Y", "Arm"){ Parabolic = true },
                new InputChannelContext("JoyStick2Y", "Drill"){ Parabolic = true },
                new InputChannelContext("ButtonStartDebounced", "ModeCycle") }),
            new InputMappingContext("Xbox Gimbal", "Xbox", "Gimbal", 30, new[] { 
                new InputChannelContext("JoyStick1X", "Pan"){ Parabolic = true },
                new InputChannelContext("JoyStick1Y", "Tilt"){ Parabolic = true },
                new InputChannelContext("ButtonY", "ZoomIn"){ Parabolic = true },
                new InputChannelContext("ButtonA", "ZoomOut"){ Parabolic = true },
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
            new InputSelectionContext("Gimbal 1", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Gimbal 2", "Xbox 1", "Xbox Gimbal",false),
            new InputSelectionContext("Science Arm", "Xbox 1", "Xbox Science Arm", false)
        });
    }
}
