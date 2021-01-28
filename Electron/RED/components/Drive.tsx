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

function scaleVector(thetaIn: number): number {
  const pi = Math.PI
  let theta = thetaIn
  if (theta < -pi) {
    theta += 2 * pi
  }
  if (theta >= 0 && theta <= pi / 2) {
    return (4 * theta) / pi - 1
  } else if (theta >= pi / 2 && theta <= pi) {
    return 1
  } else if (theta >= -pi && theta <= -pi / 2) {
    return (-4 / pi) * (theta + (5 / 4) * pi) + 2
  } else if (theta >= -pi / 2 && theta <= 0) {
    return -1
  }
  return 0
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  leftSpeed: number
  rightSpeed: number
  speedLimit: number
}
class Drive extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      leftSpeed: 0,
      rightSpeed: 0,
      speedLimit: 300,
    }

    setInterval(() => this.drive(), 100)
  }

  drive(): void {
    let leftSpeed = 0
    let rightSpeed = 0
    let speedMultiplier = this.state.speedLimit
    if ("LeftSpeed" in inputs && "RightSpeed" in inputs) {
      leftSpeed = inputs.LeftSpeed
      rightSpeed = inputs.RightSpeed
    } else if ("VectorX" in inputs && "VectorY" in inputs && "Throttle" in inputs) {
      const x = inputs.VectorX
      const y = inputs.VectorY
      const theta = Math.atan2(y, x)
      const r =
        Math.sqrt(x * x + y * y) / Math.min(Math.abs(Math.sin(theta) ** -1.0), Math.abs(Math.cos(theta) ** -1.0))
      leftSpeed = -1 * r * scaleVector(theta - Math.PI / 2)
      rightSpeed = r * scaleVector(theta)
      speedMultiplier *= inputs.Throttle
    }
    leftSpeed = Math.round(leftSpeed * speedMultiplier)
    rightSpeed = Math.round(rightSpeed * speedMultiplier)
    if ("ForwardBump" in inputs || "BackwardBump" in inputs) {
      const direction = "ForwardBump" in inputs ? 1 : -1
      rovecomm.sendCommand("DriveLeftRight", [50 * direction, 50 * direction])
    } else {
      rovecomm.sendCommand("DriveLeftRight", [leftSpeed, rightSpeed])
    }
    this.setState({
      leftSpeed,
      rightSpeed,
    })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>GPS</div>
        <div style={container}>
          <div style={row}>
            <progress
              value={this.state.leftSpeed < 0 ? -this.state.leftSpeed : 0}
              max={1000}
              style={{ display: "block", float: "right" }}
            />
            <div style={{ width: "30%", textAlign: "center" }}>Left Speed: {this.state.leftSpeed}</div>
            <progress style={{ flexGrow: 1 }} value={this.state.leftSpeed > 0 ? this.state.leftSpeed : 0} max={1000} />
          </div>
          <div style={row}>
            <progress
              value={this.state.rightSpeed < 0 ? -this.state.rightSpeed : 0}
              max={1000}
              style={{ justifyContent: "end" }}
            />
            <div style={{ width: "30%", textAlign: "center" }}>Right Speed: {this.state.rightSpeed}</div>
            <progress
              style={{ flexGrow: 1 }}
              value={this.state.rightSpeed > 0 ? this.state.rightSpeed : 0}
              max={1000}
            />
          </div>
          <div style={row}>
            Speed Limit:
            <input
              type="text"
              style={{ marginLeft: "5px" }}
              value={this.state.speedLimit || ""}
              onChange={e => this.setState({ speedLimit: parseInt(e.target.value, 10) })}
            />
          </div>
        </div>
      </div>
    )
  }
}

export default Drive
