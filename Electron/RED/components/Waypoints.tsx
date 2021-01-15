import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"
import { TwitterPicker } from "react-color"

const title: CSS.Properties = {
  fontFamily: "arial",
  width: "25%",
  textAlign: "center",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-around",
  margin: "10px",
  fontSize: "16px",
  lineHeight: "20px",
}
const input: CSS.Properties = {
  width: "30%",
}
const singleSelect: CSS.Properties = {
  width: "50%",
  lineHeight: "20px",
  fontSize: "16px",
}
const buttons: CSS.Properties = {
  width: "30%",
  margin: "5px",
  fontSize: "14px",
  lineHeight: "24px",
  borderRadius: "20px",
}
const modal: CSS.Properties = {
  position: "absolute",
  width: "400px",
  margin: "10px 50px",
  zIndex: 1,
  backgroundColor: "rgba(255,255,255,0.9)",
  border: "2px solid #990000",
  textAlign: "center",
  borderRadius: "25px",
}
const modalButton: CSS.Properties = {
  width: "75px",
  lineHeight: "24px",
  color: "white",
  fontWeight: "bold",
  borderRadius: "10px",
  border: "none",
  margin: "10px",
}
const scrollviewer: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  overflowY: "scroll",
  height: "150px",
}
const listEntry: CSS.Properties = {
  appearance: "none",
  WebkitAppearance: "none",
  width: "100%",
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  border: "none",
  background: "none",
  lineHeight: "26px",
  fontSize: "16px",
}
const listSubEntry: CSS.Properties = {
  width: "15%",
  textAlign: "center",
  overflow: "hidden",
  textOverflow: "ellipsis",
}
const colorPicker: CSS.Properties = {
  position: "absolute",
  zIndex: 2,
  marginLeft: "250px",
}
const colorButton: CSS.Properties = {
  width: "15%",
  fontSize: "0px",
  lineHeight: "20px",
  borderRadius: "20px",
}
const filepath = "./Core/Waypoints.json"

interface Waypoint {
  name: string
  latitude: number
  longitude: number
  color: string
  colorPicker: boolean
  onMap: boolean
}

interface IProps {}

interface IState {
  storedWaypoints: any
  selectedWaypoint: string
  addingWaypoint: boolean
  coordinateFormat: string
  newWaypointName: string
  newWaypointCoords: any
}

