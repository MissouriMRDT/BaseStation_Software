using Core.Contexts;

namespace Core.Configurations
{
    public static class MetadataManagerConfig
    {
        internal static MetadataSaveContext RovecommTwoMetadata = new MetadataSaveContext(new[] {


            new MetadataServerContext("Arm Board", "192.168.1.131", "11007") {
                Commands = new[]
                { //GripperSwap? ToolSelection? ArmStop? ArmBusEnableDisable? ToggleAutoPositionTelem? ArmGetXYZ
                    new MetadataRecordContext(7000, "ArmToAngle", "All values for the arm together. Armj1-j6."), //same as last year
                    new MetadataRecordContext(7001, "ArmToIK", "All values for the arm together. X,Y,Z,P,Y,R."), //updated/implemented
                    new MetadataRecordContext(7002, "IKRoverIncrement", "Incremental values for rover ik. xyzpyr"), //same as last year
                    new MetadataRecordContext(7003, "IKWristIncrement", "Incremental values for wrist ik. xyzpyr"), //same as last year
                    new MetadataRecordContext(7004, "ArmValues", "All values for the arm together. Armj1-j6, gripper1, nipper, gripper2."), //same as last year
                    new MetadataRecordContext(7005, "EndEffectorActuation", "enable/disable solenoid"), //not implemented
                    new MetadataRecordContext(7006, "GripperOpenLoop", "-1000,1000"), //not implemented
                    new MetadataRecordContext(7007, "ArmCommands", ""), //same as last year
                    new MetadataRecordContext(7008, "?????", ""), //manifest doesn't explain what this is
                    new MetadataRecordContext(7009, "ForearmMotors", "j5,j6,gripper1,nipper,gripper2"), //not implemented
                    new MetadataRecordContext(7010, "BicepMotors", "j1,j2,j3,j4"), //not implemented
                    new MetadataRecordContext(7011, "ForearmAngles", "j5,j6"), //not implemented
                    new MetadataRecordContext(7012, "BicepAngles", "j1,j2,j3,j4"), //not implemented
                    new MetadataRecordContext(7013, "ToolSelection", "Change the selected tool, 0 1 & 2"), //same as last year
                    new MetadataRecordContext(7014, "Laser", "Toggle the laser"), //same as last year
                    new MetadataRecordContext(7015, "LimitSwitchOverride", "") //same as last year

                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(7100, "ArmCurrents", "Currents for the arm motors m1-8"), //arm power only accounts for 7 motors
                    new MetadataRecordContext(7101, "ArmAngles", "Angles for the arm joints m1-6"), //same as last year
                    new MetadataRecordContext(7102, "BicepAngles", "Angles for the arm joints m1-8?"), //not implemented
                    new MetadataRecordContext(7103, "ForearmAngles", "Angles for the arm joints m1-8?"), //not implemented
                    new MetadataRecordContext(7104, "LimitSwitchValues", "ls1-8 for mc1 and mc2"), //not implemented
                    new MetadataRecordContext(7105, "IKValue", "XYZPYR") //updated. Used to be XYZYPR
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

            new MetadataServerContext("Drive Board", "192.168.1.134", "11001") {
                Commands = new[] {
                    //Drive
                    new MetadataRecordContext(1000, "DriveLeftRight", "Left wheels speed followed by right wheels speed"), //same as last year, confirmed on rover
                    new MetadataRecordContext(1001, "SpeedRamp", "Controls the accleration limit, ms to full speed"),
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(1100, "DriveWatchdogTriggered", "")
                }
            },

            new MetadataServerContext("Gimbal Board", "192.168.1.135", "11006") {
                Commands = new[] {
                    new MetadataRecordContext(6000, "LeftDriveGimbal", "pan, tilt"), //updated, confirmed with tester
                    new MetadataRecordContext(6001, "RightDriveGimbal", "pan, tilt"), //updated, confirmed with tester
                    new MetadataRecordContext(6002, "LeftMainGimbal", "pan, tilt"), //updated, confirmed with tester
                    new MetadataRecordContext(6003, "RightMainGimbal", "pan, tilt"), //updated, confirmed with tester
                    new MetadataRecordContext(6004, "LeftDriveAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6005, "RightDriveAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6006, "LeftMainAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6007, "RightMainAbsolute", "pan, tilt") //not implemented
                },
                Telemetry = new[] {
                    new MetadataRecordContext(6100, "ServoPosition", "Array of 8 servo positions") //not implemented
                }
            },

            new MetadataServerContext("Nav Board", "192.168.1.136", "11005") {
                Telemetry = new[] {
                    new MetadataRecordContext(5100, "GPSPosition", "lat,long"), //same as last year
                    new MetadataRecordContext(5101, "PitchHeadingRoll", "pitch, heading, roll") //same as last year
                }
            },

            new MetadataServerContext("ScienceAcutation Board", "192.168.1.137", "11008") {
                Commands = new[] {
                    new MetadataRecordContext(8000, "ZActuation", "-1000 to 1000 open loop for Z axis control"), //updated from screw, confirmed with tester
                    new MetadataRecordContext(8001, "GenevaOpenLoop", "-1000 to 1000 open loop for Geneva control"), //not implemented
                    new MetadataRecordContext(8002, "Chemicals", "Array to control all 3 chemicals"), //added, confirmed with tester
                    new MetadataRecordContext(8002, "GenevaToPosition", "Set Geneva absolute position"), //not implemented
                    new MetadataRecordContext(8004, "GenevaIncrementPosition", "Increment Geneva position by x"), //added, confirmed with tester
                    new MetadataRecordContext(8005, "Vacuum", "Vacuum off/on"), //added, confirmed with tester
                    new MetadataRecordContext(8006, "LimitSwitchOverride", "0-off/1-on, [Ztop, Zbottom, GenevaSet, GenevaHome]") //not implemented
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(8100, "GenevaCurrentPosition", ""), //not implemented
                    new MetadataRecordContext(8101, "LimitSwitchTriggered", "[Ztop, Zbottom, GenevaSet, GenevaHome]") //not implemented
                }
            },

            new MetadataServerContext("ScienceSensors Board", "192.168.1.138", "11009") {
                Commands = new[] {
                    new MetadataRecordContext(9000, "UVLedControl", "Control of light source."), //same as last year, confirmed with tester
                    new MetadataRecordContext(9001, "RunSpectrometer", "Sends command to begin the spectrometer sequence."), //same as last year, confirmed with tester
                    new MetadataRecordContext(9002, "ScienceLight", ""), //newly implemented, confirmed with tester
                    new MetadataRecordContext(9003, "MPPC", "num of readings") //newly implemented, confirmed with tester
                },
                Telemetry = new[] { //from google sheet, confirmed incorrect
                    new MetadataRecordContext(9100, "SpectrometerData", ""), //attempted. Discarded last years TCP implementation and just assumed this would be similar to udp data
                    new MetadataRecordContext(9101, "MPPCData", ""), //attempted. Discarded last years TCP implementation and just assumed this would be similar to udp data
                    new MetadataRecordContext(9103, "Methane", "gas concentration (%), temperature"), //updated
                    new MetadataRecordContext(9104, "CO2", "gas concentration (ppm)"), //updated
                    new MetadataRecordContext(9105, "O2", "partial pressure (mBar), tempartature (C), concentration (ppm), barometric pressure (mBar)") //updated
                }
            },

            new MetadataServerContext("Autonomy Board", "192.168.1.139", "11015") {
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

            new MetadataServerContext("Lighting Board", "192.168.1.142", "11016") {
                Commands = new[] {
                    new MetadataRecordContext(7000, "Headlights", "Headlights for the front of rover"),
                    new MetadataRecordContext(7001, "UnderglowColor", "rgb byte[]"),
                    new MetadataRecordContext(7002, "CycleLightingMode", "byte mode")
                }
            },

            new MetadataServerContext("Blackbox", "192.168.1.143", "11013") { },

            new MetadataServerContext("BSMS", "192.168.1.145", "11012") { },

            new MetadataServerContext("Camera Board 1", "192.168.1.148", "11004") { },

            new MetadataServerContext("Camera Board 2", "192.168.1.149", "11017") { },

        });
    }
}
