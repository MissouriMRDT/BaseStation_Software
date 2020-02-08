using Core.Contexts;

namespace Core.Configurations
{
    public static class MetadataManagerConfig
    {
        internal static MetadataSaveContext RovecommTwoMetadata = new MetadataSaveContext(new[] {
            new MetadataServerContext("Drive Board", "192.168.1.131", "11001") {
                Commands = new[] {
                    new MetadataRecordContext(1000, "DriveLeftRight", "Left wheels speed followed by right wheels speed"),
                    new MetadataRecordContext(1001, "DriveIndividual", "Controls each wheel individiually"),
                    new MetadataRecordContext(1002, "WatchdogOverride", "TODO")
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(1100, "DriveSpeeds", "The drive speed of each motor, counterclockwise")
                }
            },

            new MetadataServerContext("BMS Board", "192.168.1.132", "11002") {
                Commands = new[] {
                    new MetadataRecordContext(2000, "BMSStop", "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!"),
                    new MetadataRecordContext(2001, "SoftwareEStop", "Restarts the system in x seconds"),
                    new MetadataRecordContext(2002, "WirelessEStopEnable", "Enables/disables the wireless estop"),
                    new MetadataRecordContext(2003, "WirelessEstop", "Like BMS E-stop, but a wireless button")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(2100, "TotalPackCurrentInt", "BMS"),
                    new MetadataRecordContext(2101, "TotalPackVoltageInt", "BMS"),
                    new MetadataRecordContext(2102, "CellCurrentInts", "BMS"),
                    new MetadataRecordContext(2103, "BMSTemperatureInt", "BMS")
                }
            },

            new MetadataServerContext("Power Board", "192.168.1.133", "11003") {
                Commands = new[] {
                    new MetadataRecordContext(3000, "PowerBusEnableDisable", "Enables or Disables power bus"),
                    new MetadataRecordContext(3001, "12VBusEnableDisable" , "Enables or Disables 12V bus"),
                    new MetadataRecordContext(3002, "30VBusEnableDisable", "Enables or Disables 30V bus"),
                    new MetadataRecordContext(3003, "VacuumEnableDisable", "Enables or Disables vaccum bus"),
                    new MetadataRecordContext(3004, "PatchPanelEnableDisable", "Enables or Disables path panel")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(3100, "MotorBudEnabled", "Which motors are enabled"),
                    new MetadataRecordContext(3101, "12VEnabled", "Which 12V busses are enabled"),
                    new MetadataRecordContext(3102, "30VEnabled", "Which 30V busses are enabled"),
                    new MetadataRecordContext(3103, "VaccumEnabled", "Is or isn't the vacuum enabled"),
                    new MetadataRecordContext(3104, "PatchPanelEnabled", "Which panels are enabled"),
                    new MetadataRecordContext(3105, "MotorBusCurrent", "Each main motor current"),
                    new MetadataRecordContext(3106, "12VBusCurrent", "12V current draws"),
                    new MetadataRecordContext(3107, "30VBusCurrent", "30V current draws"),
                    new MetadataRecordContext(3108, "VacuumCurrent", "Vacuum current draw")
                }
            },

            new MetadataServerContext("Camera Board", "192.168.1.134", "11004") { },

            new MetadataServerContext("Nav Board", "192.168.1.135", "11005") { },

            new MetadataServerContext("Gimbal Board", "192.168.1.136", "11006") {
                Commands = new[] {
                    new MetadataRecordContext(6000, "LeftDriveGimbal", "pan, tilt"),
                    new MetadataRecordContext(6001, "RightDriveGimbal", "pan, tilt"),
                    new MetadataRecordContext(6002, "LeftMainGimbal", "pan, tilt"),
                    new MetadataRecordContext(6003, "RightMainGimbal", "pan, tilt"),
                    new MetadataRecordContext(6004, "LeftDriveAbsolute", "pan, tilt"),
                    new MetadataRecordContext(6005, "RightDriveAbsolute", "pan, tilt"),
                    new MetadataRecordContext(6006, "LeftMainAbsolute", "pan, tilt"),
                    new MetadataRecordContext(6007, "RightMainAbsolute", "pan, tilt")
                },
                Telemetry = new[] {
                    new MetadataRecordContext(6100, "ServoPosition", "Array of 8 servo positions")
                }
            },

            new MetadataServerContext("Arm Board", "192.168.1.137", "11007") {
                Commands = new[]
                {
                    new MetadataRecordContext(7000, "ArmToAngle", "All values for the arm together. Armj1-j6."),
                    new MetadataRecordContext(7001, "ArmToIK", "All values for the arm together. X,Y,Z,P,Y,R."),
                    new MetadataRecordContext(7002, "IKRoverIncrement", "Incremental values for rover ik."),
                    new MetadataRecordContext(7003, "IKWristIncrement", "Incremental values for wrist ik."),
                    new MetadataRecordContext(7004, "MoveOpenLoop", "All values for the arm together. Armj1-j6, gripper1, nipper, gripper2."),
                    new MetadataRecordContext(7005, "EndEffectorActuation", "enable/disable solenoid"),
                    new MetadataRecordContext(7006, "GripperOpenLoop", "-1000,1000"),
                    new MetadataRecordContext(7007, "ArmCommands", ""),
                    new MetadataRecordContext(7008, "?????", ""),
                    new MetadataRecordContext(7009, "ForearmMotors", "j5,j6,gripper1,nipper,gripper2"),
                    new MetadataRecordContext(7010, "BicepMotors", "j1,j2,j3,j4"),
                    new MetadataRecordContext(7011, "ForearmAngles", "j5,j6,gripper1,nipper,gripper2"),
                    new MetadataRecordContext(7012, "BicepAngles", "j1,j2,j3,j4"),
                    new MetadataRecordContext(7013, "ToolSelection", "Change the selected tool, 0 1 & 2"),
                    new MetadataRecordContext(7014, "Laser", "Toggle the laser"),
                    new MetadataRecordContext(7015, "LimitSwitchOverride", "Toggle the laser")

                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(7100, "ArmCurrents", "Currents for the arm motors m1-8"),
                    new MetadataRecordContext(7101, "ArmAngles", "Angles for the arm joints m1-6?"),
                    new MetadataRecordContext(7102, "BicepAngles", "Angles for the arm joints m1-8?"),
                    new MetadataRecordContext(7103, "ForearmAngles", "Angles for the arm joints m1-8?"),
                    new MetadataRecordContext(7104, "LimitSwitchValues", "mc1,mc2"),
                    new MetadataRecordContext(7105, "IKValue", "XYZPYR")
                }
            },

            new MetadataServerContext("ScienceAcutation Board", "192.168.1.138", "11008") {
                Commands = new[] {
                    new MetadataRecordContext(8000, "ZActuation", "-1000 to 1000 open loop for Z axis control"),
                    new MetadataRecordContext(8001, "GenevaOpenLoop", "-1000 to 1000 open loop for Geneva control"),
                    new MetadataRecordContext(8002, "Chemicals", "Array to control all 3 chemicals"),
                    new MetadataRecordContext(8002, "GenevaToPosition", "Set Geneva absolute position"),
                    new MetadataRecordContext(8004, "GenevaIncrementPosition", "Increment Geneva position by x"),
                    new MetadataRecordContext(8005, "Vaccum", "Vacuum off/on"),
                    new MetadataRecordContext(8006, "LimitSwitchOverride", "0-off/1-on, [Ztop, Zbottom, GenevaSet, GenevaHome]")
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(8100, "GenevaCurrentPosition", ""),
                    new MetadataRecordContext(8101, "LimitSwitchTriggered", "[Ztop, Zbottom, GenevaSet, GenevaHome]")
                }
            },

            new MetadataServerContext("ScienceSensors Board", "192.168.1.139", "11009") {
                Commands = new[] {
                    new MetadataRecordContext(9000, "UVLedControl", "Control of light source."),
                    new MetadataRecordContext(9001, "RunSpectrometer", "Sends command to begin the spectrometer sequence."),
                    new MetadataRecordContext(9002, "ScienceLight", ""),
                    new MetadataRecordContext(9003, "MPPC", "num of readings")
                }
            },

            new MetadataServerContext("Wireless Estop", "192.168.1.140", "11010") { },

            new MetadataServerContext("PR Controller", "192.168.1.141", "11011") { },

            new MetadataServerContext("BSMS", "192.168.1.142", "11012") { },

            new MetadataServerContext("Blackbox", "192.168.1.143", "11013") { },

            new MetadataServerContext("Autonomy Board", "192.168.1.144", "11015") { },

            new MetadataServerContext("Lighting Board", "192.168.1.131", "11001") {
                Commands = new[] {
                    new MetadataRecordContext(14000, "Headlights", "Headlight intensity for the front of rover"),
                    new MetadataRecordContext(14001, "LEDRGB", "rgb byte[]"),
                    new MetadataRecordContext(14002, "LEDFunction", ""),
                    new MetadataRecordContext(14003, "StateDisplay", "enum blue,red,green")
                }
            }
        });
    }
}
