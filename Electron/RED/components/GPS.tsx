import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { input } from "./ControlScheme"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
  padding: "5px",
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

interface IProps {
  onCoordsChange: (lat: number, lon: number) => void
  style?: CSS.Properties
}

interface IState {
  fixObtained: boolean
  fixQuality: number
  satelliteCount: number
  odometer: number
  currentLat: number
  currentLon: number
  lidar: number
}
class GPS extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      fixObtained: false,
      fixQuality: 255,
      satelliteCount: 255,
      odometer: 0,
      currentLat: 0,
      currentLon: 0,
      lidar: 0.0,
    }

    rovecomm.on("GPSTelem", (data: any) => this.GPSTelem(data))
    rovecomm.on("GPSPosition", (data: any) => this.GPSPosition(data))

    // rovecomm.sendCommand(dataIdStr, data, reliability)
    setInterval(() => this.drive(), 100)
    setInterval(() => this.gimbal(), 100)
  }

  drive() {
    if (input["LeftSpeed"] != undefined && input["RightSpeed"] != undefined ) {
      rovecomm.sendCommand("DriveLeftRight", [input["LeftSpeed"]*-300, input["RightSpeed"]*-300])
    }
  }

  gimbal() {
    if (input["PanLeft"] != undefined && input["TiltLeft"] != undefined ) {
      rovecomm.sendCommand("LeftMainGimbal", [input["PanLeft"]*-5, input["TiltLeft"]*5])
    }
  }

  GPSTelem(data: any) {
    this.setState({
      fixObtained: data[0] !== 0,
      fixQuality: data[0],
      satelliteCount: data[1],
    })
  }

  GPSPosition(data: any) {
    // We divide by 10000000 because currently waypoints are sent as shifted INT32s, not floats
    const currentLat = data[0] / 10000000
    const currentLon = data[1] / 10000000
    this.setState({
      currentLat,
      currentLon,
    })
    this.props.onCoordsChange(currentLat, currentLon)
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>GPS</div>
        <div style={container}>
          {[
            { title: "Fix Obtained", value: this.state.fixObtained.toString() },
            { title: "Fix Quality", value: this.state.fixQuality },
            { title: "Satellite Count", value: this.state.satelliteCount },
            { title: "Odometer (Miles)", value: this.state.odometer },
            { title: "Current Lat.", value: this.state.currentLat },
            { title: "Current Lon.", value: this.state.currentLon },
            { title: "Lidar", value: this.state.lidar },
          ].map(datum => {
            const { title, value } = datum
            return (
              <div key={title}>
                <h1 style={h1Style}>
                  {title}: {value}
                </h1>
              </div>
            )
          })}
        </div>
      </div>
    )
  }
}

export default GPS
