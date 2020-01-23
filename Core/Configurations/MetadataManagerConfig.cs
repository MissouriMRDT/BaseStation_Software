using Core.Contexts;

namespace Core.Configurations
{
    public static class MetadataManagerConfig
    {
        internal static MetadataSaveContext RovecommTwoMetadata = new MetadataSaveContext(new[] {
            new MetadataServerContext("Drive Board", "192.168.1.134", "11001") {
                Commands = new[] {
                    new MetadataRecordContext(1000, "DriveLeftRight", "Left wheels speed followed by right wheels speed"),
                    new MetadataRecordContext(1001, "SpeedRamp", "Controls the accleration limit, ms to full speed"),
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(1100, "DriveWatchdogTriggered", "")
                }
            },
            new MetadataServerContext("Arm Board", "192.168.1.131", "11008") {
                Commands = new[]
                {
                    new MetadataRecordContext(8000, "ArmToAngle", "All values for the arm together. Armj1-j6."),
                    new MetadataRecordContext(8002, "IKRoverIncrement", "Incremental values for rover ik."),
                    new MetadataRecordContext(8003, "IKGripperIncrement", "Incremental values for gripper ik."),
                    new MetadataRecordContext(8004, "ArmValues", "All values for the arm together. Armj1-j6, then primary then secondary gripper."),
                    new MetadataRecordContext(8007, "ArmCommands", "Swap Gripper, Get Position 0, 1"),
                    new MetadataRecordContext(8013, "ToolSelection", "Change the selected tool, 0 1 & 2"),
                    new MetadataRecordContext(8014, "Laser", "Toggle the laser"),
                    new MetadataRecordContext(8015, "LimitSwitchOverride", "Toggle the laser")

                },
                 Telemetry = new[]
                {
                    new MetadataRecordContext(8101, "ArmAngles", "Angles for the arm joints")
                }
            },

            new MetadataServerContext("BMS Board", "192.168.1.133", "11002") {
                Commands = new[] {
                    new MetadataRecordContext(2000, "BMSStop", "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(2101, "BMSVoltages", "BMS"),
                    new MetadataRecordContext(2100, "TotalPackCurrentInt", "BMS"),
                    new MetadataRecordContext(2102, "BMSTemperatureInt", "BMS"),
                    new MetadataRecordContext(2103, "BMSError", "BMS")
                }
            },

            new MetadataServerContext("Power Board", "192.168.1.132", "11003") {
                Commands = new[] {
                    new MetadataRecordContext(3000, "PowerBusEnableDisable", "Enables or Disables power bus"),
                },
                Telemetry = new[] {
                    new MetadataRecordContext(3100, "PowerCurrents", "Powerboard"),
                    new MetadataRecordContext(3101, "PowerBusStatus", "")
                }
            },
            new MetadataServerContext("NavCamera Board", "192.168.1.136", "11005") {
                Commands = new[]
                {
                    new MetadataRecordContext(5000, "CalibrateIMU", "")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(5100, "GPSPosition", "GPS Position. Latitude followed by Longitude"),
                    new MetadataRecordContext(5101, "PitchHeadingRoll", ""),
                    new MetadataRecordContext(5102, "Lidar", ""),
                    new MetadataRecordContext(5103, "GPSTelem", ""),
                    //new MetadataRecordContext(1296, "GPSQuality", "Quality of GPS signal"),
                    //new MetadataRecordContext(1298, "GPSSpeed", "Speed of GPS delta"),
                    //new MetadataRecordContext(1299, "GPSSpeedAngle", "Angle of GPS delta in degrees"),
                    //new MetadataRecordContext(1300, "GPSAltitude", "GPS Altitude"),
                    //new MetadataRecordContext(1301, "GPSSatellites", "Number of GPS Satellites"),

                    new MetadataRecordContext(4000, "CameraMuxChannel1", "Selection for Camera Mux Channel"),
                }
            },
            new MetadataServerContext("Gimbal Board", "192.168.1.135", "11006") {
                Commands = new[] {
                    new MetadataRecordContext(6002, "MainGimbalIncrement", "pan, tilt"),
                    new MetadataRecordContext(6004, "DriveGimbalIncrement", "pan, tilt")
                }
            },
            new MetadataServerContext("ScienceSensors Board", "192.168.1.138", "11010") {
                Commands = new[] {
                    new MetadataRecordContext(10000, "RunSpectrometer", "Sends command to begin the spectrometer sequence."),
                    new MetadataRecordContext(10001, "UVLedControl", "Control of light source."),
                },
                Telemetry = new[] {
                    new MetadataRecordContext(10100, "ScienceSensors", "Sensor data [AirT, AirM, SoilT, SoilM, Methane]"),
                }
            },
            new MetadataServerContext("ScienceAcutation Board", "192.168.1.137", "11009") {
                Commands = new[] {
                    new MetadataRecordContext(9000, "Screw", "-1000 to 1000 open loop for screw control"),
                    new MetadataRecordContext(9001, "ScrewAbsoluteSetPosition", ""),
                    new MetadataRecordContext(9002, "ScrewRelativeSetPosition", ""),
                    new MetadataRecordContext(9002, "XYActuation", "[x][y]"),
                    new MetadataRecordContext(9004, "CenterX", "")
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(9100, "ScrewAtPos", "byte"),
                }
            },
            new MetadataServerContext("Autonomy Board", "192.168.1.139", "11011") {
                Commands = new[] {
                    new MetadataRecordContext(11100, "AutonomousModeEnable", ""),
                    new MetadataRecordContext(11101, "AutonomousModeDisable", ""),
                    new MetadataRecordContext(11102, "WaypointAdd", ""),
                    new MetadataRecordContext(11103, "WaypointsClearAll", ""),
                    new MetadataRecordContext(11104, "AutonomyCalibrate", "")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(2580, "WaypointReached", "")
                }
            },
            new MetadataServerContext("Lighting Board", "192.168.1.142", "11007") {
                Commands = new[] {
                    new MetadataRecordContext(7000, "Headlights", "Headlights for the front of rover"),
                    new MetadataRecordContext(7001, "UnderglowColor", "rgb byte[]"),
                    new MetadataRecordContext(7002, "CycleLightingMode", "byte mode")
                }
            },
        });
    }
}
