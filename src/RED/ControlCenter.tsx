import React, { Component } from 'react';
import CSS from 'csstype';
import { rovecomm } from '../Core/RoveProtocol/Rovecomm';
import GPS from './components/GPS';
import Log from './components/Log';
import Map from './components/Map';
import Waypoints from './components/Waypoints';
import NewWindowComponent from '../Core/Window';
import RoverOverviewOfNetwork from '../RON/RON';
import RoverAttachmentManager from '../RAM/RAM';
import RoverImageryDisplay from '../RID/RID';
import Power from './components/PMS';
import ControlScheme from '../Core/components/ControlScheme';
import Drive from './components/Drive';
import Gimbal from './components/Gimbal';
// import ThreeDRover from '../Core/components/ThreeDRover';
import CameraSocketManager from './components/CameraSocketManager';
import CameraControls from './components/CameraControls';
// import SignalStack from './components/SignalStack';
import { Client } from 'basic-ftp';

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

interface IProps {}

interface IState {
  storedWaypoints: any;
  currentCoords: { lat: number; lon: number };
  ronOpen: boolean;
  ramOpen: boolean;
  ridOpen: boolean;
  fourthHeight: number;
}

class ControlCenter extends Component<IProps, IState> {
  waypointsInstance: any;

  constructor(props: IProps) {
    super(props);
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      ronOpen: false,
      ramOpen: false,
      ridOpen: false,
      fourthHeight: 1920 / 4 - 10,
    };
    this.updateWaypoints = this.updateWaypoints.bind(this);
    this.updateCoords = this.updateCoords.bind(this);

    window.addEventListener('resize', () => this.setState({ fourthHeight: window.innerHeight / 4 }));
  }

  updateWaypoints(storedWaypoints: any): void {
    this.setState({
      storedWaypoints,
    });
  }

  updateCoords(lat: any, lon: any): void {
    this.setState({
      currentCoords: { lat, lon },
    });
  }

  async exportScreenshots(): Promise<void> {
    for (let i = 0; i < 2; i++) {
      console.log('Connecting to client...');
      const client: Client = new Client();
      client.ftp.verbose = true;
      try {
        await client.access({
          host: `192.168.4.10${i}`,
          user: 'pi',
          password: 'raspberry',
          secure: false,
        });

        console.log(await client.list());

        // Ensure the remote directory exists or create it
        await client.ensureDir('/home/pi/Screenshots');

        // Download files to the local directory
        await client.downloadToDir('./PiScreenshots', '/home/pi/Screenshots');

        console.log('Downloading...');

        // Remove all files from the remote directory
        await client.clearWorkingDir();

        console.log('Remote directory cleared.');
      } catch (err) {
        console.log(err);
      } finally {
        client.close(); // Close the client after the operation
      }
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={row}>
          {
            // onClose will be fired when the new window is closed
            // everything inside NewWindowComponent is considered props.children and will be
            // displayed in a new window
            this.state.ronOpen && (
              <NewWindowComponent onClose={() => this.setState({ ronOpen: false })} name="Rover Overview of Network">
                <RoverOverviewOfNetwork />
              </NewWindowComponent>
            )
          }
          {
            // onClose will be fired when the new window is closed
            // everything inside NewWindowComponent is considered props.children and will be
            // displayed in a new window
            this.state.ramOpen && (
              <NewWindowComponent onClose={() => this.setState({ ramOpen: false })} name="Rover Attachment Manager">
                <RoverAttachmentManager
                  selectedWaypoint={
                    this.waypointsInstance.state.storedWaypoints[this.waypointsInstance.state.selectedWaypoint]
                  }
                />
              </NewWindowComponent>
            )
          }
          {
            // onClose will be fired when the new window is closed
            // everything inside NewWindowComponent is considered props.children and will be
            // displayed in a new window
            this.state.ridOpen && (
              <NewWindowComponent onClose={() => this.setState({ ridOpen: false })} name="Rover Imagery Display">
                <RoverImageryDisplay rowcol="" style={{ width: '100%', height: '100%' }} store={() => {}} />
              </NewWindowComponent>
            )
          }
          <div style={{ ...column, width: '60%' }}>
            <div style={row}>
              <GPS onCoordsChange={this.updateCoords} style={{ marginRight: '5px', width: '100%' }} />
              {/* <ThreeDRover style={{ width: '40%' }} /> */}
            </div>
            <div style={{ ...row, height: '500px' }}>
              <Waypoints
                onWaypointChange={this.updateWaypoints}
                currentCoords={this.state.currentCoords}
                ref={(instance) => {
                  this.waypointsInstance = instance;
                }}
                style={{ flexGrow: 1 }}
              />
            </div>
            <Log />
          </div>
          <div style={{ ...column, width: '40%' }}>
            <Map
              style={{ minHeight: `${this.state.fourthHeight / 1.25}px` }}
              storedWaypoints={this.state.storedWaypoints}
              currentCoords={this.state.currentCoords}
              store={(name: string, coords: any) => this.waypointsInstance.store(name, coords)}
              name="controlCenterMap"
            />
            <CameraSocketManager />
            <CameraControls defaultWidth={640} startSource={0} labelName={'Camera 1'}></CameraControls>
            <CameraControls defaultWidth={640} startSource={1} labelName={'Camera 2'}></CameraControls>
            <button onClick={this.exportScreenshots} style={{ marginTop: '10px' }}>
              Export Screenshots
            </button>
          </div>
        </div>
        <div style={{ ...column, width: '60%' }}>
          <Power />
          <Drive />
          <div style={row}>
            <ControlScheme
              style={{ flexGrow: 1, marginRight: '5px', marginBottom: '5px' }}
              configs={['Drive', 'MainGimbal', 'ControlMultipliers', 'SignalStack']}
            />
            <Gimbal style={{ height: '100%' }} />
          </div>
          <div style={row}>
            <button type="button" onClick={rovecomm.resubscribe} style={{ width: '100px' }}>
              Resubscribe All
            </button>
            <button type="button" onClick={() => this.setState({ ronOpen: true })}>
              Open Rover Overview of Network
            </button>
            <button type="button" onClick={() => this.setState({ ramOpen: true })}>
              Open Rover Attachment Manager
            </button>
            <button type="button" onClick={() => this.setState({ ridOpen: true })}>
              Open Rover Imagery Display
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default ControlCenter;
