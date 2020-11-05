/* eslint-disable prettier/prettier */
interface DATAID {
  [key: number]: string
}

export const CrappyDataIds = {
  1001: "SpeedRamp", // "Controls the accleration limit, ms to full speed"
  1000: "DriveLeftRight", // "Left wheels speed followed by right wheels speed"
  1100: "DriveWatchdogTriggered", // ""
  8000: "ArmToAngle", // "All values for the arm together. Armj1-j6."
  8002: "IKRoverIncrement", // "Incremental values for rover ik."
  8003: "IKGripperIncrement", // "Incremental values for gripper ik."
  8004: "ArmValues", // "All values for the arm together. Armj1-j6, then primary then secondary gripper."
  8007: "ArmCommands", // "Swap Gripper, Get Position 0, 1"
  8013: "ToolSelection", // "Change the selected tool, 0 1 & 2"
  8014: "Laser", // "Toggle the laser"
  8015: "LimitSwitchOverride", // "Toggle the laser"
  8101: "ArmAngles", // "Angles for the arm joints"
  2000: "BMSStop", // "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!"
  2101: "BMSVoltages", // "BMS"
  2100: "TotalPackCurrentInt", // "BMS"
  2102: "BMSTemperatureInt", // "BMS"
  2103: "BMSError", // "BMS"
  3000: "PowerBusEnableDisable", // "Enables or Disables power bus"
  3100: "PowerCurrents", // "Powerboard"
  3101: "PowerBusStatus", // ""
  5000: "CalibrateIMU", // ""
  5100: "GPSPosition", // "GPS Position. Latitude followed by Longitude"
  5101: "PitchHeadingRoll", // ""
  5102: "Lidar", // ""
  5103: "GPSTelem", // ""
  5104: "RoverDistanceSession", // "Distance in Miles the rover has traveled in the current session"
  // 1296, "GPSQuality", "Quality of GPS signal"
  // 1298, "GPSSpeed", "Speed of GPS delta"
  // 1299, "GPSSpeedAngle", "Angle of GPS delta in degrees"
  // 1300, "GPSAltitude", "GPS Altitude"
  // 1301, "GPSSatellites", "Number of GPS Satellites"
  4000: "CameraMuxChannel1", // "Selection for Camera Mux Channel"
  6002: "MainGimbalIncrement", // "pan, tilt"
  6004: "DriveGimbalIncrement", // "pan, tilt"
  10000: "RunSpectrometer", // "Sends command to begin the spectrometer sequence."
  10001: "UVLedControl", // "Control of light source."
  10100: "ScienceSensors", // "Sensor data [AirT, AirM, SoilT, SoilM, Methane]"
  9000: "Screw", // "-1000 to 1000 open loop for screw control"
  9001: "ScrewAbsoluteSetPosition", // ""
  9002: "ScrewRelativeSetPosition", // ""
  9003: "XYActuation", // "[x][y]"
  9004: "CenterX", // ""
  9100: "ScrewAtPos", // "byte"
  11100: "AutonomousModeEnable", // ""
  11101: "AutonomousModeDisable", // ""
  11102: "WaypointAdd", // ""
  11103: "WaypointsClearAll", // ""
  11104: "AutonomyCalibrate", // ""
  2580: "WaypointReached", // ""
  7000: "Headlights", // "Headlights for the front of rover"
  7001: "UnderglowColor", // "rgb byte[]"
  7002: "CycleLightingMode", // "byte mode"
}

