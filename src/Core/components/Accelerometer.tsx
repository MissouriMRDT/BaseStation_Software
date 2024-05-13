import React, { Component, Suspense } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../RoveProtocol/Rovecomm';
import { ArrowHelperProps, Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';
import * as THREE from 'three';
import RoverScene from './RoverScene';
import { windows } from '../Window';

const containersContainer: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};
const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: '3',
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
  flexGrow: '1',
  flexBasis: '200px',
  width: '100%',
};
const debugContainer: CSS.Properties = {
  flexGrow: 'auto',
  width: '100%',
};
const pitchYawOverlay: CSS.Properties = {
  backgroundColor: 'rgba(241, 241, 241, 0.5)',
  padding: '0.1em',
  fontWeight: 'bold',
  position: 'absolute',
  top: 0,
  left: 0,
};
const debugDialogOverlay: CSS.Properties = {
  backgroundColor: '#f1f1f1',
  opacity: '50%',
  padding: '0.1em',
  color: '#c1c1c1',
  cursor: 'pointer',
  position: 'absolute',
  bottom: 0,
  right: 0,
};
const updateDialogContainer: CSS.Properties = {
  backgroundColor: 'lightgreen',
  padding: '10px',
  marginTop: '10px',
  border: '2px solid green',
  borderRadius: '5px',
  cursor: 'pointer',
  fontFamily: 'Comic Sans MS',
  fontSize: '.8em',
};

const MODELS_PATH = path.join(__dirname, '../assets/models');

const DANGER_ANGLE = (50 * Math.PI) / 180;
const negativeY = new THREE.Vector3(0, -1, 0);
const positiveY = new THREE.Vector3(0, 1, 0);

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  id: string;
  width: number;
  height: number;
  upVector: THREE.Vector3;
  displayPitch: number;
  displayRoll: number;
  color: '#B92C2C' | '#363636';
  file: string;
  geometry?: THREE.BufferGeometry;
  showManualControls: boolean;
  showUpdateBox: boolean;
}

class Accelerometer extends Component<IProps, IState> {
  static defaultProps: IProps = {
    style: {},
  };

  private arrowRef: React.RefObject<ArrowHelperProps>;

  private roverRef: React.RefObject<THREE.Group>;

  static id = 0;

  constructor(props: IProps) {
    super(props);
    this.state = {
      // eslint-disable-next-line no-plusplus
      id: `3D_Rover${Accelerometer.id++}`,
      width: 300,
      height: 300,
      upVector: new THREE.Vector3(0, 1, 0),
      displayPitch: 0,
      displayRoll: 0,
      color: '#363636',
      file: path.join(MODELS_PATH, 'rover.stl'),
      showManualControls: false,
      showUpdateBox: false,
    };

    rovecomm.on('AccelerometerData', (data: number[]) => {
      // CoreBoard: y-forward, z-up, x-right
      // ThreeJS (WebGL): -z-forward, y-up, x-right
      this.setState({ upVector: new THREE.Vector3(data[0], data[2], -data[1]) });
      this.calcRotation();
    });

    this.arrowRef = React.createRef();
    this.roverRef = React.createRef();

    this.resizeCallback.bind(this);
    this.setResizeCallbacks();

    setTimeout(() => {
      this.setState({ showUpdateBox: true });
    }, 10000);
  }

  loadGeometry(): void {
    this.state.geometry?.dispose();
    if (this.state.file.endsWith('.stl')) {
      const loader = new STLLoader();
      loader.load(this.state.file, (geo) => {
        geo.center();
        geo.computeVertexNormals();
        this.setState({ geometry: geo });
      });
    }
  }

  componentDidMount(): void {
    this.loadGeometry();
    this.findWidth();
  }

  componentDidUpdate(_prevProps: Readonly<IProps>, prevState: Readonly<IState>): void {
    if (prevState.file !== this.state.file) {
      this.loadGeometry();
    }
    if (
      prevState.showManualControls !== this.state.showManualControls ||
      prevState.showUpdateBox !== this.state.showUpdateBox
    ) {
      this.findWidth();
    }
  }

  componentWillUnmount(): void {
    this.state.geometry?.dispose();
    this.removeResizeCallbacks();
  }

