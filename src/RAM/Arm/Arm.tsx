import React, { Component } from 'react';
import CSS from 'csstype';
import Angular from './components/Angular';
import ControlScheme, { controllerInputs } from '../../Core/components/ControlScheme';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import ControlFeatures from './components/ControlFeatures';
import CameraControls from '../../RED/components/CameraControls';

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
  gripperCam: number;
}

let MultiplierX = 1000;
let MultiplierY1 = 1000;
let MultiplierY2 = 1000;
let MultiplierZ = 1000;
let MultiplierPitch = 500;
let MultiplierR = 500;
let MultiplierGripper = 1000;
// let MultiplierEndEffector: number;

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      gripperToggle: false,
      gripperCam: 7,
    };
    this.setGripper = this.setGripper.bind(this);
    this.arm = this.arm.bind(this);
    setInterval(() => this.arm(), 100);
  }

  isGripperToggling = false;

  setGripper() {
    if (this.isGripperToggling) {
      return;
    }

    this.isGripperToggling = true;

    this.setState((prevState) => ({ gripperToggle: !prevState.gripperToggle }));
    rovecomm.sendCommand('SelectGripper', 'Arm', this.state.gripperToggle ? [1] : [0]);
    if (!this.state.gripperToggle) {
      this.setState({ gripperCam: 7 });
      console.log(this.state.gripperCam);
      MultiplierR = 500;
    } else {
      this.setState({ gripperCam: 8 });
      console.log(this.state.gripperCam);
      MultiplierR = 1000;
    }

    setTimeout(() => {
      this.isGripperToggling = false;
    }, 300); // 300ms delay
  }

  arm(): void {
    let X = 0;
    let Y1 = 0;
    let Y2 = 0;
    let Z = 0;
    let Pitch = 0;
    let R = 0;
    let moveArm = false;
    let LaserToggle = 0;

    if (controllerInputs.MultiplierY) {
      MultiplierX = controllerInputs.Multiplier1;
      MultiplierY1 = controllerInputs.Multiplier2;
      MultiplierY2 = controllerInputs.Multiplier3;
      MultiplierZ = controllerInputs.Multiplier4;
    } else if (controllerInputs.MultiplierX) {
      MultiplierPitch = controllerInputs.Multiplier1;
      MultiplierR = controllerInputs.Multiplier2;
      MultiplierGripper = controllerInputs.Multiplier3;
    }

    if ('WristPitchPlus' in controllerInputs && 'WristPitchMinus' in controllerInputs) {
      Pitch = (controllerInputs.WristPitchPlus - controllerInputs.WristPitchMinus) * MultiplierPitch;
      moveArm = true;
    }

    if ('GripperToggle' in controllerInputs) {
      if (controllerInputs.GripperToggle === 1) {
        this.setGripper();
      }
    }

    if ('RollPlus' in controllerInputs && 'RollMinus' in controllerInputs) {
      R = (controllerInputs.RollPlus - controllerInputs.RollMinus) * MultiplierR;
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
      const armValues = [X, Y1, Y2, Z, Pitch, R];
      console.log(armValues);
      rovecomm.sendCommand('OpenLoop', 'Arm', armValues);
    }

    if ('GripperOpen' in controllerInputs && 'GripperClose' in controllerInputs) {
      let Gripper = 0;
      if (controllerInputs.GripperOpen === 1) {
        Gripper = 1 * MultiplierGripper;
      } else if (controllerInputs.GripperClose === 1) {
        Gripper = -1 * MultiplierGripper;
      } else {
        Gripper = 0;
      }
      rovecomm.sendCommand('Gripper', 'Arm', Gripper);
    }

    if ('SolenoidOn' in controllerInputs) {
      rovecomm.sendCommand('Solenoid', 'Arm', controllerInputs.SolenoidOn);
    }

    if ('LaserToggle' in controllerInputs) {
      if (controllerInputs.LaserToggle === 1) {
        if (LaserToggle === 1) {
          LaserToggle = 0;
        } else {
          LaserToggle = 1;
        }
      }
      rovecomm.sendCommand('Laser', 'Arm', LaserToggle);
    }
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <Angular style={{ flex: 1, marginRight: '2.5px' }} />
          <div style={{ ...column, flex: 1, marginLeft: '2.5px' }}>
            {/* <IK /> */}
            <ControlFeatures
              gripperCallBack={this.setGripper}
              gripperToggle={this.state.gripperToggle}
              style={{ height: '100%' }}
            />
          </div>
        </div>
        <div style={row}>
          {/* Note: sources start at 0, but begin with 1 visually on the buttons*/}
          <CameraControls startSource={4} canvasWidth={640} canvasHeight={480} labelName={'Arm Cam 1'}></CameraControls>
          <CameraControls startSource={5} canvasWidth={640} canvasHeight={480} labelName={'Arm Cam 2'}></CameraControls>
        </div>
        <div style={row}>
          <ControlScheme configs={['Arm']} style={{ width: '100%', marginRight: '2.5px' }} />
        </div>
        <CameraControls
          startSource={6}
          canvasWidth={640}
          canvasHeight={480}
          labelName={'Arm Cam 3'}
          gripperCam={this.state.gripperCam}
          cameraToggle={true}
        />
      </div>
    );
  }
}
export default Arm;
