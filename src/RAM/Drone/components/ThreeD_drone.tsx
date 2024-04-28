import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import { windows } from '../../../Core/Window';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { Canvas } from '@react-three/fiber';
import { BufferGeometry, Euler } from 'three';
import { OrbitControls } from '@react-three/drei';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  padding: '0px',
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
  //droneOrientation: { pitch: number; yaw: number; roll: number };
}

interface IState {
  pitch: number;
  yaw: number;
  roll: number;
  id: string;
  width: number;
  height: number;
  geometry?: BufferGeometry;
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
      height: 300,
    };

    rovecomm.on('droneOrientation', (data: number[]) => this.droneData(data));
  }

  componentDidMount(): void {
    console.log('loading geometry');
    this.loadGeometry();
    console.log('finding width');
    this.findWidth();
  }

  droneData(data: number[]) {
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

  loadGeometry(): void {
    if (this.state.geometry === undefined) {
      const loader = new STLLoader();
      loader.load(MODEL, (geo) => {
        geo.center();
        geo.computeVertexNormals();
        this.setState({ geometry: geo });
      });
    }
  }

  componentWillUnmount(): void {
    this.state.geometry?.dispose();
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Drone</div>
        <div style={container} id={this.state.id}>
          <div style={{ width: this.state.width, height: this.state.height }}>
            <Canvas>
              <group rotation={new Euler(this.state.pitch, this.state.yaw, this.state.roll)}>
                <mesh geometry={this.state.geometry}>
                  <meshLambertMaterial color="#B92C2C" />
                </mesh>
              </group>
              <OrbitControls enablePan={false} minDistance={3} maxDistance={8} />
            </Canvas>
          </div>
        </div>
      </div>
    );
  }
}

export default ThreeDdrone;
