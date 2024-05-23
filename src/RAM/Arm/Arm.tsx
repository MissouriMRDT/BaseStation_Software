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
const h1Style: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '12px',
};
const input: CSS.Properties = {
  width: '75%',
};
const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
};
const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  top: '24px',
  left: '3px',
  fontFamily: 'arial',
  fontSize: '16px',
  zIndex: 1,
  color: 'white',
};
const joints: CSS.Properties = {
  display: 'grid',
  gridRowStart: '2 & {}',
  grid: 'repeat(2, 28px) / auto-flow dense',
  margin: '10px 10px',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
};

interface Joint {
  [key: string]: number;
  X: number;
  Y1: number;
  Y2: number;
  Z: number;
  Pitch: number;
  R: number;
  Gripper: number;
  Master: number;
}

interface IProps {}

interface IState {
  multiplierValues: Joint;
  gripperToggle: boolean;
  gripperCam: number;
  laserToggle: boolean;
}

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {
      gripperToggle: false,
      gripperCam: 6,
      laserToggle: false,
      multiplierValues: {
        X: 1000,
        Y1: 1000,
        Y2: 1000,
        Z: 1000,
        Pitch: 500,
        R: 500,
        Gripper: 1000,
        Master: 1,
      },
    };
    this.setGripper = this.setGripper.bind(this);
    this.arm = this.arm.bind(this);
    this.setLaser = this.setLaser.bind(this);
    this.handleMultiplierChange = this.handleMultiplierChange.bind(this);
    setInterval(() => this.arm(), 100);
  }

  isGripperToggling = false;

  handleMultiplierChange(joint: string, e: React.ChangeEvent<HTMLInputElement>) {
    let value = Number(e.target.value);
    value = Math.max(0, Math.min(value, 1000)); // Ensures the value is between 0 and 1000
    this.setState((prevState) => ({
      multiplierValues: {
        ...prevState.multiplierValues,
        [joint]: value,
      },
    }));
  }

  setGripper() {
    if (this.isGripperToggling) {
      return;
    }
    this.setState((prevState) => ({ gripperToggle: !prevState.gripperToggle }));
    this.isGripperToggling = true;
    rovecomm.sendCommand('SelectGripper', 'Arm', this.state.gripperToggle ? [1] : [0]);
    if (!this.state.gripperToggle) {
      this.setState({ gripperCam: 6 });
      console.log(this.state.gripperCam);
    } else {
      this.setState({ gripperCam: 7 });
      console.log(this.state.gripperCam);
    }
    setTimeout(() => {
      this.isGripperToggling = false;
    }, 300); // 300ms delay
  }

  isLaserToggling = false;

  setLaser() {
    if (this.isLaserToggling) {
      return;
    }

    this.isLaserToggling = true;

    this.setState((prevState) => ({ laserToggle: !prevState.laserToggle }));
    console.log(this.state.laserToggle);

    setTimeout(() => {
      this.isLaserToggling = false;
    }, 300); // 300ms delay
  }

  arm(): void {
    let X = 0;
    let Y1 = 0;
    let Y2 = 0;
    let Z = 0;
    let Pitch = 0;
    let R = 0;
    let Gripper = 0;
    let moveArm = false;

    if (
      'MultiplierY' in controllerInputs &&
      'MultiplierX' in controllerInputs &&
      'Multiplier1' in controllerInputs &&
      'Multiplier2' in controllerInputs &&
      'Multiplier3' in controllerInputs &&
      'Multiplier4' in controllerInputs
    ) {
      if (controllerInputs.MultiplierY) {
        this.setState((prevState) => ({
          multiplierValues: {
            ...prevState.multiplierValues,
            X: (prevState.multiplierValues.Master * (controllerInputs.Multiplier1 + 1)) / 0.002,
            Y1: (prevState.multiplierValues.Master * (controllerInputs.Multiplier2 + 1)) / 0.002,
            Y2: (prevState.multiplierValues.Master * (controllerInputs.Multiplier3 + 1)) / 0.002,
            Z: (prevState.multiplierValues.Master * (controllerInputs.Multiplier4 + 1)) / 0.002,
          },
        }));
      } else if (controllerInputs.MultiplierX) {
        this.setState((prevState) => ({
          multiplierValues: {
            ...prevState.multiplierValues,
            Pitch: (prevState.multiplierValues.Master * (controllerInputs.Multiplier1 + 1)) / 0.002,
            R: (prevState.multiplierValues.Master * (controllerInputs.Multiplier2 + 1)) / 0.002,
            Gripper: (prevState.multiplierValues.Master * (controllerInputs.Multiplier3 + 1)) / 0.002,
            Master: (controllerInputs.Multiplier4 + 1) / 2,
          },
        }));
      }
    }

    if ('WristPitchPlus' in controllerInputs && 'WristPitchMinus' in controllerInputs) {
      Pitch = (controllerInputs.WristPitchPlus - controllerInputs.WristPitchMinus) * this.state.multiplierValues.Pitch;
      moveArm = true;
    }

    if ('GripperToggle' in controllerInputs) {
      if (controllerInputs.GripperToggle === 1) {
        this.setGripper();
      }
    }

    if ('RollPlus' in controllerInputs && 'RollMinus' in controllerInputs) {
      R = (controllerInputs.RollPlus - controllerInputs.RollMinus) * this.state.multiplierValues.R;
      moveArm = true;
    }

    if ('XAxis' in controllerInputs) {
      X = controllerInputs.XAxis * this.state.multiplierValues.X;
      moveArm = true;
    }
    if ('Y1Axis' in controllerInputs) {
      Y1 = controllerInputs.Y1Axis * this.state.multiplierValues.Y1;
      moveArm = true;
    }
    if ('Y2Axis' in controllerInputs) {
      Y2 = controllerInputs.Y2Axis * this.state.multiplierValues.Y2;
      moveArm = true;
    }
    if ('ZAxis' in controllerInputs) {
      Z = controllerInputs.ZAxis * this.state.multiplierValues.Z;
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
      if (controllerInputs.GripperOpen === 1) {
        Gripper = 1 * this.state.multiplierValues.Gripper;
      } else if (controllerInputs.GripperClose === 1) {
        Gripper = -1 * this.state.multiplierValues.Gripper;
      } else {
        Gripper = 0;
      }
      rovecomm.sendCommand('Gripper', 'Arm', Gripper);
    }

    if ('SolenoidOn' in controllerInputs) {
      rovecomm.sendCommand('Solenoid', 'Arm', controllerInputs.SolenoidOn);
    }

    if ('LaserToggle' in controllerInputs) {
      rovecomm.sendCommand('Laser', 'Arm', this.state.laserToggle ? [1] : [0]);
      if (controllerInputs.LaserToggle === 1) {
        this.setLaser();
      }
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
          <CameraControls startSource={4} defaultWidth={640} labelName={'Arm Cam 1'}></CameraControls>
          <CameraControls startSource={5} defaultWidth={640} labelName={'Arm Cam 2'}></CameraControls>
        </div>
        <div style={row}>
          <ControlScheme configs={['Arm']} style={{ width: '100%', marginRight: '2.5px' }} />
        </div>
        <div style={label}>Slider Speeds</div>
        <div style={container}>
          <div style={joints}>
            {Object.keys(this.state.multiplierValues).map((joint) => {
              return (
                <div key={joint} style={row}>
                  <h1 style={h1Style}>{joint}</h1>
                  <input
                    type="text"
                    style={input}
                    value={this.state.multiplierValues[joint] || ''}
                    onChange={(e) => {
                      this.handleMultiplierChange(joint, e);
                    }}
                  />
                </div>
              );
            })}
          </div>{' '}
        </div>
        <CameraControls
          startSource={this.state.gripperCam - 1}
          defaultWidth={640}
          labelName={'Arm Cam 3'}
          gripperCam={this.state.gripperCam}
          cameraToggle={true}
        />
      </div>
    );
  }
}
export default Arm;