class Angular extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      storedWaypoints: {},
      selectedWaypoint: "",
      addingWaypoint: false,
      newWaypointName: "",
      newWaypointCoords: {
        lat: "",
        lon: "",
      },
      coordinateFormat: "LatLong",
    }
    this.store = this.store.bind(this)
    this.remove = this.remove.bind(this)
  }

  componentDidMount(): void {
    /* When first built, we want to check the presets file if it exists
     * and import all preset positions. If presets exist, we should also
     * default selected position to the first position. If file does not
     * exist, it isn't created until we attempt to store data
     */
    if (fs.existsSync(filepath)) {
      const storedWaypoints = JSON.parse(fs.readFileSync(filepath).toString())
      if (Object.keys(storedWaypoints).length) {
        const selectedWaypoint = Object.keys(storedWaypoints)[0]
        this.setState({ storedWaypoints, selectedWaypoint })
      }
    }
  }

  store(): void {
    /* Adds the new waypoint to the select box and updates the json file
     */
    // If selectedWaypoint is still an empty string, this is a good time
    // to update it to a useful starting value
    if (!this.state.newWaypointName) {
      return
    }

    const coords = this.state.newWaypointCoords
    let newWaypoint: Waypoint
    if (this.state.coordinateFormat === "LatLong") {
      newWaypoint = {
        name: this.state.newWaypointName,
        latitude: parseFloat(coords.lat),
        longitude: parseFloat(coords.lon),
        color: "black",
        colorPicker: false,
        onMap: true,
      }
    } else {
      newWaypoint = {
        name: this.state.newWaypointName,
        latitude:
          parseFloat(coords.latD) +
          Math.sign(parseFloat(coords.latD)) * (parseFloat(coords.latM) / 60 + parseFloat(coords.latS) / 60 / 60),
        longitude:
          parseFloat(coords.lonD) +
          Math.sign(parseFloat(coords.lonD)) * (parseFloat(coords.lonM) / 60 + parseFloat(coords.lonS) / 60 / 60),
        color: "black",
        colorPicker: false,
        onMap: true,
      }
    }

    this.setState(
      {
        addingWaypoint: false,
        newWaypointName: "",
        newWaypointCoords: {
          lat: "",
          lon: "",
        },
        coordinateFormat: "LatLong",
        storedWaypoints: {
          // Spread to ensure all currently stored waypoint are kept
          // but the newest waypoint is added
          ...this.state.storedWaypoints,
          [this.state.newWaypointName]: newWaypoint,
        },
      },
      () => {
        // function callback so that when setState has finished executing
        // we can properly update the json file. File is created if now if
        // it doesn't already exist
        fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints), err => {
          if (err) throw err
        })
      }
    )
  }

  remove(): void {
    /* Deletes a selected stored position (if a position has been selected)
     * and properly updates the json file (see store() for more detailed comments)
     */
    const { storedWaypoints } = this.state
    delete storedWaypoints[this.state.selectedWaypoint]

    // Since the selectedPosition was just deleted, we want to grab a new
    // value. We grab the first key if one exists, or if not default to ""
    const selectedWaypoint = Object.keys(storedWaypoints).length ? Object.keys(storedWaypoints)[0] : ""

    this.setState(
      {
        storedWaypoints,
        selectedWaypoint,
      },
      () => {
        fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints), err => {
          if (err) throw err
        })
      }
    )
  }

  updateCoords(event: { target: { value: string } }, coord: string): void {
    this.setState({
      newWaypointCoords: {
        ...this.state.newWaypointCoords,
        [coord]: event.target.value,
      },
    })
  }

  changeFormat(event: { target: { value: string } }): void {
    let newWaypointCoords
    const coordinateFormat = event.target.value
    if (coordinateFormat === "LatLong") {
      newWaypointCoords = {
        lat: "",
        lon: "",
      }
    } else if (coordinateFormat === "DMS") {
      newWaypointCoords = {
        latD: "",
        latM: "",
        latS: "",
        lonD: "",
        lonM: "",
        lonS: "",
      }
    }
    this.setState({
      newWaypointCoords,
      coordinateFormat,
    })
  }

  checkBox(waypoint: string): void {
    this.setState(
      {
        storedWaypoints: {
          ...this.state.storedWaypoints,
          [waypoint]: {
            ...this.state.storedWaypoints[waypoint],
            onMap: !this.state.storedWaypoints[waypoint].onMap,
          },
        },
      },
      () => {
        fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints), err => {
          if (err) throw err
        })
      }
    )
  }

  colorChanged(color: any, waypoint: string): void {
    this.setState(
      {
        storedWaypoints: {
          ...this.state.storedWaypoints,
          [waypoint]: {
            ...this.state.storedWaypoints[waypoint],
            color: color.hex,
            colorPicker: false,
          },
        },
      },
      () => {
        fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints), err => {
          if (err) throw err
        })
      }
    )
  }

  openPicker(waypoint: string): void {
    this.setState(
      {
        storedWaypoints: {
          ...this.state.storedWaypoints,
          [waypoint]: {
            ...this.state.storedWaypoints[waypoint],
            colorPicker: true,
          },
        },
      },
      () => {
        fs.writeFile(filepath, JSON.stringify(this.state.storedWaypoints), err => {
          if (err) throw err
        })
      }
    )
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Waypoints</div>
        <div style={container}>
          <div style={row}>
            <button type="button" style={buttons} onClick={() => this.setState({ addingWaypoint: true })}>
              Add Waypoint
            </button>
            <button type="button" style={buttons} onClick={this.saveCurrent}>
              Save Current
            </button>
            <button type="button" style={buttons} onClick={this.remove}>
              Remove Selected
            </button>
          </div>
          <div style={{ ...listEntry, width: "94.2%", marginRight: "5.6%", marginLeft: "1.2%" }}>
            <div style={listSubEntry}>Name</div>
            <div style={listSubEntry}>Latitude</div>
            <div style={listSubEntry}>Longitude</div>
            <div style={listSubEntry}>Color</div>
            <div style={listSubEntry}>On Map</div>
          </div>
          <div style={scrollviewer}>
            {Object.keys(this.state.storedWaypoints).map((waypointName: string) => {
              const waypoint = this.state.storedWaypoints[waypointName]
              const backgroundStyle = {
                ...listEntry,
                backgroundColor: waypointName === this.state.selectedWaypoint ? "lightblue" : "white",
              }
              return (
                <button
                  key={waypointName}
                  type="button"
                  onClick={() => this.setState({ selectedWaypoint: waypointName })}
                  style={backgroundStyle}
                >
                  <span style={listSubEntry}>{waypoint.name}</span>
                  <span style={listSubEntry}>{waypoint.latitude}</span>
                  <span style={listSubEntry}>{waypoint.longitude}</span>
                  <button
                    type="button"
                    style={{ ...colorButton, backgroundColor: waypoint.color }}
                    onClick={() => this.openPicker(waypointName)}
                  >
                    {waypoint.color}
                  </button>
                  {waypoint.colorPicker ? (
                    <div style={colorPicker}>
                      <TwitterPicker
                        color={waypoint.color}
                        onChangeComplete={(color: any) => this.colorChanged(color, waypointName)}
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
                </button>
              )
            })}
          </div>
          {this.state.addingWaypoint ? (
            <div style={modal}>
              <div style={row}>
                <div style={title}>Name:</div>
                <input
                  type="text"
                  value={this.state.newWaypointName}
                  onChange={e => this.setState({ newWaypointName: e.target.value })}
                  autoFocus={this.state.addingWaypoint}
                  style={input}
                />
              </div>
              <select style={singleSelect} value={this.state.coordinateFormat} onChange={e => this.changeFormat(e)}>
                <option value="LatLong">LatLong</option>
                <option value="DMS">DMS</option>
              </select>
              {this.state.coordinateFormat === "LatLong" ? (
                <div>
                  <div style={row}>
                    <div style={title}>Latitude</div>
                    <input
                      type="text"
                      style={input}
                      value={this.state.newWaypointCoords.lat}
                      onChange={e => this.updateCoords(e, "lat")}
                    />
                  </div>
                  <div style={row}>
                    <div style={title}>Longitude</div>
                    <input
                      type="text"
                      style={input}
                      value={this.state.newWaypointCoords.lon}
                      onChange={e => this.updateCoords(e, "lon")}
                    />
                  </div>
                </div>
              ) : (
                <div style={row}>
                  <div>
                    <div>Latitude</div>
                    <div style={row}>
                      <div style={title}>Degrees</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.latD}
                        onChange={e => this.updateCoords(e, "latD")}
                      />
                    </div>
                    <div style={row}>
                      <div style={title}>Minutes</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.latM}
                        onChange={e => this.updateCoords(e, "latM")}
                      />
                    </div>
                    <div style={row}>
                      <div style={title}>Seconds</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.latS}
                        onChange={e => this.updateCoords(e, "latS")}
                      />
                    </div>
                  </div>
                  <div>
                    <div>Longitude</div>
                    <div style={row}>
                      <div style={title}>Degrees</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.lonD}
                        onChange={e => this.updateCoords(e, "lonD")}
                      />
                    </div>
                    <div style={row}>
                      <div style={title}>Minutes</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.lonM}
                        onChange={e => this.updateCoords(e, "lonM")}
                      />
                    </div>
                    <div style={row}>
                      <div style={title}>Seconds</div>
                      <input
                        type="text"
                        style={input}
                        value={this.state.newWaypointCoords.lonS}
                        onChange={e => this.updateCoords(e, "lonS")}
                      />
                    </div>
                  </div>
                </div>
              )}
              <div style={row}>
                <button
                  type="button"
                  style={{ ...modalButton, backgroundColor: "red" }}
                  onClick={() =>
                    this.setState({
                      addingWaypoint: false,
                      newWaypoint: {
                        ...this.state.newWaypoint,
                        name: "",
                      },
                    })
                  }
                >
                  Cancel
                </button>
                <button type="button" style={{ ...modalButton, backgroundColor: "green" }} onClick={this.store}>
                  Add
                </button>
              </div>
            </div>
          ) : null}
        </div>
      </div>
    )
  }
}

export default Angular
