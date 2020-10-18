import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) / auto-flow dense",
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

interface IProps {}

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
    // something like this this.props.test.Log()
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
      <div>
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
