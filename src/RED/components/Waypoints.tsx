import React, { Component } from 'react';
import CSS from 'csstype';
import fs from 'fs';
import { TwitterPicker } from 'react-color';
import path from 'path';
import ThreeDRover from '../../Core/components/ThreeDRover';

const title: CSS.Properties = {
  fontFamily: 'arial',
  width: '25%',
  textAlign: 'center',
};
const container: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  flexGrow: 1,
  fontFamily: 'arial',
  borderTopWidth: '28px',
  borderColor: '#990000',
  borderBottomWidth: '2px',
  borderStyle: 'solid',
  marginBottom: '5px',
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
  justifyContent: 'space-around',
  margin: '10px',
  fontSize: '16px',
  lineHeight: '20px',
};
const column: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
};
const input: CSS.Properties = {
  width: '30%',
};
const singleSelect: CSS.Properties = {
  width: '50%',
  lineHeight: '20px',
  fontSize: '16px',
};
const buttons: CSS.Properties = {
  width: '30%',
  margin: '2.5px',
  fontSize: '14px',
  lineHeight: '24px',
};
const modal: CSS.Properties = {
  position: 'absolute',
  width: '500px',
  margin: '3px 10px',
  zIndex: 1,
  backgroundColor: 'white',
  border: '2px solid #990000',
  textAlign: 'center',
};
const modalButton: CSS.Properties = {
  width: '75px',
  lineHeight: '24px',
  color: 'white',
  fontWeight: 'bold',
  margin: '10px',
};
const scrollviewer: CSS.Properties = {
  display: 'flex',
  flexDirection: 'column',
  overflowY: 'scroll',
  height: '77px',
};
const listEntry: CSS.Properties = {
  appearance: 'none',
  WebkitAppearance: 'none',
  width: '100%',
  display: 'flex',
  flexDirection: 'row',
  justifyContent: 'space-between',
  border: 'none',
  background: 'none',
  lineHeight: '26px',
  fontSize: '16px',
};
const listSubEntry: CSS.Properties = {
  width: '15%',
  textAlign: 'center',
  overflow: 'hidden',
  textOverflow: 'ellipsis',
};
const colorPicker: CSS.Properties = {
  position: 'absolute',
  zIndex: 2,
  marginLeft: '250px',
};
const colorButton: CSS.Properties = {
  width: '15%',
  fontSize: '0px',
  lineHeight: '20px',
  outline: 'none',
};
const slider: CSS.Properties = {
  background: '#990000',
  width: '40%',
  WebkitAppearance: 'none',
  appearance: 'none',
  height: '6px',
  outline: 'none',
  marginLeft: '10px',
  marginRight: '10px',
};
const value: CSS.Properties = {
  fontFamily: 'arial',
  fontSize: '16px',
  lineHeight: '22px',
  width: '10%',
  textAlign: 'center',
};
const radiusRow: CSS.Properties = {
  display: 'flex',
  flexDirection: 'row',
  alignItems: 'center',
  margin: '5px',
};
const filepath = path.join(__dirname, '../assets/Waypoints.json');

export interface Waypoint {
  name: string;
  latitude: number;
  longitude: number;
  color: string;
  colorPicker: boolean;
  onMap: boolean;
  displayRadius: number;
}

interface IProps {
  style?: CSS.Properties;
  onWaypointChange: (storedWaypoints: any) => void;
  currentCoords: { lat: number; lon: number };
}

interface IState {
  storedWaypoints: any;
  selectedWaypoint: string;
  addingWaypoint: boolean;
  coordinateFormat: string;
  newWaypointName: string;
  newWaypointCoords: any;
  displayRadius: number;
}

class Waypoints extends Component<IProps, IState> {
  static id = 0;

  static defaultProps = {
    style: {},
  };

