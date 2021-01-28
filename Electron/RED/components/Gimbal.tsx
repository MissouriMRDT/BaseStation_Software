import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { inputs } from "./ControlScheme"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
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

interface IProps {
  style?: CSS.Properties
}

interface IState {
  controlling: string
  image: string
}
class Gimbal extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      controlling: "none",
      image: "../Core/Resources/NotConnected.png",
    }

    setInterval(() => this.gimbal(), 100)
  }

  gimbal(): void {
    let { controlling, image } = this.state
    if ("MainGimbalSwitch" in inputs && inputs.MainGimbalSwitch === 1) {
      controlling = "Main"
      image = "../Core/Resources/UpArrow.png"
    } else if ("DriveGimbalSwitch" in inputs && inputs.DriveGimbalSwitch === 1) {
      controlling = "Drive"
      image = "../Core/Resources/DownArrow.png"
    }
    if ("PanLeft" in inputs && "TiltLeft" in inputs) {
      if (controlling === "Main") {
        rovecomm.sendCommand("LeftMainGimbal", [inputs.PanLeft * -5, inputs.TiltLeft * 5])
        rovecomm.sendCommand("RightMainGimbal", [inputs.PanRight * -5, inputs.TiltRight * 5])
      } else if (controlling === "Drive") {
        rovecomm.sendCommand("LeftDriveGimbal", [inputs.PanLeft * 5, inputs.TiltLeft * -5])
        rovecomm.sendCommand("RightDriveGimbal", [inputs.PanRight * 5, inputs.TiltRight * -5])
      }
    }
    this.setState({
      controlling,
      image,
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Gimbal</div>
        <div style={container}>
          <img src={this.state.image} alt={this.state.controlling} />
        </div>
      </div>
    )
  }
}

export default Gimbal
