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

const button: CSS.Properties = {
  fontFamily: 'arial',
  flexGrow: 1,
  margin: '5px',
  fontSize: '14px',
  lineHeight: '24px',
  borderWidth: '2px',
  height: '40px',
};
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  alignItems: 'center',
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

  incZedAngle(prevState: IState, amount: number): void {
    const angle = prevState.zedAngle + amount <= 45.0 ? prevState.zedAngle + amount : 45.0;
    rovecomm.sendCommand('droneZedAngle', angle);
    this.setZedAngle(angle);
  }

  decZedAngle(prevState: IState, amount: number): void {
    const angle = prevState.zedAngle - amount >= 0 ? prevState.zedAngle - amount : 0.0;
    rovecomm.sendCommand('droneZedAngle', angle);
    this.setZedAngle(angle);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Zed/Teleop Controls</div>
        <div style={container}>
          <div style={{ ...row, marginLeft: '5px', marginTop: '10px', marginBottom: '10px' }}>
            <button type="button" style={{ ...button, width: '80px' }} onClick={DroneTeleop.teleop}>
              Teleop
            </button>
            <button type="button" style={{ ...button, width: '80px' }} onClick={DroneTeleop.autonomy}>
              Autonomy
            </button>
            <button type="button" style={{ ...button, width: '80px' }} onClick={DroneTeleop.reachedGoal}>
              Goal
            </button>
          </div>
          <div style={{ ...row, marginLeft: '10px', marginTop: '10px', marginBottom: '10px' }}>
            <div> Zed Angle: {this.state.zedAngle}°</div>
          </div>
          <div style={{ ...row, marginLeft: '5px', marginTop: '10px', marginBottom: '10px' }}>
            <button type="button" style={{ ...button, width: '40px' }} onClick={() => this.decZedAngle(this.state, 10)}>
              -10
            </button>
            <button
              type="button"
              style={{ ...button, width: '40px', paddingLeft: '10px' }}
              onClick={() => this.decZedAngle(this.state, 5)}
            >
              -5
            </button>
            <button
              type="button"
              style={{ ...button, width: '40px', paddingLeft: '15px' }}
              onClick={() => this.decZedAngle(this.state, 1)}
            >
              -1
            </button>
            <button
              type="button"
              style={{ ...button, width: '80px', paddingLeft: '15px', fontSize: '12px' }}
              onClick={() => this.setZedAngle(0.0)}
            >
              Reset Angle
            </button>
            <button
              type="button"
              style={{ ...button, width: '40px', paddingLeft: '15px' }}
              onClick={() => this.incZedAngle(this.state, 1)}
            >
              1
            </button>
            <button
              type="button"
              style={{ ...button, width: '40px', paddingLeft: '15px' }}
              onClick={() => this.incZedAngle(this.state, 5)}
            >
              5
            </button>
            <button
              type="button"
              style={{ ...button, width: '40px', paddingLeft: '10px' }}
              onClick={() => this.incZedAngle(this.state, 10)}
            >
              10
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default DroneTeleop;
