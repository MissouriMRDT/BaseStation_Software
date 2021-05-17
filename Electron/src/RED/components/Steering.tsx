import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { controllerInputs } from "../../Core/components/ControlScheme"

const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexDirection: "column",
  height: "calc(100% - 38px)",
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
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  width: "100px",
}

// File path to up arrow in assets folder to use at dev or production time
const UpArrow = path.join(__dirname, "../assets/UpArrow.png")

function steer(): void {
  /* Takes controller input to send preset absolute positions to steering
   * or sends a default speed of 500 to spin CW or CCW
   */
  if ("PointTurnCW" in controllerInputs && controllerInputs.PointTurnCW === 1) {
    rovecomm.sendCommand("PointTurn", [500])
  } else if ("PointTurnCCW" in controllerInputs && controllerInputs.PointTurnCCW === 1) {
    rovecomm.sendCommand("PointTurn", [-500])
  } else if ("SteerUp" in controllerInputs && controllerInputs.SteerUp === 1) {
    // Data should come in as the current angles of LF, LR, RF, RR
    rovecomm.sendCommand("SetSteeringAngle", [0, 0, 0, 0])
  } else if ("SteerDown" in controllerInputs && controllerInputs.SteerDown === 1) {
    rovecomm.sendCommand("SetSteeringAngle", [180, 180, 180, 180])
  } else if ("SteerLeft" in controllerInputs && controllerInputs.SteerLeft === 1) {
    rovecomm.sendCommand("SetSteeringAngle", [90, 90, 90, 90])
  } else if ("SteerRight" in controllerInputs && controllerInputs.SteerRight === 1) {
    rovecomm.sendCommand("SetSteeringAngle", [270, 270, 270, 270])
  }
}

interface IProps {
  style?: CSS.Properties
}
interface IState {
  angles: number[]
  sendAngles: number[]
}

class Steering extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      // List of angles of actual positions of the steering motors
      angles: [0, 0, 0, 0],
      // List of angles that we want the motors to move to
      sendAngles: [0, 0, 0, 0],
    }
    rovecomm.on("DriveAngles", (data: any) => this.recieveAngles(data))
    setInterval(() => steer(), 100)
  }

  recieveAngles(data: any) {
    // Data should come in as the current angles of LF, LR, RF, RR
    // this is exactly what we want
    this.setState({ angles: data })
  }

  sendAngles() {
    // When we have configured all the angles how we want them, we want to send them all
    // at the same time. If we send on each change event, changing from 25 to 30 may
    // cause steering to waste time trying to move from 25 to 3, then from 3 to 30
    rovecomm.sendCommand("SetSteeringAngle", this.state.sendAngles)
  }

  angleChange(pos: number, value: string) {
    // Update event for text entry in each wheels text box. Requires the index position
    // of the wheel to know which to update. Again note sendAngles is LF, LR, RF, RR
    // while, left-->right, top-->bottom the display is LF, RF, LR, RR
    const sendAngles = [...this.state.sendAngles]
    sendAngles[pos] = parseInt(value, 10)
    this.setState({ sendAngles })
  }

  allAngleChange(value: string) {
    // Sets all send anlges to the value in the lower text box, overriding whatever they were
    const angle = parseInt(value, 10)
    const sendAngles = [angle, angle, angle, angle]
    this.setState({ sendAngles })
  }

  allAngleDisplay() {
    // The all angle display is only useful if all the angles are the same. As soon as any
    // of the angles are different, we don't want to display anything because it's not true
    const angles = this.state.sendAngles
    if (angles[0] === angles[1] && angles[1] === angles[2] && angles[2] === angles[3]) {
      return angles[0]
    }
    return ""
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Steering</div>
        <div style={container}>
          <div style={row}>
            <div style={column}>
              <img src={UpArrow} alt="Wheel 0" style={{ transform: `rotate(${this.state.angles[0]}deg)` }} />
              <input
                type="text"
                style={{ margin: "0px 10px", textAlign: "center" }}
                value={this.state.sendAngles[0]}
                onChange={e => this.angleChange(0, e.target.value)}
              />
            </div>
            <div style={column}>
              <img src={UpArrow} alt="Wheel 2" style={{ transform: `rotate(${this.state.angles[1]}deg)` }} />
              <input
                type="text"
                style={{ margin: "0px 10px", textAlign: "center" }}
                value={this.state.sendAngles[2]}
                onChange={e => this.angleChange(2, e.target.value)}
              />
            </div>
          </div>
          <div style={row}>
            <div style={column}>
              <img src={UpArrow} alt="Wheel 1" style={{ transform: `rotate(${this.state.angles[2]}deg)` }} />
              <input
                type="text"
                style={{ margin: "0px 10px", textAlign: "center" }}
                value={this.state.sendAngles[1]}
                onChange={e => this.angleChange(1, e.target.value)}
              />
            </div>
            <div style={column}>
              <img src={UpArrow} alt="Wheel 3" style={{ transform: `rotate(${this.state.angles[3]}deg)` }} />
              <input
                type="text"
                style={{ margin: "0px 10px", textAlign: "center" }}
                value={this.state.sendAngles[3]}
                onChange={e => this.angleChange(3, e.target.value)}
              />
            </div>
          </div>
          <div style={{ ...column, width: "100%", alignItems: "center" }}>
            <input
              type="text"
              style={{ margin: "0px 10px", textAlign: "center" }}
              value={this.allAngleDisplay()}
              onChange={e => this.allAngleChange(e.target.value)}
            />
            <button type="button" style={{ textAlign: "center", width: "50%" }} onClick={this.sendAngles}>
              Send
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Steering
