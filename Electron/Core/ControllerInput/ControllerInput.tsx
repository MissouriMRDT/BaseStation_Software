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
    LeftSpeed: { buttonType: "joystick", buttonIndex: 1 },
    LeftDirection: { buttonType: "joystick", buttonIndex: 0 },
    RightSpeed: { buttonType: "joystick", buttonIndex: 3 },
    RightDirection: { buttonType: "joystick", buttonIndex: 2 },
    VacuumPulse: { buttonType: "button", buttonIndex: 0 },
  },
  Gimbal: {
    PanLeft: { buttonType: "joystick", buttonIndex: 0 },
    PanRight: { buttonType: "joystick", buttonIndex: 2 },
    TiltLeft: { buttonType: "joystick", buttonIndex: 1 },
    TiltRight: { buttonType: "joystick", buttonIndex: 3 },
    DPadU: { buttonType: "button", buttonIndex: 12 },
    DPadD: { buttonType: "button", buttonIndex: 13 },
    ButtonStartDebounced: { buttonType: "button", buttonIndex: 9 }, // based on the ButtonStartDebounced in InputManagerConfig, assigned value to the start button on the xbox controller. Not sure if that is the correct button
  },
  ScienceControls: {
    yDirection: { buttonType: "joystick", buttonIndex: 1 },
    xDirection: { buttonType: "joystick", buttonIndex: 0 },
  },
  ArmControls: {
    // some of these showed two instances of some buttons with the second one saying IK in the InputMangerConfig.cs
    PanLeft: { buttonType: "joystick", buttonIndex: 0 },
    PanRight: { buttonType: "joystick", buttonIndex: 2 },
    TiltLeft: { buttonType: "joystick", buttonIndex: 1 },
    TiltRight: { buttonType: "joystick", buttonIndex: 3 },
    ButtonA: { buttonType: "button", buttonIndex: 0 },
    ButtonB: { buttonType: "button", buttonIndex: 1 },
    ButtonX: { buttonType: "button", buttonIndex: 2 },
    ButtonY: { buttonType: "button", buttonIndex: 3 },
    LeftBumeper: { buttonType: "button", buttonIndex: 4 },
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
}
