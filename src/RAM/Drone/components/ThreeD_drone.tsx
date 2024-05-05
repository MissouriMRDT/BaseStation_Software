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
  //height: 'calc(100% - 47px)',
  alignItems: 'center',
  overflow: 'hidden',
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
const canvasContainer: CSS.Properties = {
  position: 'relative',
  width: '100%',
  height: '100%',
  minHeight: '400px',
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

    this.resizeCallback.bind(this);
    this.setResizeCallbacks();
  }

  componentDidMount(): void {
    this.loadGeometry();
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

  resizeCallback = () => this.findWidth();

  setResizeCallbacks() {
    for (const win of Object.keys(windows)) {
      windows[win].addEventListener('resize', this.resizeCallback);
    }
    this.findWidth();
  }

  removeResizeCallbacks() {
    for (const win of Object.keys(windows)) {
      windows[win].removeEventListener('resize', this.resizeCallback);
    }
    this.findWidth();
  }

  findWidth() {
    for (const win of Object.keys(windows)) {
      if (
        windows[win].document.getElementById(this.state.id) &&
        (windows[win].document.getElementById(this.state.id).clientWidth !== this.state.width ||
          windows[win].document.getElementById(this.state.id).clientHeight !== this.state.height)
      ) {
        this.setState((prevState) => ({
          width: windows[win].document.getElementById(prevState.id).clientWidth,
          height: windows[win].document.getElementById(prevState.id).clientHeight,
        }));
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
    this.removeResizeCallbacks();
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Drone</div>
        <div style={container}>
          <div style={canvasContainer} id={this.state.id}>
            <div style={{ width: this.state.width, height: this.state.height, position: 'absolute', top: '0px' }}>
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
      </div>
    );
  }
}

export default ThreeDdrone;
