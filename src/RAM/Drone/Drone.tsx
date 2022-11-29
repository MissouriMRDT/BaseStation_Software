import React, { Component } from 'react';
import CSS from 'csstype';
import DroneControls from './components/drone_controls';
import DroneState from './components/drone_state';
import DroneActivity from './components/drone_activity';

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
      <div style={{ ...column }}>
        <div style={{ ...row, justifyContent: 'space-between', flex: 1 }}>
          <div style={{ ...row, flex: 1 }}>
            <DroneControls
              style={{ marginRight: '5px', flexWrap: 'wrap' }}
              selectedWaypoint={this.props.selectedWaypoint}
            />
            <DroneState />
          </div>
          <div style={row}>
            <DroneActivity />
          </div>
        </div>
      </div>
    );
  }
}

export default Drone;
