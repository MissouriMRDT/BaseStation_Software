/* The manifest follows the following format:
 * DATAID = [
 *  {
 *    Board: "Name",
 *    Ip: "192.168.1.___",
 *    Port: 110__,
 *    Commands: {
 *      DataIdString: {
 *        dataId: ____, (number)
 *        dataType: ____, (Enumeration declared below)
 *        comments: "General description of command",
 *      },
 *     Telemetry: {
 *        *follows same format as commands*
 *      },
 *    },
 *  }
 * ]
 */

// Enumeration of all datatypes supported by Basestation Rovecomm
export enum DataTypes {
  INT8_T = 0,
  UINT8_T = 1,
  INT16_T = 2,
  UINT16_T = 3,
  INT32_T = 4,
  UINT32_T = 5,
  FLOAT_T = 6,
}

export enum SystemPackets {
  PING = 1,
  PING_REPLY = 2,
  SUBSCRIBE = 3,
  UNSUBSCRIBE = 4,
  INVALID_VERSION = 5,
  NO_DATA = 6,
}

// The header length is currently 5 Bytes, stored here for better code in Rovecomm.ts
export const headerLength = 5

// Data sizes of the corresponding datatype enumeration
export const dataSizes = [1, 1, 2, 2, 4, 4, 2]

