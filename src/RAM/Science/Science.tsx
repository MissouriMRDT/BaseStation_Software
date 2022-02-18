import React, { Component } from "react"
import CSS from "csstype"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"
import Fluorometer from "./components/Fluorometer"
import Heater from "./components/Heater"
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

const waterMotorMultiplier = 500
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
  if ("OpenScoop" in controllerInputs && !("CloseScoop" in controllerInputs)) {
    rovecomm.sendCommand("ScoopGrabber", 180)
  } else if (!("OpenScoop" in controllerInputs) && "CloseScoop" in controllerInputs) {
    rovecomm.sendCommand("ScoopGrabber", 0)
  }

  // All of the water send values are in one array, and we only want to send no power or half power
  // (full power is a bit too strong, and negative implies we are trying to suck water/air out of the
  // test tubes into the chemical containers)
  if ("Water1" in controllerInputs && "Water2" in controllerInputs && "Water3" in controllerInputs) {
    const water = [0, 0, 0]
    if (controllerInputs.Water1 === 1) {
      water[0] = waterMotorMultiplier
    }
    if (controllerInputs.Water2 === 1) {
      water[1] = waterMotorMultiplier
    }
    if (controllerInputs.Water3 === 1) {
      water[2] = waterMotorMultiplier
    }
    rovecomm.sendCommand("Water", water)
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
        <div style={row}>
          <SensorData style={{ flex: 3, marginRight: "5px" }} />
          <Heater />
          <Fluorometer style={{ marginLeft: "5px" }} />
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
