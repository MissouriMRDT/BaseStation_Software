import React, { Component } from "react"
import CSS from "csstype"
import ControlMultipliers, { controlMultipliers } from "./components/ControlMultipliers"
import IK from "./components/IK"
import Angular from "./components/Angular"
import Cameras from "../../Core/components/Cameras"
import { controllerInputs } from "../../Core/components/ControlScheme"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  flexGrow: 1,
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}

function arm(): void {
  let ArmWristBend = 0
  let ArmWristTwist = 0
  let ArmElbowTwist = 0
  let ArmElbowBend = 0
  let ArmBaseTwist = 0
  let ArmBaseBend = 0
  let Gripper = 0
  let Nipper = 0

  if ("WristBend" in controllerInputs && "WristTwist" in controllerInputs) {
    ArmWristBend = controllerInputs.WristBend * controlMultipliers.Wrist
    ArmWristTwist = controllerInputs.WristTwist * controlMultipliers.Wrist
  }

  if ("ElbowBend" in controllerInputs && "ElbowTwist" in controllerInputs) {
    ArmElbowBend = controllerInputs.ElbowBend * controlMultipliers.Elbow
    ArmElbowTwist = controllerInputs.ElbowTwist * controlMultipliers.Elbow
  }

  if ("BaseBendDirection" in controllerInputs && "BaseBendMagnitude" in controllerInputs) {
    const direction = controllerInputs.BaseBendDirection === 1 ? -1 : 1
    ArmBaseBend = direction * controllerInputs.BaseBendMagnitude * controlMultipliers.Base
  }

  if ("BaseTwistDirection" in controllerInputs && "BaseTwistMagnitude" in controllerInputs) {
    const direction = controllerInputs.BaseTwistDirection === 1 ? -1 : 1
    ArmBaseTwist = direction * controllerInputs.BaseTwistMagnitude * controlMultipliers.Base
  }

  if ("GripperOpen" in controllerInputs && "GripperClose" in controllerInputs) {
    if (controllerInputs.GripperOpen === 1) {
      Gripper = 1 * controlMultipliers.Gripper
    } else if (controllerInputs.GripperClose === 1) {
      Gripper = -1 * controlMultipliers.Gripper
    } else {
      Gripper = 0
    }
  }

  if ("Nipper" in controllerInputs) {
    Nipper = controllerInputs.Nipper
  }

  const armValues = [ArmWristBend, ArmWristTwist, ArmElbowTwist, ArmElbowBend, ArmBaseTwist, ArmBaseBend]

  rovecomm.sendCommand("ArmVelocityControl", armValues)
  rovecomm.sendCommand("GripperMove", Gripper)
  // rovecomm.sendCommand("NipperMove", Nipper)
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
          <Angular style={{ flex: 1, marginRight: "5px" }} />
          <IK style={{ flex: 1, marginLeft: "5px" }} />
        </div>
        <div style={row}>
          <Cameras defaultCamera={5} style={{ flex: 1, marginRight: "5px" }} />
          <Cameras defaultCamera={6} style={{ flex: 1, marginLeft: "5px" }} />
        </div>
        <ControlMultipliers />
      </div>
    )
  }
}
export default Arm
