import React, { Component } from "react"
import CSS from "csstype"
import ControlMultipliers from "./components/ControlMultipliers"
import IK from "./components/IK"
import Angular from "./components/Angular"
import Cameras from "../../Core/components/Cameras"

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

interface IProps {}

interface IState {}

class Arm extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <div style={row}>
          <Angular style={{ flex: 1, marginRight: "5px" }} />
          <IK style={{ flex: 1, marginLeft: "5px" }} />
        </div>
        <div style={row}>
          <Cameras defaultCamera={5} style={{ width: "50%", marginRight: "5px" }} />
          <Cameras defaultCamera={6} style={{ width: "50%", marginLeft: "5px" }} />
        </div>
        <ControlMultipliers />
      </div>
    )
  }
}
export default Arm
