import React, { Component } from "react"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import Map from "./components/Map"
import Waypoints from "./components/Waypoints"

interface IProps {}

interface IState {
  storedWaypoints: any
  currentCoords: { lat: number; long: number }
}

class ControlCenter extends Component<IProps, IState> {
  waypointsInstance: any

  constructor(props: any) {
    super(props)
    this.state = {
      storedWaypoints: {},
      currentCoords: { lat: 0, long: 0 },
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
      </div>
    )
  }
}

export default ControlCenter