  // We must rotate the rover such that upVector would point to <0, -1, 0> if it underwent the same rotation.
  // We find the axis to rotate it around by crossing upVector with <0, -1, 0>,
  // We use a quaternion to represent the axis and angle, then convert it to euler angles.
  calcRotation(): void {
    const normalizedUp = this.state.upVector.clone().normalize();
    const axisAround = normalizedUp.clone().cross(positiveY).normalize();
    const angleAround = normalizedUp.angleTo(positiveY);
    let roll = Math.round((Math.asin(normalizedUp.x) / Math.PI) * 180);
    let pitch = Math.round((Math.asin(normalizedUp.z) / Math.PI) * 180);
    if (normalizedUp.y < 0) {
      roll = 180 - roll;
      pitch = 180 - pitch;
    }
    this.setState({
      displayPitch: -pitch,
      displayRoll: -roll,
      color: angleAround > DANGER_ANGLE ? '#B92C2C' : '#363636',
    });
    this.arrowRef.current?.setDirection?.(normalizedUp);
    if (normalizedUp.equals(negativeY)) this.roverRef.current?.setRotationFromEuler(new THREE.Euler(0, 0, Math.PI));
    else this.roverRef.current?.setRotationFromAxisAngle(axisAround, angleAround);
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

  render(): JSX.Element {
    return (
      <div style={{ ...this.props.style, ...containersContainer }}>
        <div style={label}>3D Rover</div>
        <div style={container}>
          <div style={canvasContainer} id={this.state.id}>
            <div style={{ width: this.state.width, height: this.state.height, position: 'absolute', top: '0px' }}>
              <Canvas shadows>
                <Suspense
                  fallback={
                    <mesh>
                      <boxGeometry args={[1, 1, 1]} />
                    </mesh>
                  }
                >
                  <directionalLight position-y={2} intensity={Math.PI * 0.5} castShadow />
                  <ambientLight intensity={0.1 * Math.PI} />
                  {this.state.showManualControls && (
                    <group>
                      <mesh position={[0, 3, 0]}>
                        <boxGeometry />
                        <meshBasicMaterial color={'black'} />
                      </mesh>
                      <arrowHelper
                        args={[this.state.upVector, new THREE.Vector3(0, 3, 0), 2, 'green', 0.3, 0.3]}
                        ref={this.arrowRef}
                      />
                    </group>
                  )}
                  <group ref={this.roverRef}>
                    {this.state.file.endsWith('.glb') ? (
                      <RoverScene position-y={-2} rotation-y={Math.PI} scale={1.7} color={this.state.color} />
                    ) : (
                      <mesh geometry={this.state.geometry} dispose={null} scale={0.08} castShadow>
                        <meshLambertMaterial color={this.state.color} />
                      </mesh>
                    )}
                  </group>
                  <mesh position-y={-3} rotation-x={-Math.PI * 0.5} scale={50} receiveShadow>
                    <planeGeometry />
                    <meshLambertMaterial color={'#c1c1c1'} />
                  </mesh>
                  <OrbitControls enablePan={false} minDistance={3} maxDistance={8} />
                </Suspense>
              </Canvas>
              <span style={pitchYawOverlay}>
                {`pitch: ${this.state.displayPitch}°`}
                <br />
                {`roll: ${this.state.displayRoll}°`}
              </span>
              <span
                style={debugDialogOverlay}
                onClick={() => this.setState({ showManualControls: !this.state.showManualControls })}
              >
                <small>{this.state.showManualControls ? 'Hide' : 'Show'} Debug Controls</small>
              </span>
            </div>
          </div>
          {this.state.showManualControls && (
            <div style={debugContainer}>
              <div style={{ display: 'flex', flexDirection: 'column' }}>
                <label>set x</label>
                <input
                  type="range"
                  min="-1"
                  max="1"
                  step="0.05"
                  value={this.state.upVector.x}
                  onChange={(event) => {
                    this.setState((prevState) => ({
                      upVector: prevState.upVector.setX(parseFloat(event.target.value)),
                    }));
                    this.calcRotation();
                  }}
                />
                <label>set y</label>
                <input
                  type="range"
                  min="-1"
                  max="1"
                  step="0.05"
                  value={this.state.upVector.y}
                  onChange={(event) => {
                    this.setState((prevState) => ({
                      upVector: prevState.upVector.setY(parseFloat(event.target.value)),
                    }));
                    this.calcRotation();
                  }}
                />
                <label>set z</label>
                <input
                  type="range"
                  min="-1"
                  max="1"
                  step="0.05"
                  value={this.state.upVector.z}
                  onChange={(event) => {
                    this.setState((prevState) => ({
                      upVector: prevState.upVector.setZ(parseFloat(event.target.value)),
                    }));
                    this.calcRotation();
                  }}
                />
                <div>{`up: <${String(
                  Object.values(this.state.upVector).map((value) => Number(value).toFixed(2))
                )}>`}</div>
              </div>
            </div>
          )}
        </div>
        {this.state.file.endsWith('.stl') && this.state.showUpdateBox && (
          <div
            style={updateDialogContainer}
            onClick={() => {
              this.setState({ showUpdateBox: false, file: path.join(MODELS_PATH, 'rover_preview.glb') });
              this.findWidth();
            }}
          >
            <div
              style={{ cursor: 'pointer', float: 'right', fontWeight: 'bolder', color: 'green' }}
              onClick={(event) => {
                event.stopPropagation();
                this.setState({ showUpdateBox: false });
                this.findWidth();
              }}
            >
              X
            </div>
            <span>Hey! 3D Rover got an upgrade! 👀</span>
            <br />
            <span>
              Click <strong>here</strong> to check our our new .GLTF support!
            </span>
          </div>
        )}
      </div>
    );
  }
}

export default Accelerometer;
