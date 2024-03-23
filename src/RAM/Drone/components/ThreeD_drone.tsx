import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import STLViewer from '../../../Core/components/STLViewer';
import { windows } from '../../../Core/Window';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '5px',
  alignItems: 'center',
  height: 'calc(100% - 47px)',
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
  zoom?: number;
  //droneOrientation: { pitch: number; yaw: number; roll: number };
}

interface IState {
  pitch: number;
  yaw: number;
  roll: number;
  id: string;
  width: number;
  height: number;
}

const MODEL = path.join(__dirname, '../assets/drone.stl');

class ThreeDdrone extends Component<IProps, IState> {
  static defaultProps = {
    zoom: 10,
    style: {},
  };

  static id = 0;

  constructor(props: any) {
    super(props);
    this.state = {
      pitch: 0,
      yaw: 0,
      roll: 0,
      id: `3Ddrone_${ThreeDdrone.id}`,
      width: 300,
      height: 150,
    };

    rovecomm.on('droneOrientation', (data: number[]) => this.droneData(data));
  }

  droneData(data: any) {
    let a: number;
    let b: number;
    let c: number;
    ({ pitch: a, yaw: b, roll: c } = this.state);
    a = (data[0] / 360) * (2 * Math.PI);
    b = (data[1] / 360) * (2 * Math.PI);
    c = (data[2] / 360) * (2 * Math.PI);
    this.setState({ pitch: a, yaw: b, roll: c });
  }

  findWidth() {
    for (const win of Object.keys(windows)) {
      if (windows[win].document.getElementById(this.state.id)) {
        if (
          this.state.width !== windows[win].document.getElementById(this.state.id).clientWidth - 10 ||
          this.state.height !== windows[win].document.getElementById(this.state.id).clientHeight - 12
        ) {
          windows[win].addEventListener('resize', () => this.findWidth());
          this.setState((prevState) => ({
            width: windows[win].document.getElementById(prevState.id).clientWidth - 10,
            height: windows[win].document.getElementById(prevState.id).clientHeight - 12,
          }));
        }
      }
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Drone</div>
        <div style={container} id={this.state.id} />
        <STLViewer
          model={MODEL}
          modelColor="#B92C2C"
          backgroundColor="#FFFFFF"
          rotate={false}
          rotation={[this.state.pitch, this.state.yaw, this.state.roll]}
          orbitControls
          width={this.state.width}
          height={this.state.height}
          zoom={this.props.zoom}
        />
      </div>
    );
  }
}

export default ThreeDdrone;
