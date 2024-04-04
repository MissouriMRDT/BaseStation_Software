import React, { Component, Suspense } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../RoveProtocol/Rovecomm';
import { ArrowHelperProps, Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';
import { BufferGeometry, Euler, Group, Vector3 } from 'three';
import { RoverScene } from './RoverScene';

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
  //height: '20vw',
  aspectRatio: '16 / 9',
  position: 'relative',
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

const MODELS_PATH = path.join(__dirname, '../assets/models');

const DANGER_ANGLE = (50 * Math.PI) / 180;
const negativeY = new Vector3(0, -1, 0);
const positiveY = new Vector3(0, 1, 0);

interface IProps {
  style?: CSS.Properties;
}

interface IState {
  downVector: Vector3;
  displayPitch: number;
  displayRoll: number;
  color: '#B92C2C' | '#363636';
  file: string;
  geometry?: BufferGeometry;
  showManualControls: boolean;
}

class Accelerometer extends Component<IProps, IState> {
  static defaultProps: IProps = {
    style: {},
    // width: 300,
    // height: 150
  };

  private arrowRef: React.RefObject<ArrowHelperProps>;

  private roverRef: React.RefObject<Group>;

  private showUpdate = false;

  constructor(props: IProps) {
    super(props);
    this.state = {
      downVector: new Vector3(0, -1, 0),
      displayPitch: 0,
      displayRoll: 0,
      color: '#363636',
      file: path.join(MODELS_PATH, 'rover.stl'),
      showManualControls: false,
    };

    rovecomm.on('AccelerometerData', (data: number[]) => {
      this.setState({ downVector: new Vector3(...data) });
      this.calcRotation();
    });

    this.arrowRef = React.createRef();
    this.roverRef = React.createRef();
    setTimeout(() => {
      this.showUpdate = true;
      this.forceUpdate();
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
  }

  componentDidUpdate(_prevProps: Readonly<IProps>, prevState: Readonly<IState>): void {
    if (prevState.file !== this.state.file) {
      this.loadGeometry();
    }
  }

  componentWillUnmount(): void {
    this.state.geometry?.dispose();
  }

  // We must rotate the rover such that downVector would point to <0, -1, 0> if it underwent the same rotation.
  // We find the axis to rotate it around by crossing downVector with <0, -1, 0>,
  // We use a quaternion to represent the axis and angle, then convert it to euler angles.
  calcRotation(): void {
    const normalizedDown = this.state.downVector.clone().normalize();
    const axisAround = normalizedDown.clone().cross(negativeY).normalize();
    const angleAround = normalizedDown.angleTo(negativeY);
    let roll = Math.round((Math.asin(normalizedDown.x) / Math.PI) * 180);
    let pitch = Math.round((Math.asin(normalizedDown.z) / Math.PI) * 180);
    if (normalizedDown.y > 0) {
      roll = 180 - roll;
      pitch = 180 - pitch;
    }
    this.setState({
      displayPitch: pitch,
      displayRoll: roll,
      color: angleAround > DANGER_ANGLE ? '#B92C2C' : '#363636',
    });
    this.arrowRef.current?.setDirection?.(normalizedDown);
    if (normalizedDown.equals(positiveY)) this.roverRef.current?.setRotationFromEuler(new Euler(0, 0, Math.PI));
    else this.roverRef.current?.setRotationFromAxisAngle(axisAround, angleAround);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Rover</div>
        <div style={{ ...container }}>
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
              <group>
                <mesh position={[0, 3, 0]}>
                  <boxGeometry />
                  <meshBasicMaterial color={'black'} />
                </mesh>
                <arrowHelper
                  args={[this.state.downVector, new Vector3(0, 3, 0), 2, 'green', 0.3, 0.3]}
                  ref={this.arrowRef}
                />
              </group>
              <group ref={this.roverRef}>
                {this.state.file.endsWith('.glb') ? (
                  <RoverScene position-y={-2} scale={2.5} />
                ) : (
                  <mesh geometry={this.state.geometry} scale={0.08} castShadow>
                    <meshLambertMaterial color={this.state.color} />
                  </mesh>
                )}
              </group>
              <mesh position-y={-3} rotation-x={-Math.PI * 0.5} scale={50} receiveShadow>
                <planeGeometry />
                <meshStandardMaterial color={'#c1c1c1'} />
              </mesh>
              <OrbitControls enablePan={false} minDistance={3} maxDistance={8} />
            </Suspense>
          </Canvas>
          <span
            style={{
              backgroundColor: '#f1f1f1',
              opacity: '50%',
              padding: '0.1em',
              color: '#c1c1c1',
              cursor: 'pointer',
              position: 'absolute',
              bottom: 0,
              right: 0,
            }}
            onClick={() => this.setState({ showManualControls: !this.state.showManualControls })}
          >
            <small>{this.state.showManualControls ? 'Hide' : 'Show'} Debug Controls</small>
          </span>
        </div>
        {this.state.showManualControls && (
          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <label>set manually</label>
            <input
              type="text"
              onChange={(event) => {
                this.setState({
                  downVector: new Vector3(
                    ...event.target.value
                      .split(',')
                      .map((tok) => tok.trim())
                      .map((el) => parseFloat(el))
                      .slice(0, 3)
                  ),
                });
                this.calcRotation();
              }}
            />
            <label>set x</label>
            <input
              type="range"
              min="-1"
              max="1"
              step="0.05"
              value={this.state.downVector.x}
              onChange={(event) => {
                this.setState((prevState) => ({
                  downVector: prevState.downVector.setX(parseFloat(event.target.value)),
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
              value={this.state.downVector.y}
              onChange={(event) => {
                this.setState((prevState) => ({
                  downVector: prevState.downVector.setY(parseFloat(event.target.value)),
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
              value={this.state.downVector.z}
              onChange={(event) => {
                this.setState((prevState) => ({
                  downVector: prevState.downVector.setZ(parseFloat(event.target.value)),
                }));
                this.calcRotation();
              }}
            />
            <div>{`down: <${String(Object.values(this.state.downVector))}>`}</div>
            <div>{`pitch: ${this.state.displayPitch}°, roll: ${this.state.displayRoll}°`}</div>
          </div>
        )}
        {this.state.file.endsWith('.stl') && this.showUpdate && (
          <div
            style={{
              backgroundColor: 'lightgreen',
              padding: '10px',
              marginTop: 10,
              border: '2px solid green',
              borderRadius: '5px',
              cursor: 'pointer',
              fontFamily: 'Comic Sans MS',
              fontSize: '.8em',
            }}
            onClick={() => this.setState({ file: path.join(MODELS_PATH, 'rover_preview.glb') })}
          >
            <div
              style={{ cursor: 'pointer', float: 'right', fontWeight: 'bolder', color: 'green' }}
              onClick={(event) => {
                event.stopPropagation();
                this.showUpdate = false;
                this.forceUpdate();
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
