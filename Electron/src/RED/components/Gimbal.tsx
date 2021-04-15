import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { controllerInputs } from "../../Core/components/ControlScheme"

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

interface IProps {
  style?: CSS.Properties
}
interface IState {
  controlling: string
  image: string
}

// Dynamic paths to import images used to indicate which gimbal is being controlled
const NotConnected = path.join(__dirname, "../assets/NotConnected.png")
const UpArrow = path.join(__dirname, "../assets/UpArrow.png")
const DownArrow = path.join(__dirname, "../assets/DownArrow.png")

class Gimbal extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      // Controlling will be "none" by default, then set to "main" or "drive", and image will update to match
      controlling: "none",
      image: NotConnected,
    }

    setInterval(() => this.gimbal(), 100)
  }

  gimbal(): void {
    let { controlling, image } = this.state
    // When up on the dpad is pressed, we switch to controlling main gimbal in the code, although no rovecomm command is sent
    // unless there is actually incoming control data from the thumbsticks
    // Similarly, down on the dpad switches to controlling the drive gimbals
    if ("MainGimbalSwitch" in controllerInputs && controllerInputs.MainGimbalSwitch === 1) {
      controlling = "Main"
      image = UpArrow
    } else if ("DriveGimbalSwitch" in controllerInputs && controllerInputs.DriveGimbalSwitch === 1) {
      controlling = "Drive"
      image = DownArrow
    }
    if (
      "PanLeft" in controllerInputs &&
      "TiltLeft" in controllerInputs &&
      "PanRight" in controllerInputs &&
      "TiltRight" in controllerInputs
    ) {
      // The multiples defined below are for Valkyries mounting positions, and the * 5 is just a small constant to tweak how quickly they respond
      // to controller input
      if (controlling === "Main") {
        rovecomm.sendCommand(
          "LeftMainGimbalIncrement",
          [controllerInputs.PanLeft * 5, controllerInputs.TiltLeft * 5],
          true
        )
        rovecomm.sendCommand(
          "RightMainGimbalIncrement",
          [controllerInputs.PanRight * 5, controllerInputs.TiltRight * 5],
          true
        )
      } else if (controlling === "Drive") {
        rovecomm.sendCommand(
          "LeftDriveGimbalIncrement",
          [controllerInputs.PanLeft * 5, controllerInputs.TiltLeft * 5],
          true
        )
        rovecomm.sendCommand(
          "RightDriveGimbalIncrement",
          [controllerInputs.PanRight * 5, controllerInputs.TiltRight * 5],
          true
        )
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
