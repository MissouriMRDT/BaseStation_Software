import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { controllerInputs } from "../../Core/components/ControlScheme"

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
  /* This function returns a value [-1, 1] based off of the value of theta
   * This function really only makes sense with drive in mind - consider the case of the left wheel
   * When theta = pi/2, we want to full send forward, so we return 1
   * When theta = 0, we want to point turn clockwise, so we return -1
   * To map any polar value in between, we can use 4*theta/pi - 1
   * (note if theta = pi/2, 4*pi/2/pi-1 = 1; theta = 0, 4*0/pi-1 = -1; theta = pi/4, 4*pi/4/pi-1 = 0)
   * When theta = 3pi/4, the left wheels should be going forward at full strength, while the right wheels adjust
   * The bottom half of the unit circle is practically the same, with -1 when theta = -pi/4
   * And anywhere -pi/2 <= theta <= 0 we use -4*theta/pi - 3 in order to get the same examples shown above
   */
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
    return (-4 * theta) / pi - 3
  } else if (theta >= -pi / 2 && theta <= 0) {
    return -1
  }
  return 0
}

const maxSpeed = 1000

interface IProps {
  style?: CSS.Properties
}

interface IState {
  leftSpeed: number
  rightSpeed: number
  speedLimit: number
  steeringSpeed: number
  angle: number
}
class Drive extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      leftSpeed: 0,
      rightSpeed: 0,
      speedLimit: 300,
      steeringSpeed: 300,
      angle: 0,
    }

    this.speedLimitChange = this.speedLimitChange.bind(this)
    this.steeringSpeedChange = this.steeringSpeedChange.bind(this)
    setInterval(() => this.drive(), 100)
  }

  drive(): void {
    /* This function is called every 100ms, takes input from the controllerInputs vector, and sends the proper
     * commands to drive the left and right motors in tank drive. If being controlled via traditional tank
     * drive, "LeftSpeed" and "RightSpeed" will exist as keys of controllerInputs, and be the values to be sent
     * If we are in flightstick vector drive, the X, Y, and throttle positions will be keys of controllerInputs
     * and will be translated to proper left/right speeds (largely by the scaleVector function)
     * The appropriate values for left/right speeds are [-1000, 1000], but X/Y/Throttle will be [-1,1]
     */
    let leftSpeed = 0
    let rightSpeed = 0
    // Speed limit set by the GUI. If controller indicates 50% speed, thats 50% of the speedLimit, a value 0-1000
    let speedMultiplier = this.state.speedLimit
    if (
      ("ForwardBump" in controllerInputs && controllerInputs.ForwardBump === 1) ||
      ("BackwardBump" in controllerInputs && controllerInputs.BackwardBump === 1)
    ) {
      const direction = controllerInputs.ForwardBump === 1 ? 1 : -1
      leftSpeed = 50 * direction
      rightSpeed = 50 * direction

      rovecomm.sendCommand("DriveLeftRight", [leftSpeed, rightSpeed])
    } else if ("LeftSpeed" in controllerInputs && "RightSpeed" in controllerInputs) {
      leftSpeed = Math.round(controllerInputs.LeftSpeed * speedMultiplier)
      rightSpeed = Math.round(controllerInputs.RightSpeed * speedMultiplier)

      rovecomm.sendCommand("DriveLeftRight", [leftSpeed, rightSpeed])
    } else if ("VectorX" in controllerInputs && "VectorY" in controllerInputs && "Throttle" in controllerInputs) {
      const x = controllerInputs.VectorX
      const y = controllerInputs.VectorY
      // Computes the angle between the positive x axis and the vector (VectorX, VectorY)
      const theta = Math.atan2(y, x)
      // We base our speed off the value of the larger vector component
      const r = Math.max(Math.abs(x), Math.abs(y))
      // Scale vector is explained in terms of the left wheels, and the right wheels can be thought of as
      // a reflection over the y axis, which is easiest obtained by adjusting the angle 90deg and inverting the result
      leftSpeed = r * scaleVector(theta)
      rightSpeed = -1 * r * scaleVector(theta - Math.PI / 2)
      // We want the throttle to be seen as 0% when all the way down, and 100% when all the way up, but throttle
      // has values [-1, 1], so if we (throttle + 1) /2, we get [0,1]
      speedMultiplier *= (controllerInputs.Throttle + 1) / 2

      leftSpeed = Math.round(leftSpeed * speedMultiplier)
      rightSpeed = Math.round(rightSpeed * speedMultiplier)
      rovecomm.sendCommand("DriveLeftRight", [leftSpeed, rightSpeed])
    }

    if (
      "RotateCW" in controllerInputs &&
      "RotateCCW" in controllerInputs &&
      (controllerInputs.RotateCW || controllerInputs.RotateCCW)
    ) {
      let { angle } = this.state
      angle = (((angle + controllerInputs.RotateCW - controllerInputs.RotateCCW) % 360) + 360) % 360
      this.setState({ angle })
      // Currently, closed loop isn't fully operational, so we only use open loop control
      // rovecomm.sendCommand("SetSteeringAngle", angle)
      const direction: number = controllerInputs.RotateCW - controllerInputs.RotateCCW
      const speed: number = this.state.steeringSpeed * direction
      rovecomm.sendCommand("SetSteeringSpeeds", [speed, speed, speed, speed])
    } else if (
      "RotateTwist" in controllerInputs &&
      "RotateToggle" in controllerInputs &&
      controllerInputs.RotateToggle &&
      Math.abs(controllerInputs.RotateTwist) > 0.15
    ) {
      let { angle } = this.state
      angle = (((angle - controllerInputs.RotateTwist) % 360) + 360) % 360
      this.setState({ angle })
      // Currently, closed loop isn't fully operational, so we only use open loop control
      // rovecomm.sendCommand("SetSteeringAngle", angle)
      const speed: number = this.state.steeringSpeed * controllerInputs.RotateTwist
      rovecomm.sendCommand("SetSteeringSpeeds", [speed, speed, speed, speed])
    } else if (
      "RotateLF" in controllerInputs &&
      "RotateLR" in controllerInputs &&
      "RotateRF" in controllerInputs &&
      "RotateRR" in controllerInputs &&
      "IndependentCW" in controllerInputs &&
      "IndependentCCW" in controllerInputs
    ) {
      const LFSpeed =
        this.state.steeringSpeed *
        (controllerInputs.RotateLF ? controllerInputs.IndependentCW - controllerInputs.IndependentCCW : 0)
      const LRSpeed =
        this.state.steeringSpeed *
        (controllerInputs.RotateLR ? controllerInputs.IndependentCW - controllerInputs.IndependentCCW : 0)
      const RFSpeed =
        this.state.steeringSpeed *
        (controllerInputs.RotateRF ? controllerInputs.IndependentCW - controllerInputs.IndependentCCW : 0)
      const RRSpeed =
        this.state.steeringSpeed *
        (controllerInputs.RotateRR ? controllerInputs.IndependentCW - controllerInputs.IndependentCCW : 0)
      rovecomm.sendCommand("SetSteeringSpeeds", [LFSpeed, LRSpeed, RFSpeed, RRSpeed])
    }

    this.setState({
      leftSpeed,
      rightSpeed,
    })
  }

  speedLimitChange(event: { target: { value: string } }): void {
    let speedLimit = parseInt(event.target.value, 10)
    if (speedLimit < 0) {
      speedLimit = 0
    } else if (speedLimit > maxSpeed) {
      speedLimit = maxSpeed
    }
    this.setState({ speedLimit })
  }

  steeringSpeedChange(event: { target: { value: string } }): void {
    let steeringSpeed = parseInt(event.target.value, 10)
    if (steeringSpeed < 0) {
      steeringSpeed = 0
    } else if (steeringSpeed > maxSpeed) {
      steeringSpeed = maxSpeed
    }
    this.setState({ steeringSpeed })
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Drive</div>
        <div style={container}>
          <div style={row}>
            <progress
              value={this.state.leftSpeed < 0 ? -this.state.leftSpeed : 0}
              max={maxSpeed}
              style={{ transform: "rotate(180deg)" }}
            />
            <div style={{ width: "30%", textAlign: "center" }}>Left Speed: {this.state.leftSpeed}</div>
            <progress style={{ flexGrow: 1 }} value={this.state.leftSpeed > 0 ? this.state.leftSpeed : 0} max={1000} />
          </div>
          <div style={row}>
            <progress
              value={this.state.rightSpeed < 0 ? -this.state.rightSpeed : 0}
              max={maxSpeed}
              style={{ transform: "rotate(180deg)" }}
            />
            <div style={{ width: "30%", textAlign: "center" }}>Right Speed: {this.state.rightSpeed}</div>
            <progress
              style={{ flexGrow: 1 }}
              value={this.state.rightSpeed > 0 ? this.state.rightSpeed : 0}
              max={maxSpeed}
            />
          </div>
          <div style={row}>
            <div>
              Speed Limit:
              <input
                type="text"
                style={{ marginLeft: "5px" }}
                value={this.state.speedLimit || ""}
                onChange={this.speedLimitChange}
              />
            </div>
            <div>
              Steering Speed:
              <input
                type="text"
                style={{ marginLeft: "5px" }}
                value={this.state.steeringSpeed || ""}
                onChange={this.steeringSpeedChange}
              />
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Drive
