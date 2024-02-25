import React, { Component } from 'react';
import CSS from 'csstype';
import IK from './components/IK';
import Angular from './components/Angular';
import Cameras from '../../Core/components/Cameras';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import ControlFeatures from './components/ControlFeatures';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexGrow: 1,
  justifyContent: 'space-between',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
};

interface IProps {}

interface IState {
  gripperToggle: boolean;
}

const MultiplierX = 1000;
const MultiplierY1 = 1000;
const MultiplierY2 = 1000;
const MultiplierZ = 1000;
const MultiplierPitch = 1000;
const MultiplierR1 = 1000;
const MultiplierR2 = 1000;
const MultiplierGripper = 1000;
// let MultiplierEndEffector: number;

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      gripperToggle: false,
    };
    this.setGripper = this.setGripper.bind(this);
    this.arm = this.arm.bind(this);
    setInterval(() => this.arm(), 100);
  }

  setGripper() {
    this.setState((prevState) => ({ gripperToggle: !prevState.gripperToggle }));
  }

  arm(): void {
    let X = 0;
    let Y1 = 0;
    let Y2 = 0;
    let Z = 0;
    let Pitch = 0;
    let R1 = 0;
    let R2 = 0;
    let moveArm = false;

    // if (controllerInputs.MultiplierY) {
    //   MultiplierX = controllerInputs.Multiplier1;
    //   MultiplierY1 = controllerInputs.Multiplier2;
    //   MultiplierY2 = controllerInputs.Multiplier3;
    //   MultiplierZ = controllerInputs.Multiplier4;
    // } else if (controllerInputs.MultiplierX) {
    //   MultiplierPitch = controllerInputs.Multiplier1;
    //   MultiplierR1 = controllerInputs.Multiplier2;
    //   MultiplierR2 = controllerInputs.Multiplier3;
    //   MultiplierGripper = controllerInputs.Multiplier4;
    // }

    if ('WristPitchPlus' in controllerInputs && 'WristPitchMinus' in controllerInputs) {
      Pitch = (controllerInputs.WristPitchPlus - controllerInputs.WristPitchMinus) * MultiplierPitch;
      moveArm = true;
    }

    if ('Roll1Plus' in controllerInputs && 'Roll1Minus' in controllerInputs) {
      R1 = (controllerInputs.Roll1Plus - controllerInputs.Roll1Minus) * MultiplierR1;
      moveArm = true;
    }
    if ('Roll2Plus' in controllerInputs && 'Roll2Minus' in controllerInputs) {
      R2 = (controllerInputs.Roll2Plus - controllerInputs.Roll2Minus) * MultiplierR2;
      moveArm = true;
    }

    if ('XAxis' in controllerInputs) {
      X = controllerInputs.XAxis * MultiplierX;
      moveArm = true;
    }
    if ('Y1Axis' in controllerInputs) {
      Y1 = controllerInputs.Y1Axis * MultiplierY1;
      moveArm = true;
    }
    if ('Y2Axis' in controllerInputs) {
      Y2 = controllerInputs.Y2Axis * MultiplierY2;
      moveArm = true;
    }
    if ('ZAxis' in controllerInputs) {
      Z = controllerInputs.ZAxis * MultiplierZ;
      moveArm = true;
    }

    if (Math.abs(X) > Math.abs(Y1)) {
      Y1 = 0;
    } else {
      X = 0;
    }

    if (Math.abs(Y2) > Math.abs(Z)) {
      Z = 0;
    } else {
      Y2 = 0;
    }

    if (moveArm) {
      const armValues = [X, Y1, Y2, Z, Pitch, R1, R2];
      console.log(armValues);
      rovecomm.sendCommand('OpenLoop', armValues);
    }

    if ('GripperOpen' in controllerInputs && 'GripperClose' in controllerInputs) {
      let Gripper1 = 0;
      if (controllerInputs.GripperOpen === 1) {
        Gripper1 = 1 * MultiplierGripper;
      } else if (controllerInputs.GripperClose === 1) {
        Gripper1 = -1 * MultiplierGripper;
      } else {
        Gripper1 = 0;
      }
      console.log('Moving Gripper 1');
      rovecomm.sendCommand('Gripper', Gripper1);
    }

    if ('Solenoid' in controllerInputs) {
      rovecomm.sendCommand('Solenoid', controllerInputs.Solenoid);
    }
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <Angular style={{ flex: 1, marginRight: '2.5px' }} />
          <div style={{ ...column, flex: 1, marginLeft: '2.5px' }}>
            <IK />
            <ControlFeatures gripperCallBack={this.setGripper} style={{ height: '100%' }} />
          </div>
        </div>
        <div style={row}>
          <Cameras defaultCamera={5} style={{ width: '50%', marginRight: '2.5px' }} />
          <Cameras defaultCamera={6} style={{ width: '50%', marginLeft: '2.5px' }} />
        </div>
        <div style={row}>
          <ControlScheme configs={['Arm']} style={{ width: '50%', marginRight: '2.5px' }} />
        </div>
        <Cameras defaultCamera={7} />
      </div>
    );
  }
}
export default Arm;
