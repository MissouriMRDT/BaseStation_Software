using Core.Contexts;

namespace Core.Configurations
{
    public static class MetadataManagerConfig
    {
        internal static MetadataSaveContext RovecommTwoMetadata = new MetadataSaveContext(new[] {
            new MetadataServerContext("Drive Board", "192.168.1.131", "11001") {
                Commands = new[] {
                    //Drive
                    new MetadataRecordContext(1000, "DriveLeftRight", "Left wheels speed followed by right wheels speed"), //same as last year
                    new MetadataRecordContext(1001, "DriveIndividual", "Controls each wheel individiually"), //not implemented
                    new MetadataRecordContext(1002, "WatchdogOverride", ""), //not implemented

                    //Lighting
                    new MetadataRecordContext(14000, "Headlights", "Headlight intensity for the front of rover"), //same as last year
                    new MetadataRecordContext(14001, "UnderglowColor", "rgb byte[]"), //same as last year
                    new MetadataRecordContext(14002, "CycleLightingMode", ""), //same as last year
                    new MetadataRecordContext(14003, "StateDisplay", "enum blue,red,green") //not implemented
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(1100, "DriveSpeeds", "The drive speed of each motor, counterclockwise") //not implemented
                }
            },

            new MetadataServerContext("Power Board", "192.168.1.132", "11003") {
                Commands = new[] {
                    new MetadataRecordContext(3000, "PowerBusEnableDisable", "Enables or Disables power bus"), //not implemented
                    new MetadataRecordContext(3001, "12VBusEnableDisable" , "Enables or Disables 12V bus"), //not implemented
                    new MetadataRecordContext(3002, "30VBusEnableDisable", "Enables or Disables 30V bus"), //not implemented
                    new MetadataRecordContext(3003, "VacuumEnableDisable", "Enables or Disables vaccum bus"), //not implemented
                    new MetadataRecordContext(3004, "PatchPanelEnableDisable", "Enables or Disables path panel") //not implemented
                },
                Telemetry = new[] {
                    new MetadataRecordContext(3100, "MotorBusEnabled", "Which motors are enabled"), //not implemented
                    new MetadataRecordContext(3101, "12VEnabled", "Which 12V busses are enabled"), //not implemented
                    new MetadataRecordContext(3102, "30VEnabled", "Which 30V busses are enabled"), //not implemented
                    new MetadataRecordContext(3103, "VaccumEnabled", "Is or isn't the vacuum enabled"), //not implemented
                    new MetadataRecordContext(3104, "PatchPanelEnabled", "Which panels are enabled"), //not implemented
                    new MetadataRecordContext(3105, "MotorBusCurrent", "Each main motor current"), //updated/split from last year
                    new MetadataRecordContext(3106, "12VBusCurrent", "12V current draws"), //updated/split from last year
                    new MetadataRecordContext(3107, "30VBusCurrent", "30V current draws"), //not implemented
                    new MetadataRecordContext(3108, "VacuumCurrent", "Vacuum current draw") //not implemented
                }
            },

            /*New rovecomm ids, but we will be using the old board for a little while
            new MetadataServerContext("BMS Board", "192.168.1.133", "11002") {
                Commands = new[] {
                    new MetadataRecordContext(2000, "BMSStop", "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!"), //same as last year
                    new MetadataRecordContext(2001, "SoftwareEStop", "Restarts the system in x seconds"), //updated, used to be in Estop
                    new MetadataRecordContext(2002, "WirelessEStopEnable", "Enables/disables the wireless estop"), //not implemented
                    new MetadataRecordContext(2003, "WirelessEstop", "Like BMS E-stop, but a wireless button") //not implemented
                },
                Telemetry = new[] {
                    new MetadataRecordContext(2100, "TotalPackCurrentInt", "BMS"), //same as last year
                    new MetadataRecordContext(2101, "TotalPackVoltageInt", "BMS"), //seperated from cell currents
                    new MetadataRecordContext(2102, "CellCurrentInts", "BMS"), //name updated, indexs updated
                    new MetadataRecordContext(2103, "BMSTemperatureInt", "BMS") //same as last year
                }
            },
            */
            //2019 BMS
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

            new MetadataServerContext("Camera Board", "192.168.1.134", "11004") { },

            new MetadataServerContext("Nav Board", "192.168.1.135", "11005") {
                Telemetry = new[] {
                    new MetadataRecordContext(5100, "GPSLatLong", "lat,long"),
                    new MetadataRecordContext(5101, "PitchHeadingRoll", "pitch, heading, roll")
                }
            },

            new MetadataServerContext("Gimbal Board", "192.168.1.136", "11006") {
                Commands = new[] {
                    new MetadataRecordContext(6000, "LeftDriveGimbal", "pan, tilt"), //updated
                    new MetadataRecordContext(6001, "RightDriveGimbal", "pan, tilt"), //updated
                    new MetadataRecordContext(6002, "LeftMainGimbal", "pan, tilt"), //updated
                    new MetadataRecordContext(6003, "RightMainGimbal", "pan, tilt"), //updated
                    new MetadataRecordContext(6004, "LeftDriveAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6005, "RightDriveAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6006, "LeftMainAbsolute", "pan, tilt"), //not implemented
                    new MetadataRecordContext(6007, "RightMainAbsolute", "pan, tilt") //not implemented
                },
                Telemetry = new[] {
                    new MetadataRecordContext(6100, "ServoPosition", "Array of 8 servo positions") //not implemented
                }
            },

            new MetadataServerContext("Arm Board", "192.168.1.137", "11007") {
                Commands = new[]
                { //GripperSwap? ToolSelection? ArmStop? ArmBusEnableDisable? ToggleAutoPositionTelem? ArmGetXYZ
                    new MetadataRecordContext(7000, "ArmToAngle", "All values for the arm together. Armj1-j6."), //same as last year
                    new MetadataRecordContext(7001, "ArmToIK", "All values for the arm together. X,Y,Z,P,Y,R."), //updated/implemented
                    new MetadataRecordContext(7002, "IKRoverIncrement", "Incremental values for rover ik."), //same as last year
                    new MetadataRecordContext(7003, "IKWristIncrement", "Incremental values for wrist ik."), //same as last year
                    new MetadataRecordContext(7004, "ArmValues", "All values for the arm together. Armj1-j6, gripper1, nipper, gripper2."), //same as last year
                    new MetadataRecordContext(7005, "EndEffectorActuation", "enable/disable solenoid"), //not implemented
                    new MetadataRecordContext(7006, "GripperOpenLoop", "-1000,1000"), //not implemented
                    new MetadataRecordContext(7007, "ArmCommands", ""), //same as last year
                    new MetadataRecordContext(7008, "?????", ""), //manifest doesn't explain what this is
                    new MetadataRecordContext(7009, "ForearmMotors", "j5,j6,gripper1,nipper,gripper2"), //not implemented
                    new MetadataRecordContext(7010, "BicepMotors", "j1,j2,j3,j4"), //not implemented
                    new MetadataRecordContext(7011, "ForearmAngles", "j5,j6,gripper1,nipper,gripper2"), //not implemented
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
                    new MetadataRecordContext(7104, "LimitSwitchValues", "mc1,mc2"), //not implemented
                    new MetadataRecordContext(7105, "IKValue", "XYZPYR") //updated. Used to be XYZYPR
                }
            },

            new MetadataServerContext("ScienceAcutation Board", "192.168.1.138", "11008") {
                Commands = new[] {
                    new MetadataRecordContext(8000, "ZActuation", "-1000 to 1000 open loop for Z axis control"), //NOT UPDATED: still screw stuff
                    new MetadataRecordContext(8001, "GenevaOpenLoop", "-1000 to 1000 open loop for Geneva control"), //not implemented
                    new MetadataRecordContext(8002, "Chemicals", "Array to control all 3 chemicals"), //not implemented
                    new MetadataRecordContext(8002, "GenevaToPosition", "Set Geneva absolute position"), //not implemented
                    new MetadataRecordContext(8004, "GenevaIncrementPosition", "Increment Geneva position by x"), //not implemented
                    new MetadataRecordContext(8005, "Vaccum", "Vacuum off/on"), //not implemented
                    new MetadataRecordContext(8006, "LimitSwitchOverride", "0-off/1-on, [Ztop, Zbottom, GenevaSet, GenevaHome]") //not implemented
                },
                Telemetry = new[]
                {
                    new MetadataRecordContext(8100, "GenevaCurrentPosition", ""), //not implemented
                    new MetadataRecordContext(8101, "LimitSwitchTriggered", "[Ztop, Zbottom, GenevaSet, GenevaHome]") //not implemented
                }
            },

            new MetadataServerContext("ScienceSensors Board", "192.168.1.139", "11009") {
                Commands = new[] {
                    new MetadataRecordContext(9000, "UVLedControl", "Control of light source."), //same as last year
                    new MetadataRecordContext(9001, "RunSpectrometer", "Sends command to begin the spectrometer sequence."), //same as last year
                    new MetadataRecordContext(9002, "ScienceLight", ""), //not implemented
                    new MetadataRecordContext(9003, "MPPC", "num of readings") //not implemented
                }

                //no telemetry, so a lot of legacy code
            },

            new MetadataServerContext("BSMS", "192.168.1.142", "11012") { },

            new MetadataServerContext("Blackbox", "192.168.1.143", "11013") { },

            new MetadataServerContext("Autonomy Board", "192.168.1.144", "11015") { }

        });
    }
}