export const DATAID = [
  {
    Board: "Drive",
    Ip: "192.168.1.131",
    Port: 11001,
    Commands: {
      DriveLeftRight: {
        dataId: 1000,
        dataType: DataTypes.INT16_T,
        comments: "Left wheels speed followed by right wheels speed",
      },
      DriveIndividual: {
        dataId: 1001,
        dataType: DataTypes.INT16_T,
        comments: "Controls each wheel individiually",
      },
      WatchdogOverride: {
        dataId: 1002,
        dataType: DataTypes.UINT8_T,
        comments: "",
      },
      Headlights: {
        dataId: 14000,
        dataType: DataTypes.UINT8_T,
        comments: "Headlight intensity for the front of rover",
      },
      UnderglowColor: {
        dataId: 14001,
        dataType: DataTypes.UINT8_T,
        comments: "rgb byte[]",
      },
      CycleLightingMode: {
        dataId: 14002,
        dataType: DataTypes.UINT8_T,
        comments: "",
      },
      StateDisplay: {
        dataId: 14003,
        dataType: DataTypes.UINT8_T,
        comments: "enum blue,red,green",
      },
    },
    Telemetry: {
      DriveSpeeds: {
        dataId: 1100,
        dataType: DataTypes.UINT8_T,
        comments: "The drive speed of each motor, counterclockwise",
      },
    },
  },
  {
    Board: "Power",
    Ip: "192.168.1.132",
    Port: 11003,
    Commands: {
      PowerBusEnableDisable: {
        dataId: 3000,
        dataType: DataTypes.UINT8_T,
        comments: "Enables or Disables power bus",
      },
      TwelveVBusEnableDisable: {
        dataId: 3001,
        dataType: DataTypes.UINT8_T,
        comments: "Enables or Disables 12V bus",
      },
      ThirtyVBusEnableDisable: {
        dataId: 3002,
        dataType: DataTypes.UINT8_T,
        comments: "Enables or Disables 30V bus",
      },
      VacuumEnableDisable: {
        dataId: 3003,
        dataType: DataTypes.UINT8_T,
        comments: "Enables or Disables vacuum bus",
      },
      PatchPanelEnableDisable: {
        dataId: 3004,
        dataType: DataTypes.UINT8_T,
        comments: "Enables or Disables path panel",
      },
    },
    Telemetry: {
      MotorBusEnabled: {
        dataId: 3100,
        dataType: DataTypes.UINT8_T,
        comments: "Which motors are enabled",
      },
      TwelveVEnabled: {
        dataId: 3101,
        dataType: DataTypes.UINT8_T,
        comments: "Which 12V busses are enabled",
      },
      ThirtyVEnabled: {
        dataId: 3102,
        dataType: DataTypes.UINT8_T,
        comments: "Which 30V busses are enabled",
      },
      VacuumEnabled: {
        dataId: 3103,
        dataType: DataTypes.UINT8_T,
        comments: "Is or isn't the vacuum enabled",
      },
      PatchPanelEnabled: {
        dataId: 3104,
        dataType: DataTypes.UINT8_T,
        comments: "Which panels are enabled",
      },
      MotorBusCurrent: {
        dataId: 3105,
        dataType: DataTypes.FLOAT_T,
        comments: "Each main motor current",
      },
      TwelveVBusCurrent: {
        dataId: 3106,
        dataType: DataTypes.FLOAT_T,
        comments: "actuation, logic",
      },
      ThirtyVBusCurrent: {
        dataId: 3107,
        dataType: DataTypes.FLOAT_T,
        comments: "12V bus, rockets, aux",
      },
      VacuumCurrent: {
        dataId: 3108,
        dataType: DataTypes.FLOAT_T,
        comments: "Vacuum current draw",
      },
    },
  },
  {
    Board: "BMS",
    Ip: "192.168.1.133",
    Port: 11002,
    Commands: {
      BMSStop: {
        dataId: 2000,
        dataType: DataTypes.UINT8_T,
        comments:
          "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!",
      },
    },
    Telemetry: {
      BMSVoltages: {
        dataId: 2101,
        dataType: DataTypes.UINT16_T,
        comments: "BMS",
      },
      TotalPackCurrentInt: {
        dataId: 2100,
        dataType: DataTypes.INT32_T,
        comments: "BMS",
      },
      BMSTemperatureInt: {
        dataId: 2102,
        dataType: DataTypes.UINT16_T,
        comments: "BMS",
      },
      BMSError: {
        dataId: 2103,
        dataType: DataTypes.UINT8_T,
        comments: "Tells if the BMS has encountered an error",
      },
    },
  },
  {
    Board: "Camera",
    Ip: "192.168.1.134",
    Port: 11004,
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Nav",
    Ip: "192.168.1.136",
    Port: 11005,
    Commands: {},
    Telemetry: {
      GPSPosition: {
        dataId: 5100,
        dataType: DataTypes.INT32_T,
        comments: "lat,long",
      },
      PitchHeadingRoll: {
        dataId: 5101,
        dataType: DataTypes.INT16_T,
        comments: "pitch, heading, roll",
      },
    },
  },
  {
    Board: "Gimbal",
    Ip: "192.168.1.135",
    Port: 11006,
    Commands: {
      LeftDriveGimbal: {
        dataId: 6000,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      RightDriveGimbal: {
        dataId: 6001,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      LeftMainGimbal: {
        dataId: 6002,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      RightMainGimbal: {
        dataId: 6003,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      LeftDriveAbsolute: {
        dataId: 6004,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      RightDriveAbsolute: {
        dataId: 6005,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      LeftMainAbsolute: {
        dataId: 6006,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
      RightMainAbsolute: {
        dataId: 6007,
        dataType: DataTypes.INT16_T,
        comments: "pan, tilt",
      },
    },
    Telemetry: {
      ServoPosition: {
        dataId: 6100,
        dataType: DataTypes.INT16_T,
        comments: "Array of 8 servo positions",
      },
    },
  },
  {
    Board: "Arm",
    Ip: "192.168.1.137",
    Port: 11007,
    Commands: {
      ArmToAngle: {
        dataId: 7000,
        dataType: DataTypes.FLOAT_T,
        comments: "All values for the arm together. Armj1-j6.",
      },
      ArmToIK: {
        dataId: 7001,
        dataType: DataTypes.FLOAT_T,
        comments: "All values for the arm together. X,Y,Z,P,Y,R.",
      },
      IKRoverIncrement: {
        dataId: 7002,
        dataType: DataTypes.FLOAT_T,
        comments: "Incremental values for rover ik. xyzpyr",
      },
      IKWristIncrement: {
        dataId: 7003,
        dataType: DataTypes.FLOAT_T,
        comments: "Incremental values for wrist ik. xyzpyr",
      },
      ArmValues: {
        dataId: 7004,
        dataType: DataTypes.FLOAT_T,
        comments:
          "All values for the arm together. Armj1-j6, gripper1, nipper, gripper2.",
      },
      EndEffectorActuation: {
        dataId: 7005,
        dataType: DataTypes.UINT8_T,
        comments: "enable/disable solenoid",
      },
      GripperOpenLoop: {
        dataId: 7006,
        dataType: DataTypes.UINT8_T,
        comments: "-1000,1000",
      },
      ArmCommands: { dataId: 7007, dataType: DataTypes.INT8_T, comments: "" },
      Unknown: { dataId: 7008, dataType: DataTypes.INT8_T, comments: "" },
      ForearmMotors: {
        dataId: 7009,
        dataType: DataTypes.FLOAT_T,
        comments: "j5,j6,gripper1,nipper,gripper2",
      },
      BicepMotors: {
        dataId: 7010,
        dataType: DataTypes.FLOAT_T,
        comments: "j1,j2,j3,j4",
      },
      ForearmAngles: {
        dataId: 7011,
        dataType: DataTypes.FLOAT_T,
        comments: "j5,j6",
      },
      BicepAngles: {
        dataId: 7012,
        dataType: DataTypes.FLOAT_T,
        comments: "j1,j2,j3,j4",
      },
      ToolSelection: {
        dataId: 7013,
        dataType: DataTypes.UINT8_T,
        comments: "Change the selected tool, 0 1 & 2",
      },
      Laser: {
        dataId: 7014,
        dataType: DataTypes.UINT8_T,
        comments: "Toggle the laser",
      },
      LimitSwitchOverride: {
        dataId: 7015,
        dataType: DataTypes.UINT8_T,
        comments: "",
      },
    },
    Telemetry: {
      ArmCurrents: {
        dataId: 7100,
        dataType: DataTypes.FLOAT_T,
        comments: "Currents for the arm motors m1-8",
      },
      ArmAngles: {
        dataId: 7101,
        dataType: DataTypes.FLOAT_T,
        comments: "Angles for the arm joints m1-6",
      },
      BicepAngles: {
        dataId: 7102,
        dataType: DataTypes.FLOAT_T,
        comments: "Angles for the arm joints m1-8?",
      },
      ForearmAngles: {
        dataId: 7103,
        dataType: DataTypes.FLOAT_T,
        comments: "Angles for the arm joints m1-8?",
      },
      LimitSwitchValues: {
        dataId: 7104,
        dataType: DataTypes.FLOAT_T,
        comments: "ls1-8 for mc1 and mc2",
      },
      IKValue: {
        dataId: 7105,
        dataType: DataTypes.FLOAT_T,
        comments: "XYZPYR",
      },
    },
  },
  {
    Board: "Science Actuation",
    Ip: "192.168.1.138",
    Port: 11008,
    Commands: {
      ZActuation: {
        dataId: 8000,
        dataType: DataTypes.INT16_T,
        comments: "-1000 to 1000 open loop for Z axis control",
      },
      GenevaOpenLoop: {
        dataId: 8001,
        dataType: DataTypes.INT16_T,
        comments: "-1000 to 1000 open loop for Geneva control",
      },
      Chemicals: {
        dataId: 8002,
        dataType: DataTypes.INT16_T,
        comments: "Array to control all 3 chemicals",
      },
      GenevaToPosition: {
        dataId: 8002,
        dataType: DataTypes.UINT8_T,
        comments: "Set Geneva absolute position",
      },
      GenevaIncrementPosition: {
        dataId: 8004,
        dataType: DataTypes.INT8_T,
        comments: "Increment Geneva position by x",
      },
      Vacuum: {
        dataId: 8005,
        dataType: DataTypes.UINT8_T,
        comments: "Vacuum off/on",
      },
      LimitSwitchOverride: {
        dataId: 8006,
        dataType: DataTypes.UINT8_T,
        comments: "0-off/1-on, [Ztop, Zbottom, GenevaSet, GenevaHome]",
      },
    },
    Telemetry: {
      GenevaCurrentPosition: {
        dataId: 8100,
        dataType: DataTypes.UINT8_T,
        comments: "",
      },
      LimitSwitchTriggered: {
        dataId: 8101,
        dataType: DataTypes.UINT8_T,
        comments: "[Ztop, Zbottom, GenevaSet, GenevaHome]",
      },
    },
  },
  {
    Board: "Science Sensors",
    Ip: "192.168.1.139",
    Port: 11009,
    Commands: {
      UVLedControl: {
        dataId: 9000,
        dataType: DataTypes.UINT8_T,
        comments: "Control of light source.",
      },
      RunSpectrometer: {
        dataId: 9001,
        dataType: DataTypes.UINT8_T,
        comments: "Sends command to begin the spectrometer sequence.",
      },
      ScienceLight: { dataId: 9002, dataType: DataTypes.UINT8_T, comments: "" },
      MPPC: {
        dataId: 9003,
        dataType: DataTypes.UINT16_T,
        comments: "num of readings",
      },
    },
    Telemetry: {
      MPPCData: { dataId: 9100, dataType: DataTypes.UINT16_T, comments: "" },
      SpectrometerData: {
        dataId: 9101,
        dataType: DataTypes.UINT16_T,
        comments: "Spectrometer returns 2 144 long uint16 arrays upon request",
      },
      Methane: {
        dataId: 9103,
        dataType: DataTypes.FLOAT_T,
        comments: "gas concentration (%), temperature",
      },
      CO2: {
        dataId: 9104,
        dataType: DataTypes.FLOAT_T,
        comments: "gas concentration (ppm)",
      },
      O2: {
        dataId: 9105,
        dataType: DataTypes.FLOAT_T,
        comments:
          "partial pressure (mBar), tempartature (C), concentration (ppm), barometric pressure (mBar)",
      },
    },
  },
  {
    Board: "BSMS",
    Ip: "192.168.1.142",
    Port: 11012,
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Blackbox",
    Ip: "192.168.1.143",
    Port: 11013,
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Autonomy Board",
    Ip: "192.168.1.144",
    Port: 11015,
    Commands: {},
    Telemetry: {},
  },
]
