import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../../../Core/RoveProtocol/Rovecomm';

const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  fontFamily: 'arial',
  borderTopWidth: '30px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  flexWrap: 'wrap',
  overflow: 'scroll',
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
const button: CSS.Properties = {
  fontFamily: 'arial',
  flexGrow: 1,
  margin: '5px',
  fontSize: '14px',
  lineHeight: '24px',
  borderWidth: '2px',
  height: '40px',
};

function startAutonomy(): void {
  rovecomm.sendCommand('droneStartAutonomy', [1]);
}
function stopAutonomy(): void {
  rovecomm.sendCommand('droneDisableAutonomy', [1]);
}
function clearWaypoints(): void {
  rovecomm.emit('droneAutonomyActivity', "-------- Clearing Autonomy's waypoints --------");
  rovecomm.sendCommand('droneClearWaypoints', [1]);
}

interface IProps {
  style?: CSS.Properties;
  selectedWaypoint: any;
}
interface IState {}

class DroneControls extends Component<IProps, IState> {
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
      'droneAutonomyActivity',
      `Sending Position (
         Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
         Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('droneAddPositionLeg', [
      this.props.selectedWaypoint.latitude,
      this.props.selectedWaypoint.longitude,
    ]);
  }

  addMarkerLeg(): void {
    rovecomm.emit(
      'droneAutonomyActivity',
      `Sending Marker (Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
         Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('droneAddMarkerLeg', [
      this.props.selectedWaypoint.latitude,
      this.props.selectedWaypoint.longitude,
    ]);
  }

  addGateLeg(): void {
    rovecomm.emit(
      'AutonomyActivity',
      `Sending Gate (Lat: ${this.props.selectedWaypoint.latitude.toFixed(7)}
         Lon: ${this.props.selectedWaypoint.longitude.toFixed(7)})`
    );
    rovecomm.sendCommand('droneAddGateLeg', [
      this.props.selectedWaypoint.latitude,
      this.props.selectedWaypoint.longitude,
    ]);
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drone Controls</div>
        <div style={container}>
          <div style={row}>
            <button type="button" onClick={startAutonomy} style={button}>
              <h1 style={button}>Start Autonomy</h1>
            </button>
            <button type="button" onClick={stopAutonomy} style={button}>
              <h1 style={button}>Stop Autonomy</h1>
            </button>
            <button type="button" onClick={this.addPositionLeg} style={button}>
              <h1 style={button}>Add Position Leg</h1>
            </button>
            <button type="button" onClick={this.addMarkerLeg} style={button}>
              <h1 style={button}>Add Marker Leg</h1>
            </button>
            <button type="button" onClick={this.addGateLeg} style={button}>
              <h1 style={button}>Add Gate Leg</h1>
            </button>
            <button type="button" onClick={clearWaypoints} style={button}>
              <h1 style={button}>Clear Waypoints</h1>
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default DroneControls;
