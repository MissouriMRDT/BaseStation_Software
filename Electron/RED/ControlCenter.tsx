import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import RAM from "../RAM/RAM"
import RON from "../RON/RON"
import GPS from "./components/GPS"
import Log from "./components/Log"
import Map from "./components/Map"
import Waypoints from "./components/Waypoints"
import NewWindowComponent from "../Core/Window"
import Spectrometer from "../RAM/components/Spectrometer"
import RoverOverviewOfNetwork from "../RON/RON"
import RoverAttachmentManager from "../RAM/RAM"

interface IProps {}

interface IState {
  storedWaypoints: any
  currentCoords: { lat: number; long: number }
  ronOpen: boolean
  ramOpen: boolean
}

class ControlCenter extends Component<IProps, IState> {
  waypointsInstance: any

  constructor(props: any) {
    super(props)
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, long: 0 },
      ronOpen: false,
      ramOpen: false,
    }
    this.updateWaypoints = this.updateWaypoints.bind(this)
    this.updateCoords = this.updateCoords.bind(this)
  }

  updateWaypoints(storedWaypoints: any): void {
    this.setState({
      storedWaypoints,
    })
  }

  updateCoords(lat: any, long: any): void {
    this.setState({
      currentCoords: { lat, long },
    })
  }

  render(): JSX.Element {
    return (
      <div>
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.ronOpen && (
            <NewWindowComponent onClose={() => this.setState({ ronOpen: false })} name="RON">
              <RoverOverviewOfNetwork />
            </NewWindowComponent>
          )
        }
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.ramOpen && (
            <NewWindowComponent onClose={() => this.setState({ ramOpen: false })} name="RAM">
              <RoverAttachmentManager />
            </NewWindowComponent>
          )
        }
        <Log />
        <GPS onCoordsChange={this.updateCoords} />
        <Waypoints
          onWaypointChange={this.updateWaypoints}
          currentCoords={this.state.currentCoords}
          ref={instance => {
            this.waypointsInstance = instance
          }}
        />
        <Map
          storedWaypoints={this.state.storedWaypoints}
          store={(name: string, coords: any) => this.waypointsInstance.store(name, coords)}
        />
        <button type="button" onClick={rovecomm.resubscribe} style={{ width: "100px" }}>
          Resubscribe All
        </button>
        <button type="button" onClick={() => this.setState({ ronOpen: true })}>
          Open Rover Overview of Network
        </button>
        <button type="button" onClick={() => this.setState({ ramOpen: true })}>
          Open Rover Attachment Manager
        </button>
      </div>
    )
  }
}

export default ControlCenter
