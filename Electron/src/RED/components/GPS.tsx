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
  currentLat: number
  currentLon: number
  pitch: number
  yaw: number
  roll: number
  distance: number
  quality: number
}
class GPS extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      currentLat: 0,
      currentLon: 0,
      pitch: 0,
      yaw: 0,
      roll: 0,
      distance: 0,
      quality: 0,
    }

    rovecomm.on("GPSLatLon", (data: any) => this.GPSLatLon(data))
    rovecomm.on("IMUData", (data: any) => this.IMUData(data))
    rovecomm.on("LidarData", (data: any) => this.LidarData(data))
  }

  GPSLatLon(data: any) {
    const currentLat = data[0]
    const currentLon = data[1]
    this.setState({
      currentLat,
      currentLon,
    })
    this.props.onCoordsChange(currentLat, currentLon)
  }

  IMUData(data: any) {
    this.setState({
      pitch: data[0],
      yaw: data[1],
      roll: data[2],
    })
  }

  LidarData(data: any) {
    this.setState({
      distance: data[0],
      quality: data[1],
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>GPS</div>
        <div style={container}>
          {[
            { title: "Current Lat.", value: this.state.currentLat },
            { title: "Current Lon.", value: this.state.currentLon },
            { title: "Pitch", value: this.state.pitch },
            { title: "Yaw", value: this.state.yaw },
            { title: "Roll", value: this.state.roll },
            { title: "Distance", value: this.state.distance },
            { title: "Quality", value: this.state.quality },
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
