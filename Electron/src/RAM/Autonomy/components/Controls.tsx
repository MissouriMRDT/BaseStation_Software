import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../../Core/RoveProtocol/Rovecomm"

const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  borderTopWidth: "30px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexWrap: "wrap",
  flexDirection: "column",
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
}
const button: CSS.Properties = {
  fontFamily: "arial",
  flexGrow: 1,
  margin: "5px",
  fontSize: "14px",
  lineHeight: "24px",
  borderRadius: "20px",
  outline: "none",
}

function startAutonomy(): void {
  rovecomm.sendCommand("StartAutonomy", 1)
}
function stopAutonomy(): void {
  rovecomm.sendCommand("DisableAutonomy", 1)
}
function clearWaypoints(): void {
  rovecomm.emit("AutonomyActivity", "-------- Clearing Autonomy's waypoints --------")
  rovecomm.sendCommand("ClearWaypoints", 1)
}

interface IProps {
  style?: CSS.Properties
  selectedWaypoint: any
}
interface IState {}

class Controls extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}

    this.addMarkerLeg = this.addMarkerLeg.bind(this)
    this.addGateLeg = this.addGateLeg.bind(this)
  }

  addMarkerLeg(): void {
    rovecomm.emit(
      "AutonomyActivity",
      `Sending Marker (Lat: ${this.props.selectedWaypoint.latitude} Lon: ${this.props.selectedWaypoint.longitude})`
    )
    rovecomm.sendCommand("AddMarkerLeg", [this.props.selectedWaypoint.latitude, this.props.selectedWaypoint.longitude])
  }

  addGateLeg(): void {
    rovecomm.emit(
      "AutonomyActivity",
      `Sending Gate (Lat: ${this.props.selectedWaypoint.latitude} Lon: ${this.props.selectedWaypoint.longitude})`
    )
    rovecomm.sendCommand("AddGateLeg", [this.props.selectedWaypoint.latitude, this.props.selectedWaypoint.longitude])
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Controls</div>
        <div style={container}>
          <div style={row}>
            <button type="button" onClick={startAutonomy} style={button}>
              <h1 style={button}>Start Autonomy</h1>
            </button>
            <button type="button" onClick={stopAutonomy} style={button}>
              <h1 style={button}>Stop Autonomy</h1>
            </button>
            <button type="button" onClick={this.addMarkerLeg} style={button}>
              <h1 style={button}>Add Marker Waypoint</h1>
            </button>
            <button type="button" onClick={this.addGateLeg} style={button}>
              <h1 style={button}>Add Gate Waypoint</h1>
            </button>
            <button type="button" onClick={clearWaypoints} style={button}>
              <h1 style={button}>Clear Waypoints</h1>
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Controls
