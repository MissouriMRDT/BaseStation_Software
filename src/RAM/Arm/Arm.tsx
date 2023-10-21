import React, { Component } from 'react';
import CSS from 'csstype';
import IK from './components/IK';
import Angular from './components/Angular';
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

let MultiplierJ1: number;
let MultiplierJ2: number;
let MultiplierJ3: number;
let MultiplierJ4: number;
let MultiplierJ5: number;
let MultiplierJ6: number;
let MultiplierGripper: number;

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
    let ArmWristBend = 0;
    let ArmWristTwist = 0;
    let ArmElbowTwist = 0;
    let ArmElbowBend = 0;
    let ArmBaseTwist = 0;
    let ArmBaseBend = 0;
    let moveArm = false;

    if (controllerInputs.MultiplierY) {
      MultiplierJ1 = controllerInputs.Multiplier1;
      MultiplierJ2 = controllerInputs.Multiplier2;
      MultiplierJ3 = controllerInputs.Multiplier3;
      MultiplierJ4 = controllerInputs.Multiplier4;
    } else if (controllerInputs.MultiplierX) {
      MultiplierJ5 = controllerInputs.Multiplier1;
      MultiplierJ6 = controllerInputs.Multiplier2;
      MultiplierGripper = controllerInputs.Multiplier3;
    }

    // J5
    if ('WristBendLeft' in controllerInputs && 'WristBendRight' in controllerInputs) {
      ArmWristBend = (controllerInputs.WristBendLeft - controllerInputs.WristBendRight) * MultiplierJ5;
      moveArm = true;
    }

    // J6
    if ('WristTwistLeft' in controllerInputs && 'WristTwistRight' in controllerInputs) {
      ArmWristTwist = (controllerInputs.WristTwistLeft - controllerInputs.WristTwistRight) * MultiplierJ6;
      moveArm = true;
    }

    // J3
    if ('ElbowBend' in controllerInputs) {
      ArmElbowBend = controllerInputs.ElbowBend * MultiplierJ3;
      moveArm = true;
    }

    // J4
    if ('ElbowTwist' in controllerInputs) {
      ArmElbowTwist = controllerInputs.ElbowTwist * MultiplierJ4;
      moveArm = true;
    }

    // J2
    if ('BaseBend' in controllerInputs) {
      ArmBaseBend = controllerInputs.BaseBend * MultiplierJ2;
      moveArm = true;
    }

    // J1
    if ('BaseTwist' in controllerInputs) {
      ArmBaseTwist = controllerInputs.BaseTwist * MultiplierJ1;
      moveArm = true;
    }

    if (Math.abs(ArmBaseBend) > Math.abs(ArmBaseTwist)) {
      ArmBaseTwist = 0;
    } else {
      ArmBaseBend = 0;
    }

    if (Math.abs(ArmElbowBend) > Math.abs(ArmElbowTwist)) {
      ArmElbowTwist = 0;
    } else {
      ArmElbowBend = 0;
    }

    if (moveArm) {
      const armValues = [ArmBaseTwist, ArmBaseBend, ArmElbowBend, ArmElbowTwist, ArmWristTwist, ArmWristBend];
      console.log(armValues);
      rovecomm.sendCommand('ArmVelocityControl', armValues);
    }

    if ('GripperOpen' in controllerInputs && 'GripperClose' in controllerInputs) {
      let Gripper1 = 0;
      let Gripper2 = 0;
      if (this.state.gripperToggle) {
        if (controllerInputs.GripperOpen === 1) {
          Gripper2 = 1 * MultiplierGripper;
        } else if (controllerInputs.GripperClose === 1) {
          Gripper2 = -1 * MultiplierGripper;
        } else {
          Gripper2 = 0;
        }
        console.log('Moving Gripper 2');
      } else {
        if (controllerInputs.GripperOpen === 1) {
          Gripper1 = 1 * MultiplierGripper;
        } else if (controllerInputs.GripperClose === 1) {
          Gripper1 = -1 * MultiplierGripper;
        } else {
          Gripper1 = 0;
        }
        console.log('Moving Gripper 1');
      }
      rovecomm.sendCommand('GripperMove', [Gripper1, Gripper2]);
    }

    if ('EndEffectorOn' in controllerInputs) {
      let EndEffector = 0;
      if (controllerInputs.EndEffectorOn === 1) {
        EndEffector = 1;
      }
      rovecomm.sendCommand('EndEffector', EndEffector);
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
          <ControlScheme configs={['Arm']} style={{ width: '50%', marginRight: '2.5px' }} />
        </div>
      </div>
    );
  }
}
export default Arm;
