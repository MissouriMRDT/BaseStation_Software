import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  right: 0,
  bottom: "0rem",
  padding: "0.5rem",
  fontFamily: "arial",
  fontSize: "0.5rem",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  width: "440px",
  borderTopWidth: "16px",
  borderColor: "darkred",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 36px) / auto-flow dense",
}
const label: CSS.Properties = {
  position: "absolute",
  top: "9px",
  fontSize: "12px",
  zIndex: 1,
  color: "white",
}

interface IProps {}

interface IState {
  fixObtained: boolean
  satelliteCount: number
  currentLat: number
  lidar: number
  fixQuality: number
  odometer: number
  currentLon: number
}

class GPS extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      fixObtained: false,
      satelliteCount: 255,
      currentLat: 0,
      lidar: 0.0,
      fixQuality: 255,
      odometer: 0,
      currentLon: 0,
    }

    rovecomm.on("GPSTelem", (data: any) => this.GPSTelem(data))
    rovecomm.on("GPSPosition", (data: any) => this.GPSPosition(data))

    // rovecomm.sendCommand(Packet(dataId, data), reliability)
  }

  GPSTelem(data: any) {
    this.setState({
      fixObtained: data[0] !== 0,
      fixQuality: data[0],
      satelliteCount: data[1],
    })
  }

  GPSPosition(data: any) {
    this.setState({
      currentLat: data[0] / 10000000,
      currentLon: data[1] / 10000000,
    })
  }

  render(): JSX.Element {
    return (
      <div style={container}>
        <div style={label}>GPS</div>
        {[
          { title: "Fix Obtained", value: this.state.fixObtained.toString() },
          { title: "Satellite Count", value: this.state.satelliteCount },
          { title: "Current Lat.", value: this.state.currentLat },
          { title: "Lidar", value: this.state.lidar },
          { title: "Fix Quality", value: this.state.fixQuality },
          { title: "Odometer (Miles)", value: this.state.odometer },
          { title: "Current Lon.", value: this.state.currentLon },
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
    )
  }
}

export default GPS
