import React, { Component } from "react"
import CSS from "csstype"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"
import Heater from "./components/Heater"
import Cameras from "../../Core/components/Cameras"
import RockLookUp from "./components/rocklookup"
import ControlScheme, { controllerInputs } from "../../Core/components/ControlScheme"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import Fluorometer from "./components/Fluorometer"

const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
}
const sensorMotorMultiplier = 500
const scoopMotorMultiplier = 500

function science(): void {
  // Z actuation of the science system is controlled by the left up/down thumbstick
  if ("SensorAxis" in controllerInputs) {
    rovecomm.sendCommand("SensorAxis", [controllerInputs.SensorAxis * sensorMotorMultiplier])
  }

  if ("XoopAxis" in controllerInputs) {
    rovecomm.sendCommand("XoopAxis", [controllerInputs.XoopAxis * scoopMotorMultiplier])
  }

  if ("ZoopAxis" in controllerInputs) {
    rovecomm.sendCommand("ZoopAxis", [controllerInputs.ZoopAxis * scoopMotorMultiplier])
  }

  //Open scoop if *only* the OpenScoop button is pressed
  //Close scoop if *only* the CloseScoop button is pressed
  //If both are pressed, do nothing
  if ("OpenScoop" in controllerInputs) {
    if (controllerInputs.OpenScoop === 1) {
      rovecomm.sendCommand("ScoopGrabber", 0)
    }
  }
  if ("CloseScoop" in controllerInputs) {
    if (controllerInputs.CloseScoop === 1) {
      rovecomm.sendCommand("ScoopGrabber", 65)
    }
  }

  // Water controls are sent in one bitmasked value
  if ("Water1" in controllerInputs && "Water2" in controllerInputs && "Water3" in controllerInputs) {
    let water = ""
    water += controllerInputs.Water3
    water += controllerInputs.Water2
    water += controllerInputs.Water1
    rovecomm.sendCommand("Water", parseInt(water, 2))
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
        <div style={{ ...row, marginBottom: "7px" }}>
          <SensorData style={{ width: "34%", marginRight: "5px" }} />
          <Heater style={{ width: "33%" }} />
          <Fluorometer style={{ width: "33%", marginLeft: "5px" }} />
        </div>
        <ControlScheme configs={["Science"]} />
        <div style={row}>
          <Cameras defaultCamera={7} style={{ width: "50%", marginRight: "2.5px" }} />
          <RockLookUp style={{ marginLeft: "2.5px" }} />
        </div>
      </div>
    )
  }
}
export default Science
