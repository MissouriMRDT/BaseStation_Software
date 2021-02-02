/*
  button buttonIndex (for xbox controller):
    0: A
    1: B
    2: X
    3: Y
    4: LB
    5: RB
    6: RT
    7: LT
    8: back
    9: start
    10: left stick click
    11: right stick click
    12: d up
    13: d down
    14: d left
    15: d right
    16: (unsure possibly home button)
  joystick buttonIndex
    0: left stick left/right
    1: left stick up/down
    2: right stick left/right
    3: right stick up/down
*/

export const CONTROLLERINPUT = {
  TankDrive: {
    config: "Drive",
    controller: "Xbox",
    bindings: {
      LeftSpeed: { buttonType: "joystick", buttonIndex: 1 },
      LeftDirection: { buttonType: "joystick", buttonIndex: 0 },
      RightSpeed: { buttonType: "joystick", buttonIndex: 3 },
      RightDirection: { buttonType: "joystick", buttonIndex: 2 },
      VacuumPulse: { buttonType: "button", buttonIndex: 0 },
    },
  },
  DiagonalDrive: {
    config: "Drive",
    controller: "Flight Stick",
    bindings: {
      LeftSpeed: { buttonType: "joystick", buttonIndex: 0 },
      RightSpeed: { buttonType: "joystick", buttonIndex: 1 },
    },
  },
  VectorDrive: {
    config: "Drive",
    controller: "Flight Stick",
    bindings: {
      VectorX: { buttonType: "joystick", buttonIndex: 0 },
      VectorY: { buttonType: "joystick", buttonIndex: 1 },
      Throttle: { buttonType: "joystick", buttonIndex: 6 },
      ForwardBump: { buttonType: "button", buttonIndex: 7 },
      BackwardBump: { buttonType: "button", buttonIndex: 9 },
    },
  },
  Gimbal: {
    config: "MainGimbal",
    controller: "Xbox",
    bindings: {
      PanLeft: { buttonType: "joystick", buttonIndex: 0 },
      PanRight: { buttonType: "joystick", buttonIndex: 2 },
      TiltLeft: { buttonType: "joystick", buttonIndex: 1 },
      TiltRight: { buttonType: "joystick", buttonIndex: 3 },
      MainGimbalSwitch: { buttonType: "button", buttonIndex: 12 },
      DriveGimbalSwitch: { buttonType: "button", buttonIndex: 13 },
      ButtonStartDebounced: { buttonType: "button", buttonIndex: 9 },
    },
  },
  ScienceControls: {
    config: "Science",
    controller: "Xbox",
    bindings: {
      yDirection: { buttonType: "joystick", buttonIndex: 1 },
      xDirection: { buttonType: "joystick", buttonIndex: 0 },
      VacuumPulse: { buttonType: "button", buttonIndex: 0 },
      Chem1: { buttonType: "button", buttonIndex: 2 },
      Chem2: { buttonType: "button", buttonIndex: 3 },
      Chem3: { buttonType: "button", buttonIndex: 1 },
    },
  },
  ArmControls: {
    // some of these showed two instances of some buttons with the second one saying IK in the InputMangerConfig.cs
    config: "Arm",
    controller: "Xbox",
    bindings: {
      PanLeft: { buttonType: "joystick", buttonIndex: 0 },
      PanRight: { buttonType: "joystick", buttonIndex: 2 },
      TiltLeft: { buttonType: "joystick", buttonIndex: 1 },
      TiltRight: { buttonType: "joystick", buttonIndex: 3 },
      ButtonA: { buttonType: "button", buttonIndex: 0 },
      ButtonB: { buttonType: "button", buttonIndex: 1 },
      ButtonX: { buttonType: "button", buttonIndex: 2 },
      ButtonY: { buttonType: "button", buttonIndex: 3 },
      LeftBumper: { buttonType: "button", buttonIndex: 4 },
      RightBumper: { buttonType: "button", buttonIndex: 5 },
      LeftTrigger: { buttonType: "button", buttonIndex: 6 },
      RightTrigger: { buttonType: "button", buttonIndex: 7 },
      ButtonBackDebounced: { buttonType: "button", buttonIndex: 8 },
      ButtonStartDebounced: { buttonType: "button", buttonIndex: 9 },
      DpadUDebounce: { buttonType: "button", buttonIndex: 12 },
      DpadDDebounce: { buttonType: "button", buttonIndex: 13 },
      DpadLDebounce: { buttonType: "button", buttonIndex: 14 },
      DpadRDebounce: { buttonType: "button", buttonIndex: 15 },
    },
  },
}