export const DATAID = [
  {
    Board: "Drive",
    Ip: "192.168.1.131",
    Port: "11001",
    Commands: {
      DriveLeftRight: { dataId: 1000, dataType: 2, comments: "" },
      DriveIndividual: { dataId: 1001, dataType: 2, comments: "" },
      WatchdogOverride: { dataId: 1002, dataType: 1, comments: "" },
      Headlights: { dataId: 14000, dataType: 1, comments: "" },
      UnderglowColor: { dataId: 14001, dataType: 1, comments: "" },
      CycleLightingMode: { dataId: 14002, dataType: 1, comments: "" },
      StateDisplay: { dataId: 14003, dataType: 1, comments: "" },
    },
    Telemetry: {
      DriveSpeeds: { dataId: 1100, dataType: 1, comments: "" },
    },
  },
  {
    Board: "Power",
    Ip: "192.168.1.132",
    Port: "11003",
    Commands: {
      PowerBusEnableDisable: {
        dataId: 3000,
        dataType: 1,
        comments: "Enables or Disables power bus",
      },
      TwelveVBusEnableDisable: {
        dataId: 3001,
        dataType: 1,
        comments: "Enables or Disables 12V bus",
      },
      ThirtyVBusEnableDisable: {
        dataId: 3002,
        dataType: 1,
        comments: "Enables or Disables 30V bus",
      },
      VacuumEnableDisable: {
        dataId: 3003,
        dataType: 1,
        comments: "Enables or Disables vacuum bus",
      },
      PatchPanelEnableDisable: {
        dataId: 3004,
        dataType: 1,
        comments: "Enables or Disables path panel",
      },
    },
    Telemetry: {
      MotorBusEnabled: {
        dataId: 3100,
        dataType: 1,
        comments: "Which motors are enabled",
      },
      TwelveVEnabled: {
        dataId: 3101,
        dataType: 1,
        comments: "Which 12V busses are enabled",
      },
      ThirtyVEnabled: {
        dataId: 3102,
        dataType: 1,
        comments: "Which 30V busses are enabled",
      },
      VacuumEnabled: {
        dataId: 3103,
        dataType: 1,
        comments: "Is or isn't the vacuum enabled",
      },
      PatchPanelEnabled: {
        dataId: 3104,
        dataType: 1,
        comments: "Which panels are enabled",
      },
      MotorBusCurrent: {
        dataId: 3105,
        dataType: 6,
        comments: "Each main motor current",
      },
      TwelveVBusCurrent: {
        dataId: 3106,
        dataType: 6,
        comments: "actuation, logic",
      },
      ThirtyVBusCurrent: {
        dataId: 3107,
        dataType: 6,
        comments: "12V bus, rockets, aux",
      },
      VacuumCurrent: {
        dataId: 3108,
        dataType: 6,
        comments: "Vacuum current draw",
      },
    },
  },
  {
    Board: "BMS",
    Ip: "192.168.1.133",
    Port: "11002",
    Commands: {
      BMSStop: {
        dataId: 2000,
        dataType: 1,
        comments:
          "BMS E-stop. WARNING: Kills all rover power. Cannot be reversed remotely!",
      },
    },
    Telemetry: {
      BMSVoltages: { dataId: 2101, dataType: 3, comments: "BMS" },
      TotalPackCurrentInt: { dataId: 2100, dataType: 4, comments: "BMS" },
      BMSTemperatureInt: { dataId: 2102, dataType: 3, comments: "BMS" },
      BMSError: { dataId: 2103, dataType: 1, comments: "BMS" },
    },
  },
  {
    Board: "Camera",
    Ip: "192.168.1.134",
    Port: "11004",
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Nav",
    Ip: "192.168.1.136",
    Port: "11005",
    Commands: {},
    Telemetry: {
      GPSPosition: { dataId: 5100, dataType: 4, comments: "lat,long" },
      PitchHeadingRoll: {
        dataId: 5101,
        dataType: 2,
        comments: "pitch, heading, roll",
      },
    },
  },
  {
    Board: "Gimbal",
    Ip: "192.168.1.135",
    Port: "11006",
    Commands: {
      LeftDriveGimbal: { dataId: 6000, dataType: 2, comments: "pan, tilt" },
      RightDriveGimbal: { dataId: 6001, dataType: 2, comments: "pan, tilt" },
      LeftMainGimbal: { dataId: 6002, dataType: 2, comments: "pan, tilt" },
      RightMainGimbal: { dataId: 6003, dataType: 2, comments: "pan, tilt" },
      LeftDriveAbsolute: { dataId: 6004, dataType: 2, comments: "pan, tilt" },
      RightDriveAbsolute: { dataId: 6005, dataType: 2, comments: "pan, tilt" },
      LeftMainAbsolute: { dataId: 6006, dataType: 2, comments: "pan, tilt" },
      RightMainAbsolute: { dataId: 6007, dataType: 2, comments: "pan, tilt" },
    },
    Telemetry: {
      ServoPosition: {
        dataId: 6100,
        dataType: 2,
        comments: "Array of 8 servo positions",
      },
    },
  },
  {
    Board: "Arm",
    Ip: "192.168.1.137",
    Port: "11007",
    Commands: {
      ArmToAngle: {
        dataId: 7000,
        dataType: 6,
        comments: "All values for the arm together. Armj1-j6.",
      },
      ArmToIK: {
        dataId: 7001,
        dataType: 6,
        comments: "All values for the arm together. X,Y,Z,P,Y,R.",
      },
      IKRoverIncrement: {
        dataId: 7002,
        dataType: 6,
        comments: "Incremental values for rover ik. xyzpyr",
      },
      IKWristIncrement: {
        dataId: 7003,
        dataType: 6,
        comments: "Incremental values for wrist ik. xyzpyr",
      },
      ArmValues: {
        dataId: 7004,
        dataType: 6,
        comments:
          "All values for the arm together. Armj1-j6, gripper1, nipper, gripper2.",
      },
      EndEffectorActuation: {
        dataId: 7005,
        dataType: 1,
        comments: "enable/disable solenoid",
      },
      GripperOpenLoop: { dataId: 7006, dataType: 1, comments: "-1000,1000" },
      ArmCommands: { dataId: 7007, dataType: 0, comments: "" },
      Unknown: { dataId: 7008, dataType: 0, comments: "" },
      ForearmMotors: {
        dataId: 7009,
        dataType: 6,
        comments: "j5,j6,gripper1,nipper,gripper2",
      },
      BicepMotors: { dataId: 7010, dataType: 6, comments: "j1,j2,j3,j4" },
      ForearmAngles: { dataId: 7011, dataType: 6, comments: "j5,j6" },
      BicepAngles: { dataId: 7012, dataType: 6, comments: "j1,j2,j3,j4" },
      ToolSelection: {
        dataId: 7013,
        dataType: 1,
        comments: "Change the selected tool, 0 1 & 2",
      },
      Laser: { dataId: 7014, dataType: 1, comments: "Toggle the laser" },
      LimitSwitchOverride: { dataId: 7015, dataType: 1, comments: "" },
    },
    Telemetry: {
      ArmCurrents: {
        dataId: 7100,
        dataType: 6,
        comments: "Currents for the arm motors m1-8",
      },
      ArmAngles: {
        dataId: 7101,
        dataType: 6,
        comments: "Angles for the arm joints m1-6",
      },
      BicepAngles: {
        dataId: 7102,
        dataType: 6,
        comments: "Angles for the arm joints m1-8?",
      },
      ForearmAngles: {
        dataId: 7103,
        dataType: 6,
        comments: "Angles for the arm joints m1-8?",
      },
      LimitSwitchValues: {
        dataId: 7104,
        dataType: 6,
        comments: "ls1-8 for mc1 and mc2",
      },
      IKValue: { dataId: 7105, dataType: 6, comments: "XYZPYR" },
    },
  },
  {
    Board: "Science Actuation",
    Ip: "192.168.1.138",
    Port: "11008",
    Commands: {
      ZActuation: {
        dataId: 8000,
        dataType: 2,
        comments: "-1000 to 1000 open loop for Z axis control",
      },
      GenevaOpenLoop: {
        dataId: 8001,
        dataType: 2,
        comments: "-1000 to 1000 open loop for Geneva control",
      },
      Chemicals: {
        dataId: 8002,
        dataType: 2,
        comments: "Array to control all 3 chemicals",
      },
      GenevaToPosition: {
        dataId: 8002,
        dataType: 1,
        comments: "Set Geneva absolute position",
      },
      GenevaIncrementPosition: {
        dataId: 8004,
        dataType: 0,
        comments: "Increment Geneva position by x",
      },
      Vacuum: { dataId: 8005, dataType: 1, comments: "Vacuum off/on" },
      LimitSwitchOverride: {
        dataId: 8006,
        dataType: 1,
        comments: "0-off/1-on, [Ztop, Zbottom, GenevaSet, GenevaHome]",
      },
    },
    Telemetry: {
      GenevaCurrentPosition: { dataId: 8100, dataType: 1, comments: "" },
      LimitSwitchTriggered: {
        dataId: 8101,
        dataType: 1,
        comments: "[Ztop, Zbottom, GenevaSet, GenevaHome]",
      },
    },
  },
  {
    Board: "Science Sensors",
    Ip: "192.168.1.139",
    Port: "11009",
    Commands: {
      UVLedControl: {
        dataId: 9000,
        dataType: 1,
        comments: "Control of light source.",
      },
      RunSpectrometer: {
        dataId: 9001,
        dataType: 3,
        comments: "Sends command to begin the spectrometer sequence.",
      },
      ScienceLight: { dataId: 9002, dataType: 1, comments: "" },
      MPPC: { dataId: 9003, dataType: 3, comments: "num of readings" },
    },
    Telemetry: {
      SpectrometerData: { dataId: 9100, dataType: 6, comments: "" },
      MPPCData: { dataId: 9101, dataType: 6, comments: "" },
      Methane: {
        dataId: 9103,
        dataType: 6,
        comments: "gas concentration (%), temperature",
      },
      CO2: { dataId: 9104, dataType: 6, comments: "gas concentration (ppm)" },
      O2: {
        dataId: 9105,
        dataType: 6,
        comments:
          "partial pressure (mBar), tempartature (C), concentration (ppm), barometric pressure (mBar)",
      },
    },
  },
  {
    Board: "BSMS",
    Ip: "192.168.1.142",
    Port: "11012",
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Blackbox",
    Ip: "192.168.1.143",
    Port: "11013",
    Commands: {},
    Telemetry: {},
  },
  {
    Board: "Autonomy Board",
    Ip: "192.168.1.144",
    Port: "11015",
    Commands: {},
    Telemetry: {},
  },
]
