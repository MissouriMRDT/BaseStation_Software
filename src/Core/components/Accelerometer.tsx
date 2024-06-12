import React, { Component, Suspense } from 'react';
import CSS from 'csstype';
import path from 'path';
import fs from 'fs';
import { rovecomm } from '../RoveProtocol/Rovecomm';
import { ArrowHelperProps, Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import * as THREE from 'three';
import RoverScene from './RoverScene';
import { windows } from '../Window';
import { EventEmitter } from 'events';

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

const TIPOVER_PATH = path.join(__dirname, '../assets/TipoverVectors.json');
const negativeY = new THREE.Vector3(0, -1, 0);
const positiveY = new THREE.Vector3(0, 1, 0);

interface TipoverEntry {
  id: number;
  upVector: THREE.Vector3;
}

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
  dangerLevel: 'green' | 'yellow' | 'red';
  showDebug: boolean;
  showUpdateBox: boolean;
  showGltf: boolean;
  storedTipoverVectors: TipoverEntry[];
  selectedTipoverVector: TipoverEntry | null;
}

const globalTipoverVectors: TipoverEntry[] = [];
const synchronizer = new EventEmitter();
synchronizer.addListener('update', () =>
  fs.writeFile(
    TIPOVER_PATH,
    JSON.stringify(globalTipoverVectors, null, 2),
    (err) => err && console.error('Failed to save tipover vectors to file!', err)
  )
);
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
      dangerLevel: 'green',
      showDebug: false,
      showUpdateBox: false,
      showGltf: false,
      storedTipoverVectors: globalTipoverVectors,
      selectedTipoverVector: null,
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

    this.tipoverCallback.bind(this);
    synchronizer.addListener('update', this.tipoverCallback);

    setTimeout(() => {
      this.setState({ showUpdateBox: true });
    }, 10000);
  }

  componentDidMount(): void {
    // try to read tipover vectors from file
    if (globalTipoverVectors.length === 0) {
      if (fs.existsSync(TIPOVER_PATH)) {
        const tipoverList = JSON.parse(fs.readFileSync(TIPOVER_PATH).toString()) as TipoverEntry[];
        globalTipoverVectors.push(
          ...tipoverList.map((entry) => ({
            id: entry.id,
            upVector: new THREE.Vector3().copy(entry.upVector), // the json doesn't parse right unless you make a new object
          }))
        );
        console.log(globalTipoverVectors);
        synchronizer.emit('update');
      }
    }
    this.findWidth();
  }

  componentDidUpdate(_prevProps: Readonly<IProps>, prevState: Readonly<IState>): void {
    if (prevState.showDebug !== this.state.showDebug || prevState.showUpdateBox !== this.state.showUpdateBox) {
      this.findWidth();
    }
  }

  componentWillUnmount(): void {
    this.removeResizeCallbacks();
    synchronizer.removeListener('update', this.tipoverCallback);
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
    // this still braks for y < 0, but if the rover gets itself into that orientation, we have bigger problems
    if (normalizedUp.y < 0) {
      roll = 180 - roll;
      pitch = 180 - pitch;
    }
    // set angles
    this.arrowRef.current?.setDirection?.(normalizedUp);
    if (normalizedUp.equals(negativeY)) this.roverRef.current?.setRotationFromEuler(new THREE.Euler(0, 0, Math.PI));
    else this.roverRef.current?.setRotationFromAxisAngle(axisAround, angleAround);
    // set colors
    let danger: IState['dangerLevel'] = 'green';
    if (this.state.storedTipoverVectors.length > 0) {
      const closestTipoverVector = this.state.storedTipoverVectors.reduce((prev, curr) =>
        normalizedUp.dot(curr.upVector) > normalizedUp.dot(prev.upVector) ? curr : prev
      ).upVector;
      const tipoverAngle = positiveY.angleTo(closestTipoverVector);
      const upAngle = positiveY.angleTo(normalizedUp);
      if (upAngle - tipoverAngle < -Math.PI / 12) danger = 'green';
      else if (upAngle - tipoverAngle >= 0) danger = 'red';
      else danger = 'yellow';
    }
    this.arrowRef.current?.setColor?.(danger);
    // set debug info
    this.setState({
      displayPitch: -pitch,
      displayRoll: -roll,
      dangerLevel: danger,
    });
  }

  resizeCallback = () => this.findWidth();

  tipoverCallback = () => {
    if (this.state.selectedTipoverVector)
      if (!globalTipoverVectors.find((entry) => entry.id === this.state.selectedTipoverVector?.id))
        this.setState({ selectedTipoverVector: null });
    this.setState({ storedTipoverVectors: globalTipoverVectors });
    this.calcRotation(); // update colors
  };

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
                  {this.state.showDebug && (
                    <group>
                      <mesh position={[0, 3, 0]}>
                        <boxGeometry />
                        <meshBasicMaterial color={'black'} />
                      </mesh>
                      <arrowHelper
                        args={[this.state.upVector, new THREE.Vector3(0, 3, 0), 2, 'green', 0.3, 0.3]}
                        ref={this.arrowRef}
                      />
                      {this.state.storedTipoverVectors.map((vector) => {
                        return (
                          <arrowHelper
                            key={vector.id}
                            args={[
                              vector.upVector,
                              new THREE.Vector3(0, 3, 0),
                              2,
                              vector === this.state.selectedTipoverVector ? 'pink' : 'red',
                              0.3,
                              0.3,
                            ]}
                            onClick={(event) => {
                              event.stopPropagation();
                              this.setState({ selectedTipoverVector: vector });
                            }}
                          />
                        );
                      })}
                    </group>
                  )}
                  <group ref={this.roverRef}>
                    <RoverScene
                      color={
                        this.state.dangerLevel === 'red'
                          ? '#B92C2C'
                          : this.state.dangerLevel === 'yellow'
                          ? '#FFF200'
                          : '#363636'
                      }
                      showGltf={this.state.showGltf}
                    />
                  </group>
                  <mesh
                    position-y={-3}
                    rotation-x={-Math.PI * 0.5}
                    scale={50}
                    onClick={() => this.setState({ selectedTipoverVector: null })}
                    receiveShadow
                  >
                    <planeGeometry />
                    <meshLambertMaterial color={'#c1c1c1'} />
                  </mesh>
                  <OrbitControls enablePan={false} minDistance={4} maxDistance={8} />
                </Suspense>
              </Canvas>
              <span style={pitchYawOverlay}>
                {`pitch: ${this.state.displayPitch}°`}
                <br />
                {`roll: ${this.state.displayRoll}°`}
              </span>
              <span style={debugDialogOverlay} onClick={() => this.setState({ showDebug: !this.state.showDebug })}>
                <small>{this.state.showDebug ? 'Hide' : 'Show'} Debug Controls</small>
              </span>
            </div>
          </div>
          {this.state.showDebug && (
            <div style={debugContainer}>
              <div style={{ display: 'flex', flexDirection: 'column' }}>
                <div style={{ padding: '5px', display: 'inline-flex', gap: '5px' }}>
                  <button
                    onClick={() => {
                      globalTipoverVectors.push({
                        id:
                          globalTipoverVectors.length > 0
                            ? Math.max(...globalTipoverVectors.map((vector) => vector.id)) + 1
                            : 0,
                        upVector: this.state.upVector.clone().normalize(),
                      });
                      synchronizer.emit('update');
                    }}
                  >
                    Add Tipover Vector
                  </button>
                  {this.state.selectedTipoverVector && (
                    <button
                      onClick={() => {
                        if (this.state.selectedTipoverVector) {
                          const removeIndex = globalTipoverVectors.findIndex(
                            (entry) => entry.id === this.state.selectedTipoverVector?.id
                          );
                          globalTipoverVectors.splice(removeIndex, 1);
                          synchronizer.emit('update');
                        }
                      }}
                    >
                      Remove Selected Vector
                    </button>
                  )}
                </div>
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
                <button onClick={() => this.setState({ showGltf: !this.state.showGltf })}>
                  Mode: {this.state.showGltf ? 'Will' : 'Rover'}
                </button>
              </div>
            </div>
          )}
        </div>
        {this.state.showUpdateBox && (
          <div
            style={updateDialogContainer}
            onClick={() => {
              this.setState({ showUpdateBox: false, showGltf: true });
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
