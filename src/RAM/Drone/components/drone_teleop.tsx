import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm, RovecommManifest } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexDirection: 'column',
  width: '40%',
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

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  alignItems: 'center',
};

/* const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
}; */

const button: CSS.Properties = {
  margin: '10px',
  width: '40',
  height: '25px',
};

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  zedAngle: number;
}

class DroneTeleop extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  static teleop(): void {
    rovecomm.sendCommand('droneStateDisplay', RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Teleop);
  }

  static autonomy(): void {
    rovecomm.sendCommand('droneStateDisplay', RovecommManifest.Multimedia.Enums.DISPLAYSTATE.Autonomy);
  }

  static reachedGoal(): void {
    rovecomm.sendCommand('droneStateDisplay', RovecommManifest.Mulitmedia.Enums.DISPLAYSTATE.reached_Goal);
  }

  static incZedAngle(prevState: IState, amount: number): void {
    const angle = amount + prevState.zedAngle <= 45.0 ? prevState.zedAngle + amount : 45.0;
    rovecomm.sendCommand('droneZedAngle', angle);
  }

  static decZedAngle(prevState: IState, amount: number): void {
    const angle = amount - prevState.zedAngle >= 0 ? prevState.zedAngle - amount : 0.0;
    rovecomm.sendCommand('droneZedAngle', angle);
  }

  constructor(props: IProps) {
    super(props);
    this.state = {
      zedAngle: 0.0,
    };
  }

  setZedAngle(angle: number): void {
    this.setState({ zedAngle: angle });
    rovecomm.sendCommand('droneZedAngle', angle);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Zed/Teleop Controls</div>
        <div style={container}>
          <div style={{ ...row, marginLeft: '14px' }}>
            <div style={{ ...button, width: '80px' }} onClick={DroneTeleop.teleop}>
              Teleop
            </div>
            <div style={{ ...button, width: '80px' }} onClick={DroneTeleop.autonomy}>
              Autonomy
            </div>
            <div style={{ ...button, width: '80px' }} onClick={DroneTeleop.reachedGoal}>
              Goal
            </div>
          </div>
          <div style={{ ...row, marginLeft: '14px' }}>
            <div> Zed Angle: {this.state.zedAngle}°</div>
          </div>
          <div style={{ ...row, marginLeft: '14px' }}>
            <div style={{ ...button, width: '40px' }} onClick={() => DroneTeleop.decZedAngle(this.state, 10)}>
              -10
            </div>
            <div
              style={{ ...button, width: '40px', paddingLeft: '10px' }}
              onClick={() => DroneTeleop.decZedAngle(this.state, 5)}
            >
              -5
            </div>
            <div style={{ ...button, width: '80px', paddingLeft: '15px' }} onClick={() => this.setZedAngle(0.0)}>
              Reset Angle
            </div>
            <div
              style={{ ...button, width: '40px', paddingLeft: '15px' }}
              onClick={() => DroneTeleop.incZedAngle(this.state, 5)}
            >
              5
            </div>
            <div
              style={{ ...button, width: '40px', paddingLeft: '10px' }}
              onClick={() => DroneTeleop.incZedAngle(this.state, 10)}
            >
              10
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default DroneTeleop;
