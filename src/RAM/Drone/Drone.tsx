import React, { Component } from 'react';
import CSS from 'csstype';
import DroneBattery from './components/drone_battery';
import DroneData from './components/drone_data';
// import DroneCamera from './components/drone_camera';
// import DroneLocation from './components/drone_location';
// import DroneMap from './components/drone_map';
import ThreeDdrone from './components/ThreeD_drone';
import Cameras from '../../Core/components/Cameras';
// import Map from '../../RED/components/Map';

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

interface IState {
  // fourthHeight: number;
  storedWaypoints: any;
  currentCoords: { lat: number; lon: number };
  ronOpen: boolean;
  ramOpen: boolean;
  ridOpen: boolean;
  fourthHeight: number;
}

class Drone extends Component<IProps, IState> {
  waypointsInstance: any;

  constructor(props: IProps) {
    super(props);
    this.state = {
      // fourthHeight: 640 / 4 - 10,
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      ronOpen: false,
      ramOpen: false,
      ridOpen: false,
      fourthHeight: 1920 / 4 - 10,
    };
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <div style={{ ...column, width: '50%' }}>
            <ThreeDdrone />
          </div>
          <div style={{ ...column, width: '50%' }}>
            {/* <Map
              style={{ minHeight: `${this.state.fourthHeight / 1.25}px` }}
              storedWaypoints={this.state.storedWaypoints}
              currentCoords={this.state.currentCoords}
              store={(name: string, coords: any) => this.waypointsInstance.store(name, coords)}
              name="controlCenterMap"
            /> */}
          </div>
        </div>
        <div style={row}>
          <Cameras style={{ width: '100%' }} defaultCamera={1} />
        </div>
        <div style={row}>
          <div style={{ ...column }}>
            <DroneData />
          </div>
          <div style={{ ...column }}>
            <DroneBattery />
          </div>
        </div>
      </div>
    );
  }
}

export default Drone;
