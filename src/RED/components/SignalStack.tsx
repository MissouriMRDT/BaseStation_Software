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
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  flexShrink: 1,
  marginRight: '5px',
  marginBottom: '5px', // Add vertical spacing
};
const textStyle: CSS.Properties = {
  marginBottom: '5px',
  marginTop: '5px',
  marginLeft: '5px',
  justifyContent: 'center',
};
const buttonContainerStyle: CSS.Properties = {
  display: 'flex',
  justifyContent: 'center',
  gap: '5px',
  marginBottom: '5px',
};
const imageStyle: CSS.Properties = {
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
};
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
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

const signalMotorMultiplier = 300;

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
      basestationLat: 1,
      basestationLon: 1,
      toggle: false,
    };
    this.updateCoords = this.updateCoords.bind(this);
    setInterval(() => this.signalstack(), 100);
  }

  componentWillUnmount() {
    clearInterval(this.state.interval);
  }

  updateCoords() {
    const basestationLatHTML = document.getElementById('baselat') as HTMLTextAreaElement;
    const basestationLat = parseFloat(basestationLatHTML.value);
    const basestationLonHTML = document.getElementById('baselon') as HTMLTextAreaElement;
    const basestationLon = parseFloat(basestationLonHTML.value);

    if (!Number.isNaN(basestationLat) && !Number.isNaN(basestationLon)) {
      this.setState({ basestationLat });
      this.setState({ basestationLon });
    }
  }

  calculateAngle() {
    const targetAngle: number =
      Math.atan2(this.props.roverLon - this.state.basestationLon, this.props.roverLat - this.state.basestationLat) *
      (180 / Math.PI);
    // if (targetAngle === 0) {
    //   targetAngle = 90 - targetAngle;
    // } else if (targetAngle === 180) {
    //   targetAngle = 270 - targetAngle;
    // }
    return targetAngle;
  }

  rotateArrow(angle: number) {
    const arrow = document.getElementById('needle');
    if (arrow) {
      arrow.style.transform = 'rotate(' + angle + 'deg)';
    }
  }

  signalstack(): void {
    if ('Pan' in controllerInputs) {
      rovecomm.sendCommand('OpenLoop', 'SignalStack', controllerInputs.Pan * signalMotorMultiplier);
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Signal Stack</div>
        <div style={container}>
          <div style={imageStyle}>
            <img id="needle" src={this.state.image} alt={this.state.controlling} />
          </div>{' '}
          <div style={row}>
            <div style={{ ...column, width: '40%' }}>
              <p style={textStyle}>Rover Lat: {this.props.roverLat}</p>
              <p style={textStyle}>BaseStation Lat: {this.state.basestationLat}</p>
              <input style={textStyle} type="text" id="baselat" placeholder="Enter Basestation's Lat:" />
            </div>
            <div style={{ ...column, width: '40%' }}>
              <p style={textStyle}>Rover Lon: {this.props.roverLon}</p>
              <p style={textStyle}>BaseStation Lon: {this.state.basestationLon}</p>
              <input style={textStyle} type="text" id="baselon" placeholder="Enter Basestation's lon:" />
            </div>
          </div>
          <div style={buttonContainerStyle}>
            <button type="button" onClick={this.updateCoords}>
              Submit
            </button>
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
