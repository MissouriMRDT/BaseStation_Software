import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm, RovecommManifest } from "../RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
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
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}

interface IProps {
  defaultCamera: number
  style?: CSS.Properties
}

interface IState {
  currentCamera: number
  cameraIps: string[]
}

class Cameras extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      currentCamera: this.props.defaultCamera,
      cameraIps: [RovecommManifest.Camera1.Ip, RovecommManifest.Camera2.Ip, RovecommManifest.Autonomy.Ip],
    }

    // rovecomm.sendCommand(Packet(dataId, data), reliability)
  }

  ConstructAddress() {
    const index = Math.floor((this.state.currentCamera - 1) / 4)
    const camera = ((this.state.currentCamera - 1) % 4) + 1
    const ip = this.state.cameraIps[index]
    return `http://${ip}:8080/${camera}/stream`
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Cameras</div>
        <div style={container}>
          <div style={row}>
            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(num => {
              return (
                <button
                  type="button"
                  key={num}
                  onClick={() => this.setState({ currentCamera: num })}
                  style={{ flexGrow: 1 }}
                >
                  <h1 style={h1Style}>{num}</h1>
                </button>
              )
            })}
          </div>
          <img src={this.ConstructAddress()} alt={`Camera ${this.state.currentCamera}`} />
        </div>
      </div>
    )
  }
}

export default Cameras
