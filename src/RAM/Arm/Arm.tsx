import React, { Component } from 'react';
import CSS from 'csstype';
import ControlMultipliers, { controlMultipliers } from './components/ControlMultipliers';
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

function arm(): void {
  let ArmWristBend = 0;
  let ArmWristTwist = 0;
  let ArmElbowTwist = 0;
  let ArmElbowBend = 0;
  let ArmBaseTwist = 0;
  let ArmBaseBend = 0;
  let moveArm = false;

  if ('WristBendLeft' in controllerInputs && 'WristBendRight' in controllerInputs) {
    ArmWristBend = (controllerInputs.WristBendLeft - controllerInputs.WristBendRight) * controlMultipliers.Wrist;
    moveArm = true;
  }

  if ('WristTwistLeft' in controllerInputs && 'WristTwistRight' in controllerInputs) {
    ArmWristTwist = (controllerInputs.WristTwistLeft - controllerInputs.WristTwistRight) * controlMultipliers.Wrist;
    moveArm = true;
  }

  if ('ElbowBend' in controllerInputs && 'ElbowTwist' in controllerInputs) {
    ArmElbowBend = controllerInputs.ElbowBend * controlMultipliers.Elbow;
    ArmElbowTwist = controllerInputs.ElbowTwist * controlMultipliers.Elbow;
    moveArm = true;
  }

  if ('BaseBend' in controllerInputs && 'BaseTwist' in controllerInputs) {
    ArmBaseTwist = controllerInputs.BaseTwist * controlMultipliers.Base;
    ArmBaseBend = controllerInputs.BaseBend * controlMultipliers.Base;
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
    let Gripper = 0;
    if (controllerInputs.GripperOpen === 1) {
      Gripper = 1 * controlMultipliers.Gripper;
    } else if (controllerInputs.GripperClose === 1) {
      Gripper = -1 * controlMultipliers.Gripper;
    } else {
      Gripper = 0;
    }
    rovecomm.sendCommand('GripperMove', Gripper);
  }

  if ('EndEffectorOn' in controllerInputs && 'EndEffectorOff' in controllerInputs) {
    let EndEffector = 0;
    if (controllerInputs.EndEffectorOn === 1) {
      EndEffector = 1 * controlMultipliers.EndEffector;
    } else if (controllerInputs.EndEffectorOff === 1) {
      EndEffector = -1 * controlMultipliers.EndEffector;
    } else {
      EndEffector = 0;
    }
    rovecomm.sendCommand('EndEffector', EndEffector);
  }
}

interface IProps {}

interface IState {}

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {};

    setInterval(() => arm(), 100);
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <Angular style={{ flex: 1, marginRight: '2.5px' }} />
          <div style={{ ...column, flex: 1, marginLeft: '2.5px' }}>
            <IK />
            <ControlFeatures style={{ height: '100%' }} />
          </div>
        </div>
        <div style={row}>
          <Cameras defaultCamera={5} style={{ width: '50%', marginRight: '2.5px' }} />
          <Cameras defaultCamera={6} style={{ width: '50%', marginLeft: '2.5px' }} />
        </div>
        <ControlMultipliers />
        <ControlScheme configs={['Arm']} />
        <Cameras defaultCamera={7} />
      </div>
    );
  }
}
export default Arm;
