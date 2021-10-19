import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import Map from "./components/Map"
import Waypoints from "./components/Waypoints"
import NewWindowComponent from "../Core/Window"
import RoverOverviewOfNetwork from "../RON/RON"
import RoverAttachmentManager from "../RAM/RAM"
import RoverImageryDisplay from "../RID/RID"
import Cameras from "../Core/components/Cameras"
import Timer from "./components/Timer"
import Power from "./components/Power&BMS"
import ControlScheme from "../Core/components/ControlScheme"
import Drive from "./components/Drive"
import Gimbal from "./components/Gimbal"
import ThreeDRover from "../Core/components/ThreeDRover"
import Lighting from "./components/Lighting"
import Steering from "./components/Steering"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}

interface IProps {}

interface IState {
  storedWaypoints: any
  currentCoords: { lat: number; lon: number }
  ronOpen: boolean
  ramOpen: boolean
  ridOpen: boolean
  fourthHeight: number
}

class ControlCenter extends Component<IProps, IState> {
  waypointsInstance: any

  constructor(props: IProps) {
    super(props)
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, lon: 0 },
      ronOpen: false,
      ramOpen: false,
      ridOpen: false,
      fourthHeight: window.innerHeight / 4,
    }
    this.updateWaypoints = this.updateWaypoints.bind(this)
    this.updateCoords = this.updateCoords.bind(this)

    window.addEventListener("resize", () => this.setState({ fourthHeight: window.innerHeight / 4 }))
  }

  updateWaypoints(storedWaypoints: any): void {
    this.setState({
      storedWaypoints,
    })
  }

  updateCoords(lat: any, lon: any): void {
    this.setState({
      currentCoords: { lat, lon },
    })
  }

  render(): JSX.Element {
    return (
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
              <RoverImageryDisplay rowcol="" style={{ width: "100%", height: "100%" }} />
            </NewWindowComponent>
          )
        }
        <div style={{ ...column, width: "60%" }}>
          <div style={row}>
            <GPS onCoordsChange={this.updateCoords} style={{ flexGrow: 1, marginRight: "5px", width: "60%" }} />
            <ThreeDRover style={{ width: "40%" }} />
          </div>
          <div style={row}>
            <Waypoints
              onWaypointChange={this.updateWaypoints}
              currentCoords={this.state.currentCoords}
              ref={instance => {
                this.waypointsInstance = instance
              }}
              style={{ flexGrow: 1, marginRight: "5px" }}
            />
          </div>
          <Timer />
          <Log />
          <Power />
          <Drive />
          <div style={row}>
            <ControlScheme style={{ flexGrow: 1, marginRight: "5px" }} configs={["Drive", "MainGimbal"]} />
            <Gimbal />
          </div>
          <div style={row}>
            <button type="button" onClick={rovecomm.resubscribe} style={{ width: "100px" }}>
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
        <div style={{ ...column, width: "40%" }}>
          <Map
            style={{ minHeight: `${this.state.fourthHeight}px` }}
            storedWaypoints={this.state.storedWaypoints}
            currentCoords={this.state.currentCoords}
            store={(name: string, coords: any) => this.waypointsInstance.store(name, coords)}
            name="controlCenterMap"
          />
          <Cameras defaultCamera={1} maxHeight={this.state.fourthHeight} style={{ width: "100%" }} />
          <Cameras defaultCamera={2} maxHeight={this.state.fourthHeight} style={{ width: "100%" }} />
          <Cameras defaultCamera={3} maxHeight={this.state.fourthHeight} style={{ width: "100%" }} />
        </div>
      </div>
    )
  }
}

export default ControlCenter
