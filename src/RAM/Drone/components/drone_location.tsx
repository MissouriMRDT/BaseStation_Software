import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'grid',
  fontFamily: 'arial',
  borderColor: '#990000',
  borderStyle: 'solid',
  borderTopWidth: '30px',
  borderBottomWidth: '3px',
  whiteSpace: 'pre-wrap',
  padding: '5px',
};

const label: CSS.Properties = {
  marginTop: '-10px',
  position: 'relative',
  fontFamily: 'arial',
  fontSize: '16px',
  color: '#ffffff',
  top: '24px',
  left: '3px',
};

interface IProps {}

interface IState {
  alt: number;
  roll: number;
  pitch: number;
  yaw: number;
  long: number;
  lat: number;
}

class DroneLocation extends Component<IProps, IState> {
  constructor(props: any) {
    super(props);
    this.state = {
      alt: 0,
      roll: 0,
      pitch: 0,
      yaw: 0,
      long: 0,
      lat: 0,
    };
    // Not Implemented
    rovecomm.on('droneLocation', (data: number[]) => {
      this.updateLocation(data);
    });
    rovecomm.on('droneOrientation', (data: number[]) => {
      this.updateOrientation(data);
    });
  }

  updateLocation(data: number[]) {
    this.setState({
      alt: data[0],
      long: data[1],
      lat: data[2],
    });
  }

  updateOrientation(data: number[]) {
    this.setState({
      pitch: data[0],
      yaw: data[1],
      roll: data[2],
    });
  }

  render(): JSX.Element {
    return (
      <div style={{ width: '60%' }}>
        <div style={label}>Drone Location</div>
        <div style={container}>
          <div>Roll: {this.state.roll}</div>
          <div>Pitch: {this.state.pitch}</div>
          <div>Yaw: {this.state.yaw}</div>
          <div>Altitude: {this.state.alt}</div>
          <div>Longitude: {this.state.long}</div>
          <div>Latitude: {this.state.lat}</div>
        </div>
      </div>
    );
  }
}

export default DroneLocation;
