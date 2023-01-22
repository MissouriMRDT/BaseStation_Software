import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';
import { button, container } from '../../../Core/components/CssConstants';

const localcontainer: CSS.Properties = {
  display: 'flex',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  justifyContent: 'center',
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
const row: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
};
const localbutton: CSS.Properties = {
  fontFamily: 'arial',
  flexGrow: 1,
  margin: '5px',
  fontSize: '14px',
  lineHeight: '24px',
  borderWidth: '2px',
};

function startAutonomy(): void {
  rovecomm.sendCommand('StartAutonomy', [1]);
}
function stopAutonomy(): void {
  rovecomm.sendCommand('DisableAutonomy', [1]);
}
function clearWaypoints(): void {
  rovecomm.emit('AutonomyActivity', "-------- Clearing Autonomy's waypoints --------");
  rovecomm.sendCommand('ClearWaypoints', [1]);
}

interface IProps {
  style?: CSS.Properties;
  selectedWaypoint: any;
  theme: string;
}
interface IState {}

class Controls extends Component<IProps, IState> {
  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {};

    this.addPositionLeg = this.addPositionLeg.bind(this);
    this.addMarkerLeg = this.addMarkerLeg.bind(this);
    this.addGateLeg = this.addGateLeg.bind(this);
  }

  addPositionLeg(): void {
    rovecomm.emit(
      'AutonomyActivity',
      `Sending Position (
       Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
       Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('AddPositionLeg', [
      this.props.selectedWaypoint.latitude,
      this.props.selectedWaypoint.longitude,
    ]);
  }

  addMarkerLeg(): void {
    rovecomm.emit(
      'AutonomyActivity',
      `Sending Marker (Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
       Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('AddMarkerLeg', [this.props.selectedWaypoint.latitude, this.props.selectedWaypoint.longitude]);
  }

  addGateLeg(): void {
    rovecomm.emit(
      'AutonomyActivity',
      `Sending Gate (Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
       Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('AddGateLeg', [this.props.selectedWaypoint.latitude, this.props.selectedWaypoint.longitude]);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Controls</div>
        <div style={{ ...container(this.props.theme), ...localcontainer }}>
          <div style={row}>
            <button type="button" onClick={startAutonomy} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Start Autonomy</h1>
            </button>
            <button type="button" onClick={stopAutonomy} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Stop Autonomy</h1>
            </button>
            <button type="button" onClick={this.addPositionLeg} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Add Position Leg</h1>
            </button>
            <button type="button" onClick={this.addMarkerLeg} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Add Marker Leg</h1>
            </button>
            <button type="button" onClick={this.addGateLeg} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Add Gate Leg</h1>
            </button>
            <button type="button" onClick={clearWaypoints} style={{ ...button(this.props.theme), ...localbutton }}>
              <h1 style={{ ...button(this.props.theme), ...localbutton }}>Clear Waypoints</h1>
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default Controls;
