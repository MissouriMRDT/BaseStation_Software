import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../../Core/RoveProtocol/Rovecomm';
import { controllerInputs } from '../../Core/components/ControlScheme';

const container: CSS.Properties = {
  // display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
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

interface IProps {
  style?: CSS.Properties;
  roverLat: number;
  roverLon: number;
}
interface IState {
  controlling: string;
  image: string;
  interval: NodeJS.Timeout;
  basestationLat: number;
  basestationLon: number;
  toggle: boolean;
}

// Dynamic paths to import images used to indicate which gimbal is being controlled
// const NotConnected = path.join(__dirname, '../assets/NotConnected.png');
const UpArrow = path.join(__dirname, '../assets/UpArrow.png');
// const DownArrow = path.join(__dirname, '../assets/DownArrow.png');

const signalMotorMultiplier = 1000;

class SignalStack extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      // Controlling will be "none" by default, then set to "main" or "drive", and image will update to match
      controlling: 'Main',
      image: UpArrow,
      interval: setInterval(() => this.signalstack(), 100),
      basestationLat: -1,
      basestationLon: -1,
      toggle: false,
    };
    this.latitudeChange = this.latitudeChange.bind(this);
  }

  componentWillUnmount() {
    clearInterval(this.state.interval);
  }

  latitudeChange(event: { target: { value: string } }) {
    let basestationLat = parseInt(event.target.value, 10);
    if (basestationLat < 0) {
      basestationLat = 0;
    }
    this.setState({ basestationLat });
  }

  longitudeChange(event: { target: { value: string } }) {
    let basestationLon = parseInt(event.target.value, 10);
    if (basestationLon < 0) {
      basestationLon = 0;
    }
    this.setState({ basestationLon });
  }

  calculateAngle() {
    let targetAngle: number = Math.atan(
      ((this.props.roverLat - this.state.basestationLat) / (this.props.roverLon - this.state.basestationLon)) *
        (180 / Math.PI)
    );
    if (this.props.roverLon - this.state.basestationLon > 0) {
      targetAngle = 90 - targetAngle;
    } else {
      targetAngle = 270 - targetAngle;
    }
    return targetAngle;
  }

  rotateArrow(angle: number) {
    const arrow = document.getElementById('needle');
    if (arrow) {
      arrow.style.transform = 'rotate(' + angle + 'deg)';
    }
  }

  signalstack(): void {
    const angle: number = this.calculateAngle();
    this.rotateArrow(angle);
    rovecomm.sendCommand('SetAngleTarget', 'SignalStack', angle);

    if ('Pan' in controllerInputs) {
      rovecomm.sendCommand('OpenLoop', 'SignalStack', controllerInputs.Pan * signalMotorMultiplier);
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Signal Stack</div>
        <div style={container}>
          <img id="needle" src={this.state.image} alt={this.state.controlling} />
          <div>
            <input type="text" id="baselat" value={this.state.basestationLat || ''} onChange={this.latitudeChange} />
            <input type="text" id="baselon" value={this.state.basestationLon || ''} onChange={this.latitudeChange} />
            <button type="button" onClick={() => this.setState({ toggle: !this.state.toggle })}>
              {this.state.toggle ? 'On' : 'Off'}
            </button>
          </div>
        </div>
      </div>
    );
  }
}
export default SignalStack;
