import React, { Component, Suspense } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../RoveProtocol/Rovecomm';
import { ArrowHelperProps, Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';
import { BufferGeometry, Euler, Group, Object3DEventMap, Vector3 } from 'three';
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
  height: 'calc(100% - 47px)',
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

const ROVER_FILE = path.join(__dirname, '../assets/models/Rover.stl');

interface IProps {
  style?: CSS.Properties;
  zoom?: number;
}

interface IState {
  downVector: Vector3;
  rotation: Euler;
  width: number;
  height: number;
  geometry?: BufferGeometry;
  scene?: Group<Object3DEventMap>;
}

class Accelerometer extends Component<IProps, IState> {
  static defaultProps: IProps = {
    zoom: 20,
    style: {},
  };

  private arrowRef: React.RefObject<ArrowHelperProps>;

  constructor(props: IProps) {
    super(props);
    this.state = {
      downVector: new Vector3(0, -1, 0),
      rotation: new Euler(),
      width: 300,
      height: 150,
    };

    rovecomm.on('AccelerometerData', (data: number[]) => {
      this.setState({ downVector: new Vector3(...data) });
      this.calcRotation();
    });

    this.arrowRef = React.createRef();
  }

  componentDidMount(): void {
    if (ROVER_FILE.endsWith('.stl')) {
      const loader = new STLLoader();
      loader.load(ROVER_FILE, (geo) => {
        geo.center();
        geo.computeVertexNormals();
        this.setState({ geometry: geo });
      });
    }
  }

  componentWillUnmount(): void {
    //this.state.scene.dispose();
    //this.state.geometry?.dispose();
    this.setState({ scene: undefined, geometry: undefined });
  }

  calcRotation(): void {
    const v = this.state.downVector.clone();
    v.normalize();
    v.negate();
    const pitch = Math.atan2(v.y, v.z) - Math.PI * 0.5;
    const yaw = 0;
    const roll = Math.atan2(v.y, v.x) - Math.PI * 0.5;
    const e = new Euler(pitch, yaw, roll, 'XZY');
    //console.log(e, v);
    this.setState({ rotation: e });
    this.arrowRef.current?.setDirection(this.state.downVector.clone().normalize());
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Rover</div>
        <div style={{ ...container, width: this.state.width, height: this.state.height }}>
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
              {ROVER_FILE.endsWith('.glb') ? (
                <RoverScene rotation={this.state.rotation} position-y={-1} scale={2} />
              ) : (
                <group>
                  <mesh geometry={this.state.geometry} rotation={this.state.rotation} scale={0.1} castShadow>
                    <meshLambertMaterial color={'#B92C2C'} />
                  </mesh>
                  <arrowHelper
                    args={[this.state.downVector, new Vector3(), 2, 'green', 0.3, 0.3]}
                    ref={this.arrowRef}
                  />
                </group>
              )}
              <mesh position-y={-3} rotation-x={-Math.PI * 0.5} scale={50} receiveShadow>
                <planeGeometry />
                <meshStandardMaterial color={'#c1c1c1'} />
              </mesh>
              <OrbitControls panSpeed={0} />
            </Suspense>
          </Canvas>
        </div>
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
              this.setState((prevState) => ({ downVector: prevState.downVector.setX(parseFloat(event.target.value)) }));
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
              this.setState((prevState) => ({ downVector: prevState.downVector.setY(parseFloat(event.target.value)) }));
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
              this.setState((prevState) => ({ downVector: prevState.downVector.setZ(parseFloat(event.target.value)) }));
              this.calcRotation();
            }}
          />
          <div>{`downVec: <${String(Object.values(this.state.downVector))}>`}</div>
        </div>
      </div>
    );
  }
}

export default Accelerometer;
