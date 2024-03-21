import React, { Component } from 'react';
import CSS from 'csstype';
import path from 'path';
import { rovecomm } from '../RoveProtocol/Rovecomm';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader';
import { BufferGeometry } from 'three';

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

const ROVER_FILE = path.join(__dirname, '../assets/Rover.stl');

interface IProps {
  style?: CSS.Properties;
  zoom?: number;
}

interface IState {
  downVector: number[];
  width: number;
  height: number;
  geometry?: BufferGeometry;
}

class Accelerometer extends Component<IProps, IState> {
  static defaultProps: IProps = {
    zoom: 20,
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      downVector: [0, -1, 0],
      width: 300,
      height: 150,
    };

    rovecomm.on('AccelerometerData', (data: number[]) =>
      this.setState((prevState: IState) => ({
        ...prevState,
        downVector: data,
      }))
    );
  }
  componentDidMount(): void {
    const loader = new STLLoader();
    loader.load(ROVER_FILE, (geo) => {
      geo.center();
      geo.computeVertexNormals();
      this.setState((prevState: IState) => ({
        ...prevState,
        geometry: geo
      }));
    });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>3D Rover</div>
        <div style={{...container, width:this.state.width, height:this.state.height}}>
          <Canvas>
            <directionalLight
              position={[3.3, 1.0, 4.4]}
              intensity={Math.PI}
            />
            <ambientLight intensity={0.5 * Math.PI} />
            <group>
              <mesh geometry={this.state.geometry} rotation={[0, 0, 0]} scale={.1}>
                <meshLambertMaterial color={"#B92C2C"} />
              </mesh>
            </group>
            <OrbitControls/>
          </Canvas>
        </div>
      </div>
    )
  }
}

export default Accelerometer;