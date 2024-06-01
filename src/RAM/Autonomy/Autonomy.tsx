import React, { Component } from 'react';
import CSS from 'csstype';
import Controls from './components/Controls';
import StateDiagram from './components/StateDiagram';
import Activity from './components/Activity';
import Lighting from './components/Lighting';
import DrivePower from './components/DrivePower';
import AutonomyLog from './components/AutonomyLog';

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

class Autonomy extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.state = {};
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={{ ...row, justifyContent: 'start' }}>
          <div style={{ ...column }}>
            <Controls style={{ marginRight: '5px', flexWrap: 'wrap' }} selectedWaypoint={this.props.selectedWaypoint} />
            <div style={row}>
              <DrivePower style={{ flex: 1, marginRight: '5px' }} />
            </div>
            <div style={row}>
              <AutonomyLog style={{ flex: 1, marginRight: '5px' }} />
            </div>
            <div style={row}>
              <Activity style={{ flex: 1, marginRight: '5px' }} />
            </div>
            {/* <div style={{ ...row, marginRight: '5px', flexGrow: 1 }}>
            </div> */}
          </div>
          <div style={{ ...column }}>
            <Lighting />
          </div>
        </div>
        <div style={row}>
          <StateDiagram />
        </div>
      </div>
    );
  }
}
export default Autonomy;
