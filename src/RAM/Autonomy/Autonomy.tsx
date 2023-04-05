import React, { Component } from 'react';
import CSS from 'csstype';
import Controls from './components/Controls';
import StateDiagram from './components/StateDiagram';
import Activity from './components/Activity';
import Lighting from './components/Lighting';

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
      <div style={{ ...column }}>
        <div style={{ ...row, justifyContent: 'space-between', flex: 1 }}>
          <div style={{ ...column, flex: 1 }}>
            <Controls style={{ marginRight: '5px', flexWrap: 'wrap' }} selectedWaypoint={this.props.selectedWaypoint} />
            <div style={{ ...row, marginRight: '5px', flexGrow: 1 }}>
              <Activity style={{ flex: 1, marginRight: '5px' }} />
              <Lighting />
            </div>
          </div>
          <StateDiagram />
        </div>
      </div>
    );
  }
}
export default Autonomy;
