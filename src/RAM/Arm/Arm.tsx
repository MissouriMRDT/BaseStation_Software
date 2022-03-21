import React, { Component } from "react"
import CSS from "csstype"
import ControlMultipliers, { controlMultipliers } from "./components/ControlMultipliers"
import IK from "./components/IK"
import Angular from "./components/Angular"
import Cameras from "../../Core/components/Cameras"
import ControlScheme, { controllerInputs } from "../../Core/components/ControlScheme"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import ControlFeatures from "./components/ControlFeatures"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 1,
  justifyContent: "space-between"
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
}

function arm(): void {
  let ArmWristBend = 0
  let ArmWristTwist = 0
  let ArmElbowTwist = 0
  let ArmElbowBend = 0
  let ArmBaseTwist = 0
  let ArmBaseBend = 0
  let moveArm = false

  if ("WristBendDirection" in controllerInputs && "WristBendMagnitude" in controllerInputs) {
    const direction = controllerInputs.WristBendDirection === 1 ? -1 : 1
    ArmWristBend = direction * controllerInputs.WristBendMagnitude * controlMultipliers.Wrist
    moveArm = true
  }

  if ("WristTwistDirection" in controllerInputs && "WristTwistMagnitude" in controllerInputs) {
    const direction = controllerInputs.WristTwistDirection === 1 ? -1 : 1
    ArmWristTwist = direction * controllerInputs.WristTwistMagnitude * controlMultipliers.Wrist
    moveArm = true
  }

  if ("ElbowBend" in controllerInputs && "ElbowTwist" in controllerInputs) {
    ArmElbowBend = controllerInputs.ElbowBend * controlMultipliers.Elbow
    ArmElbowTwist = controllerInputs.ElbowTwist * controlMultipliers.Elbow
    moveArm = true
  }

  if ("BaseBend" in controllerInputs && "BaseTwist" in controllerInputs) {
    ArmBaseTwist = controllerInputs.BaseTwist * controlMultipliers.Base
    ArmBaseBend = controllerInputs.BaseBend * controlMultipliers.Base
    moveArm = true
  }

  if (moveArm) {
    const armValues = [ArmWristBend, ArmWristTwist, ArmElbowTwist, ArmElbowBend, ArmBaseTwist, ArmBaseBend]
    rovecomm.sendCommand("ArmVelocityControl", armValues)
  }

  if ("GripperOpen" in controllerInputs && "GripperClose" in controllerInputs) {
    let Gripper = 0
    if (controllerInputs.GripperOpen === 1) {
      Gripper = 1 * controlMultipliers.Gripper
    } else if (controllerInputs.GripperClose === 1) {
      Gripper = -1 * controlMultipliers.Gripper
    } else {
      Gripper = 0
    }
    rovecomm.sendCommand("GripperMove", Gripper)
  }

  if ("SolenoidOn" in controllerInputs && "SolenoidOff" in controllerInputs) {
    if (controllerInputs.SolenoidOn === 1) {
      rovecomm.sendCommand("Solenoid", [1])
    } else if (controllerInputs.SolenoidOff === 1) {
      rovecomm.sendCommand("Solenoid", [0])
    }
  }
}

interface IProps {}

interface IState {}

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}

    setInterval(() => arm(), 100)
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <Angular style={{ flex: 1, marginRight: "2.5px" }} />
          <div style={{ ...column, flex: 1, marginLeft: "2.5px" }}>
            <IK />
            <ControlFeatures style={{ height: "100%" }} />
          </div>
        </div>
        <div style={row}>
          <Cameras defaultCamera={5} style={{ width: "50%", marginRight: "2.5px" }} />
          <Cameras defaultCamera={6} style={{ width: "50%", marginLeft: "2.5px" }} />
        </div>
        <ControlMultipliers />
        <ControlScheme configs={["Arm"]} />
      </div>
    )
  }
}
export default Arm