  constructor(props: IProps) {
    super(props);
    this.state = {
      storedWaypoints: {},
      selectedWaypoint: '',
      addingWaypoint: false,
      newWaypointName: '',
      newWaypointCoords: {
        lat: '',
        lon: '',
      },
      coordinateFormat: 'LatLon',
      displayRadius: 0,
    };
    this.store = this.store.bind(this);
    this.remove = this.remove.bind(this);
    this.sliderChange = this.sliderChange.bind(this);
  }

  componentDidMount(): void {
    /** When first built, we want to check the presets file if it exists
     * and import all preset positions. If presets exist, we should also
     * default selected position to the first position. If file does not
     * exist, it isn't created until we attempt to store data
     */
    if (fs.existsSync(filepath)) {
      const storedWaypoints = JSON.parse(fs.readFileSync(filepath).toString());
      const waypointKeys = Object.keys(storedWaypoints);
      const waypointCount = waypointKeys.length;
      if (waypointCount) {
        const selectedWaypoint = waypointKeys[0];
        this.setState({ storedWaypoints, selectedWaypoint });
        this.props.onWaypointChange(storedWaypoints);
      }

      ThreeDRover.id = parseInt(waypointKeys[waypointCount - 1], 10) + 1;
    }
  }

  cascadeWaypoint(): void {
    /** Function to be called on setState callback so that when setState has
     * finished executing we can properly update the json file and trigger the
     * parent function from props. File is created now if it doesn't already exist
     */
    this.props.onWaypointChange(this.state.storedWaypoints);
    fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints, null, 2), (err) => {
      if (err) throw err;
    });
  }

  store(name: string, coords: { lat: number; lon: number }): void {
    // Adds the new waypoint to the select box and updates the json file
    // If selectedWaypoint is still an empty string, this is a good time
    // to update it to a useful starting value
    if (!name) {
      return;
    }
    let newWaypoint: Waypoint;
    const radius: number = this.state.displayRadius;
    if (this.state.coordinateFormat === 'LatLon') {
      newWaypoint = {
        name,
        latitude: Number.isNaN(parseFloat(coords.lat)) ? 0 : parseFloat(coords.lat),
        longitude: Number.isNaN(parseFloat(coords.lon)) ? 0 : parseFloat(coords.lon),
        color: 'black',
        colorPicker: false,
        onMap: true,
        displayRadius: radius,
      };
    } else {
      newWaypoint = {
        name,
        latitude:
          parseFloat(coords.latD) +
          Math.sign(parseFloat(coords.latD)) * (parseFloat(coords.latM) / 60 + parseFloat(coords.latS) / 60 / 60),
        longitude:
          parseFloat(coords.lonD) +
          Math.sign(parseFloat(coords.lonD)) * (parseFloat(coords.lonM) / 60 + parseFloat(coords.lonS) / 60 / 60),
        color: 'black',
        colorPicker: false,
        onMap: true,
        displayRadius: radius,
      };
    }

    this.setState(
      (prevState) => ({
        addingWaypoint: false,
        newWaypointName: '',
        newWaypointCoords: {
          lat: '',
          lon: '',
        },
        coordinateFormat: 'LatLon',
        storedWaypoints: {
          // Spread to ensure all currently stored waypoint are kept
          // but the newest waypoint is added
          ...prevState.storedWaypoints,
          [ThreeDRover.id]: newWaypoint,
        },
      }),
      this.cascadeWaypoint
    );
    ThreeDRover.id += 1;
  }

  /** Deletes a selected stored position (if a position has been selected)
   * and properly updates the json file (see store() for more detailed comments)
   */
  remove(): void {
    const { storedWaypoints } = this.state;
    // Since the selectedPosition is to be deleted, we want to grab a new
    // value. We grab the next key if one exists, or if not the previous key,
    // or if not default to ""
    const waypoints = Object.keys(storedWaypoints);
    const index = waypoints.indexOf(this.state.selectedWaypoint);
    if (waypoints.length <= 1) {
      console.log('you cant lol');
      return;
    }
    let newSelectedWaypoint: string;
    if (waypoints.length > index + 1) {
      newSelectedWaypoint = waypoints[index + 1];
    } else if (waypoints.length > index) {
      newSelectedWaypoint = waypoints[index - 1];
    } else {
      newSelectedWaypoint = '';
    }

    delete storedWaypoints[this.state.selectedWaypoint];

    this.setState(
      {
        storedWaypoints,
        selectedWaypoint: newSelectedWaypoint,
      },
      this.cascadeWaypoint
    );
  }

  updateCoords(event: { target: { value: string } }, coord: string): void {
    this.setState((prevState) => ({
      newWaypointCoords: {
        ...prevState.newWaypointCoords,
        [coord]: event.target.value,
      },
    }));
  }

  changeFormat(event: { target: { value: string } }): void {
    let newWaypointCoords;
    const coordinateFormat = event.target.value;
    if (coordinateFormat === 'LatLon') {
      newWaypointCoords = { lat: '', lon: '' };
    } else if (coordinateFormat === 'DMS') {
      newWaypointCoords = {
        latD: '',
        latM: '',
        latS: '',
        lonD: '',
        lonM: '',
        lonS: '',
      };
    }
    this.setState({
      newWaypointCoords,
      coordinateFormat,
    });
  }

  checkBox(waypoint: string): void {
    this.setState(
      (prevState) => ({
        storedWaypoints: {
          ...prevState.storedWaypoints,
          [waypoint]: {
            ...prevState.storedWaypoints[waypoint],
            onMap: !prevState.storedWaypoints[waypoint].onMap,
          },
        },
      }),
      this.cascadeWaypoint
    );
  }

  colorChanged(color: any, waypoint: string): void {
    this.setState(
      (prevState) => ({
        storedWaypoints: {
          ...prevState.storedWaypoints,
          [waypoint]: {
            ...prevState.storedWaypoints[waypoint],
            color: color.hex,
            colorPicker: false,
          },
        },
      }),
      this.cascadeWaypoint
    );
  }

  openPicker(waypoint: string): void {
    this.setState(
      (prevState) => ({
        storedWaypoints: {
          ...prevState.storedWaypoints,
          [waypoint]: {
            ...prevState.storedWaypoints[waypoint],
            colorPicker: true,
          },
        },
      }),
      this.cascadeWaypoint
    );
  }

  sliderChange(event: { target: { value: string } }): void {
    this.setState({ displayRadius: parseInt(event.target.value, 10) });
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Waypoints</div>
        <div style={container}>
          <div style={column}>
            <div style={row}>
              <button type="button" style={buttons} onClick={() => this.setState({ addingWaypoint: true })}>
                Add Waypoint
              </button>
              <button
                type="button"
                style={buttons}
                onClick={() => this.store(new Date().toLocaleTimeString(), this.props.currentCoords)}
              >
                Save Current
              </button>
              <button type="button" style={buttons} onClick={this.remove}>
                Remove Selected
              </button>
            </div>
            <div style={radiusRow}>
              <div style={{ ...value, width: '20%' }}>Point Radius:</div>
              <input
                type="text"
                value={this.state.displayRadius || '0'}
                onChange={(e) =>
                  this.setState({
                    displayRadius: Number.isNaN(parseInt(e.target.value, 10)) ? 0 : parseInt(e.target.value, 10),
                  })
                }
                style={value}
              />
              <input
                type="range"
                min="0"
                max="250"
                value={this.state.displayRadius}
                style={slider}
                onChange={(e) => this.sliderChange(e)}
              />
              <button onClick={() => this.setState({ displayRadius: 0 })}>Off</button>
            </div>
          </div>
          <div
            style={{
              ...listEntry,
              width: '94.2%',
              marginRight: '5.6%',
              marginLeft: '1.2%',
            }}
          >
            <div style={listSubEntry}>Name</div>
            <div style={listSubEntry}>Latitude</div>
            <div style={listSubEntry}>Longitude</div>
            <div style={listSubEntry}>Color</div>
            <div style={listSubEntry}>On Map</div>
          </div>
          <div style={scrollviewer}>
            {Object.keys(this.state.storedWaypoints).map((waypointName: string) => {
              const waypoint = this.state.storedWaypoints[waypointName];
              const backgroundStyle = {
                ...listEntry,
                backgroundColor: waypointName === this.state.selectedWaypoint ? 'lightblue' : 'white',
              };
              return (
                <div
                  key={waypointName}
                  onClick={() => this.setState({ selectedWaypoint: waypointName }, this.cascadeWaypoint)}
                  style={backgroundStyle}
                >
                  <span style={listSubEntry}>{waypoint.name}</span>
                  <span style={listSubEntry}>{waypoint.latitude}</span>
                  <span style={listSubEntry}>{waypoint.longitude}</span>
                  <button
                    type="button"
                    style={{
                      ...colorButton,
                      backgroundColor: waypoint.color,
                    }}
                    onClick={() => this.openPicker(waypointName)}
                  >
                    {waypoint.color}
                  </button>
                  {waypoint.colorPicker ? (
                    <div style={colorPicker}>
                      <TwitterPicker
                        color={waypoint.color}
                        onChangeComplete={(color) => this.colorChanged(color, waypointName)}
                      />
                    </div>
                  ) : null}
                  <div style={listSubEntry}>
                    <input
                      type="checkbox"
                      defaultChecked={waypoint.onMap}
                      onChange={() => this.checkBox(waypointName)}
                    />
                  </div>
                </div>
              );
            })}
          </div>
          {this.state.addingWaypoint ? (
            <div style={modal}>
              <div style={row}>
                <div style={title}>Name:</div>
                <input
                  type="text"
                  value={this.state.newWaypointName}
                  onChange={(e) => this.setState({ newWaypointName: e.target.value })}
                  style={input}
                />
              </div>
              <select style={singleSelect} value={this.state.coordinateFormat} onChange={(e) => this.changeFormat(e)}>
                <option value="LatLon">LatLon</option>
                <option value="DMS">DMS</option>
              </select>
              {this.state.coordinateFormat === 'LatLon' ? (
                [
                  { label: 'Latitude', short: 'lat' },
                  { label: 'Longitude', short: 'lon' },
                ].map((axis: any) => {
                  return (
                    <div style={row} key={axis.label}>
                      <div style={title}>{axis.label}</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords[axis.short]}
                        onChange={(e) => this.updateCoords(e, axis.short)}
                      />
                    </div>
                  );
                })
              ) : (
                <div style={row}>
                  {[
                    { label: 'Latitude', short: 'lat' },
                    { label: 'Longitude', short: 'lon' },
                  ].map((axis: any) => {
                    return (
                      <div key={axis.label}>
                        <div>{axis.label}</div>
                        {['Degrees', 'Minutes', 'Seconds'].map((unit: string) => {
                          return (
                            <div style={row} key={unit}>
                              <div style={title}>{unit}</div>
                              <input
                                type="text"
                                style={input}
                                value={this.state.newWaypointCoords[axis.short + unit[0]]}
                                onChange={(e) => this.updateCoords(e, axis.short + unit[0])}
                              />
                            </div>
                          );
                        })}
                      </div>
                    );
                  })}
                </div>
              )}
              <div style={row}>
                <button
                  type="button"
                  style={{ ...modalButton, backgroundColor: 'red' }}
                  onClick={() =>
                    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
                    // @ts-ignore
                    this.setState((prevState) => ({
                      addingWaypoint: false,
                      newWaypoint: {
                        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
                        // @ts-ignore
                        ...prevState.newWaypoint,
                        name: '',
                      },
                    }))
                  }
                >
                  Cancel
                </button>
                <button
                  type="button"
                  style={{ ...modalButton, backgroundColor: 'green' }}
                  onClick={() => this.store(this.state.newWaypointName, this.state.newWaypointCoords)}
                >
                  Add
                </button>
              </div>
            </div>
          ) : null}
        </div>
      </div>
    );
  }
}

export default Waypoints;
