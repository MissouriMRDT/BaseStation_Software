using RED.Contexts;

namespace RED.Configurations
{
    internal static class MetadataManagerConfig
    {
        internal static MetadataSaveContext DefaultMetadata = new MetadataSaveContext(new[] {
            new MetadataServerContext("Drive Board", "192.168.1.130") {
                Commands = new[] {
                    new MetadataRecordContext(528, "DriveLeftRight", "Left wheels speed followed by right wheels speed"),
                    new MetadataRecordContext(100, "MotorLeftSpeed", "Left motor speed. Range is -1000 to 1000 (full reverse to full forward)"),
                    new MetadataRecordContext(101, "MotorRightSpeed", "Right motor speed. Range is -1000 to 1000 (full reverse to full forward)"),
                    new MetadataRecordContext(1584, "DropBayOpen", "Commands the specified drop bay to open"),
                    new MetadataRecordContext(1585, "DropBayClose", "Commands the specified drop bay to close"),
                    new MetadataRecordContext(2320, "UnderglowColor", ""),
                    new MetadataRecordContext(2336, "Headlights", ""),
                    new MetadataRecordContext(102, "PanServo", ""),
                    new MetadataRecordContext(103, "TiltServo", ""),
                }
            },
            new MetadataServerContext("Arm Board", "192.168.1.131") {
                Commands = new[] {
                    new MetadataRecordContext(801, "ArmJ1", "Positive is clockwise. Negative is counter-clockwise."),
                    new MetadataRecordContext(802, "ArmJ2", "Positive is up. Negative is down."),
                    new MetadataRecordContext(803, "ArmJ3", "Positive is clockwise. Negative is counter-clockwise."),
                    new MetadataRecordContext(804, "ArmJ4", "Positive is up. Negative is down."),
                    new MetadataRecordContext(805, "ArmJ5", "Positive is forward. Negative is backward."),
                    new MetadataRecordContext(806, "ArmJ6", "Positive is clockwise. Negative is counter-clockwise."),
                    new MetadataRecordContext(807, "ArmValues", "All values for the arm together. Armj1-j6, then primary then secondary gripper."),
                    new MetadataRecordContext(800, "ArmStop", "Stops all arm movement"),
                    new MetadataRecordContext(816, "ArmEnableAll", "Enables/Disables Power to all Joints"),
                    new MetadataRecordContext(817, "ArmEnableMain", "Enables/Disables Power to 12V Bus"),
                    new MetadataRecordContext(818, "ArmEnableJ1", "Enables/Disables Power to J1"),
                    new MetadataRecordContext(819, "ArmEnableJ2", "Enables/Disables Power to J2"),
                    new MetadataRecordContext(820, "ArmEnableJ3", "Enables/Disables Power to J3"),
                    new MetadataRecordContext(821, "ArmEnableJ4", "Enables/Disables Power to J4"),
                    new MetadataRecordContext(822, "ArmEnableJ5", "Enables/Disables Power to J5"),
                    new MetadataRecordContext(823, "ArmEnableJ6", "Enables/Disables Power to J6"),
                    new MetadataRecordContext(824, "ArmEnableEndeff1", "Enables/Disables Power to Endeffector"),
                    new MetadataRecordContext(825, "ArmEnableEndeff2", "Enables/Disables Power to Endeffector Servo"),
                    new MetadataRecordContext(784, "ArmAbsoluteAngle", "Sets position based on six absolute angles"),
                    new MetadataRecordContext(785, "ArmAbsoluteXYZ", "Sets position based on absolute XYZ, yaw pitch and roll"),
                    new MetadataRecordContext(808, "ArmGetXYZ", "Requests to get the arm's current IK coordinates"),
                    new MetadataRecordContext(786, "IKRoverIncrement", "Increments IK positions based on rover perspective, XYZ yaw pitch roll. Each value is normalized -1000 to 1000"),
                    new MetadataRecordContext(787, "IKWristIncrement", "Increments IK positions based on wrist perspective, XYZ yaw pitch roll. Each value is normalized -1000 to 1000"),
                    new MetadataRecordContext(793, "ArmGetPosition", "Requests Current Position"),
                    new MetadataRecordContext(896, "LimitSwitchUnOverride", "Index of limit switch to remove override"),
                    new MetadataRecordContext(897, "LimitSwitchOverride", "Index of limit switch to override"),
                    new MetadataRecordContext(864, "Endeffector1", "Positive is open. Negative is close."),
                    new MetadataRecordContext(868, "Endeffector2", "Positive is clockwise. Negative is counterclockwise."),
                    new MetadataRecordContext(869, "GripperSwap", "Swaps the gripper position by 180 degrees so that the rear facing gripper faces forward."),
                    new MetadataRecordContext(870, "OpPoint", "Sets the IK Op Point, 3 floats, x y z"),
                    new MetadataRecordContext(871, "ToggleAutoPositionTelem", "toggles automatically sending arm position data ")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(792, "ArmCurrentPosition", "Contains the current angles from each joint"),
                    new MetadataRecordContext(794, "ArmCurrentXYZ", "Contains the current IK coordinates of arm. xyz, yaw pitch roll"),
                    new MetadataRecordContext(832, "ArmFault", "Indicates when the arm reports a fault condition"),
                    new MetadataRecordContext(865, "GripperCurrent", "Current reading from gripper motor"),
                    new MetadataRecordContext(867, "DrillCurrent", "Current reading from drill motor"),
                    new MetadataRecordContext(880, "ArmCurrentMain", "Current reading for main power")
                }
            },
            new MetadataServerContext("Power Board", "192.168.1.132") {
                Commands = new[] {
                    new MetadataRecordContext(1040, "BMSStop", "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!"),
                    new MetadataRecordContext(1041, "BMSReboot", "BMS E-stop with reboot."),
                    new MetadataRecordContext(1078, "BMSFanControl", "0=off, 1=on"),
                    new MetadataRecordContext(1079, "BMSBuzzerControl", "0=off, 1=on"),
                    new MetadataRecordContext(1088, "PowerBusEnable", "Enables power bus"),
                    new MetadataRecordContext(1089, "PowerBusDisable", "Disables power bus")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(1056, "Cell1Voltage", "BMS"),
                    new MetadataRecordContext(1057, "Cell2Voltage", "BMS"),
                    new MetadataRecordContext(1058, "Cell3Voltage", "BMS"),
                    new MetadataRecordContext(1059, "Cell4Voltage", "BMS"),
                    new MetadataRecordContext(1060, "Cell5Voltage", "BMS"),
                    new MetadataRecordContext(1061, "Cell6Voltage", "BMS"),
                    new MetadataRecordContext(1062, "Cell7Voltage", "BMS"),
                    new MetadataRecordContext(1063, "Cell8Voltage", "BMS"),
                    new MetadataRecordContext(1072, "TotalPackCurrent", "BMS"),
                    new MetadataRecordContext(1073, "TotalPackVoltage", "BMS"),
                    new MetadataRecordContext(1074, "BMSTemperature1", "BMS"),
                    new MetadataRecordContext(1075, "BMSTemperature2", "BMS"),
                    new MetadataRecordContext(1076, "BMSPackOvercurrent", "BMS"),
                    new MetadataRecordContext(1077, "BMSPackUndervoltage", "BMS"),
                    new MetadataRecordContext(1104, "Motor1Current", "Powerboard"),
                    new MetadataRecordContext(1105, "Motor2Current", "Powerboard"),
                    new MetadataRecordContext(1106, "Motor3Current", "Powerboard"),
                    new MetadataRecordContext(1107, "Motor4Current", "Powerboard"),
                    new MetadataRecordContext(1108, "Motor5Current", "Powerboard"),
                    new MetadataRecordContext(1109, "Motor6Current", "Powerboard"),
                    new MetadataRecordContext(1110, "Motor7Current", "Powerboard"),
                    new MetadataRecordContext(1111, "Motor8Current", "Powerboard"),
                    new MetadataRecordContext(1112, "Bus5VCurrent", "Powerboard"),
                    new MetadataRecordContext(1113, "Bus12VCurrent", "Powerboard"),
                    new MetadataRecordContext(1114, "General12V40A", "Powerboard"),
                    new MetadataRecordContext(1115, "ActuationCurrent", "Powerboard"),
                    new MetadataRecordContext(1116, "LogicCurrent", "Powerboard"),
                    new MetadataRecordContext(1117, "CommunicationsCurrent", "Powerboard"),
                    new MetadataRecordContext(1120, "InputVoltage", "Powerboard"),
                    new MetadataRecordContext(1090, "PowerBusOverCurrentNotification", "Sent if a bus is disabled due to overcurrent. Value indicates bus")
                }
            },
            new MetadataServerContext("Nav Board", "192.168.1.133") {
                Telemetry = new[] {
                    new MetadataRecordContext(1312, "Heading", "The direction of the heading of the rover"),
                    new MetadataRecordContext(1313, "IMUTemperature", "IMU Data"),
                    new MetadataRecordContext(1314, "IMUAccelerometer", "IMU Data"),
                    new MetadataRecordContext(1315, "IMUGyroscope", "IMU Data"),
                    new MetadataRecordContext(1316, "IMUMagnetometer", "IMU Data"),
                    new MetadataRecordContext(1296, "GPSQuality", "Quality of GPS signal"),
                    new MetadataRecordContext(1297, "GPSPosition", "GPS Position. Latitude followed by Longitude"),
                    new MetadataRecordContext(1298, "GPSSpeed", "Speed of GPS delta"),
                    new MetadataRecordContext(1299, "GPSSpeedAngle", "Angle of GPS delta in degrees"),
                    new MetadataRecordContext(1300, "GPSAltitude", "GPS Altitude"),
                    new MetadataRecordContext(1301, "GPSSatellites", "Number of GPS Satellites")
                }
            },
            new MetadataServerContext("Gimbal Board", "192.168.1.134") {
                Commands = new[] {
                    new MetadataRecordContext(1552, "Pan", "Pan speed of pan/tilt mount (-1000 to 1000)"),
                    new MetadataRecordContext(1553, "Zoom", "Zoom the camera. -1000 to 1000 representing how fast to zoom in or out"),
                    new MetadataRecordContext(1554, "Tilt", "Tilt speed of pan/tilt mount  (-1000 to 1000)"),
                    new MetadataRecordContext(1555, "Roll", "Roll of the (-1000,1000)"),
                    new MetadataRecordContext(1556, "Mast", "Mast control open loop (-1000,1000)"),
                    new MetadataRecordContext(1557, "GimbalOpenValues", "Compacted values for pan tilt roll mast and Zoom"),
                    new MetadataRecordContext(1558, "GimbalRecord", "Set camera recording state. Enumerated, 0 = stop, 1 = start, 2 = snapshot")
                }
            },
            new MetadataServerContext("Science Board", "192.168.1.135") {
                Commands = new[] {
                    new MetadataRecordContext(1808, "ScienceCommand", "Sends command/request to the Science experiment. Enumerated value indicates type of request."),
                },
                Telemetry = new[] {
                    new MetadataRecordContext(1824, "SciSensor0", "Sensor 0 reading from Science Experiment"),
                    new MetadataRecordContext(1825, "SciSensor1", "Sensor 1 reading from Science Experiment"),
                    new MetadataRecordContext(1826, "SciSensor2", "Sensor 2 reading from Science Experiment"),
                    new MetadataRecordContext(1827, "SciSensor3", "Sensor 3 reading from Science Experiment"),
                    new MetadataRecordContext(1828, "SciSensor4", "Sensor 4 reading from Science Experiment"),
                    new MetadataRecordContext(1829, "SciSensor5", "Sensor 5 reading from Science Experiment"),
                    new MetadataRecordContext(1830, "SciSensor6", "Sensor 6 reading from Science Experiment")
                }
            },
            new MetadataServerContext("Sensor Pod", "192.168.1.136") {
                Telemetry = new[] {
                    new MetadataRecordContext(1832, "Temperature", ""),
                    new MetadataRecordContext(1833, "Moisture", "Moist")
                }
            },
            new MetadataServerContext("Autonomy Board", "192.168.1.138") {
                Commands = new[] {
                    new MetadataRecordContext(2576, "AutonomousModeEnable", ""),
                    new MetadataRecordContext(2577, "AutonomousModeDisable", ""),
                    new MetadataRecordContext(2578, "WaypointAdd", ""),
                    new MetadataRecordContext(2579, "WaypointsClearAll", ""),
                    new MetadataRecordContext(2581, "AutonomyCalibrate", "")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(2580, "WaypointReached", "")
                }
            },
            new MetadataServerContext("Drill Board", "192.168.1.139") {
                Commands = new[] {
                    new MetadataRecordContext(2900, "Drill", "-1000 to 1000 open loop for drill control"),
                    new MetadataRecordContext(2901, "Screw", "-1000 to 1000 open loop for screw control"),
                    new MetadataRecordContext(2902, "Geneva", "Commands carousel to rotate on open loop speed, CCW -> CW, -1000 to 1000"),
                    new MetadataRecordContext(2903, "ScrewPosition", "(1,4) positions: (Bottom, Pick-Up, Staged, Drop Point)"),
                    new MetadataRecordContext(2904, "GenevaPosition", "(1,2,3,4,5,6) positions")
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(2905, "GenevaAtPos", "byte, returns (1,6)"),
                    new MetadataRecordContext(2906, "ScrewAtPos", "byte, returns (1,4)"),
                    new MetadataRecordContext(2907, "ScrewLimitTriggered", "byte, (0,4) (0 if no trigger)"),
                    new MetadataRecordContext(2908, "GenevaLimitTriggered", "byte, (0,1)"),
                    new MetadataRecordContext(2909, "CarouselLimitTriggered", "byte, (0,1)")
                }
            },
            new MetadataServerContext("Camera Board", "192.168.1.140") {
                Commands = new[] {
                    new MetadataRecordContext(2832, "CameraMuxChannel1", "Selection for Camera Mux Channel 1"),
                    new MetadataRecordContext(2833, "CameraMuxChannel2", "Selection for Camera Mux Channel 2")
                }
            },
        });
    }
}
