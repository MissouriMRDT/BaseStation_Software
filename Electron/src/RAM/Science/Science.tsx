import React, { Component } from "react"
import CSS from "csstype"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"
import Geneva from "./components/Geneva"
import Cameras from "../../Core/components/Cameras"
import ControlScheme, { controllerInputs } from "../../Core/components/ControlScheme"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  justifyContent: "space-between",
  flexGrow: 1,
  alignItems: "stretch",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 1,
  marginRight: "5px",
}

const motorMultiplier = 500
const zMotorMultiplier = 200

function science(): void {
  // Z actuation of the science system is controlled by the left up/down thumbstick
  if ("zDirection" in controllerInputs) {
    rovecomm.sendCommand("ZAxis", [controllerInputs.zDirection * zMotorMultiplier])
  }

  // Geneva should mainly be controlled by the gui, but open loop control is possible
  // via the bumpers
  if ("GenevaCCW" in controllerInputs || "GenevaCW" in controllerInputs) {
    let direction = 0
    if ("GenevaCCW" in controllerInputs && controllerInputs.GenevaCCW === 1) {
      direction = -1
    } else if ("GenevaCW" in controllerInputs && controllerInputs.GenevaCW === 1) {
      direction = 1
    }
    console.log("GenevaOpenLoop", direction * motorMultiplier)
    rovecomm.sendCommand("GenevaOpenLoop", [direction * motorMultiplier])
  }

  // All of the chemical send values are in one array, and we only want to send no power or half power
  // (full power is a bit too strong, and negative implies we are trying to suck water/air out of the
  // test tubes into the chemical containers)
  if ("Chem1" in controllerInputs && "Chem2" in controllerInputs && "Chem3" in controllerInputs) {
    const chemicals = [0, 0, 0]
    if (controllerInputs.Chem1 === 1) {
      chemicals[0] = motorMultiplier
    }
    if (controllerInputs.Chem2 === 1) {
      chemicals[1] = motorMultiplier
    }
    if (controllerInputs.Chem3 === 1) {
      chemicals[2] = motorMultiplier
    }
    rovecomm.sendCommand("Chemicals", chemicals)
  }

  // NOTE: This is NOT how this should be done for the 2021 rover.
  // We are testing the science system with  the broken 2020 power
  // board which does not allow us to enable / diable just the vacuum
  // so we plug vacuum into the wheels and turn them on / off instead
  // of driving around.
  if ("VacuumPulse" in controllerInputs) {
    const value = controllerInputs.VacuumPulse ? 255 : 0
    rovecomm.sendCommand("MotorBusEnable", value)
  }
}

interface IProps {}

interface IState {}

class Science extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}

    setInterval(() => science(), 100)
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <SensorGraphs />
        <Spectrometer />
        <div style={row}>
          <SensorData style={{ flex: 3, marginRight: "5px" }} />
          <Geneva style={{ flexGrow: 1, marginLeft: "5px" }} />
        </div>
        <ControlScheme configs={["Science"]} />
        <div style={row}>
          <Cameras defaultCamera={7} style={{ width: "50%", marginRight: "5px" }} />
          <Cameras defaultCamera={8} style={{ width: "50%", marginLeft: "5px" }} />
        </div>
      </div>
    )
  }
}
export default Science
