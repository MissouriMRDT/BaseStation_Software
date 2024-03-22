import React, { Component } from 'react';
import CSS from 'csstype';
import DroneBattery from './components/drone_battery';
import DroneCamera from './components/drone_camera';
import DroneLocation from './components/drone_location';
// import DroneMap from './components/drone_map';
// import ThreeDdrone from './components/ThreeD_drone';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  flexShrink: 1,
  flexGrow: 1,
  justifyContent: 'space-between',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  flexShrink: 1,
  marginRight: '5px',
};

interface IProps {
  selectedWaypoint: any;
}

interface IState {}

class Drone extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {};
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          {/* <DroneMap style={{ marginRight: '5px', flexWrap: 'wrap' }}/> */}
          {/* <ThreeDdrone/> */}
        </div>
        <div style={row}>
          <DroneLocation/>
          <DroneBattery/>
        </div>
        <div /* style={{ ...row, width: '100%' }} */>
          <DroneCamera/>
        </div>
      </div>
    );
  }
}

export default Drone;
