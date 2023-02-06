import React, { Component } from 'react';
import CSS from 'csstype';
import DroneControls from './components/drone_controls';
import DroneState from './components/drone_state';
import DroneActivity from './components/drone_activity';
import DroneTeleop from './components/drone_teleop';

const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
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
      <div style={row}>
        <div style={{ ...column, flex: 1 }}>
          <DroneControls
            style={{ marginRight: '5px', flexWrap: 'wrap' }}
            selectedWaypoint={this.props.selectedWaypoint}
          />
          <DroneActivity style={{ marginRight: '5px' }} />
          <DroneTeleop style={{ marginRight: '5px' }} />
        </div>
        <div style={{ ...column, width: '20%' }}>
          <DroneState />
        </div>
      </div>
    );
  }
}

export default Drone;
