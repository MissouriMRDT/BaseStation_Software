import React, { Component } from "react"
import CSS from "csstype"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"
import Geneva from "./components/Geneva"
import Cameras from "../../Core/components/Cameras"

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

interface IProps {}

interface IState {}

class Science extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={column}>
        <SensorGraphs />
        <Spectrometer />
        <SpectrometerViewer />
        <div style={row}>
          <SensorData style={{ flex: 3, marginRight: "5px" }} />
          <Geneva style={{ flex: 1, marginLeft: "5px" }} />
        </div>
        <div style={row}>
          <Cameras defaultCamera={7} style={{ width: "50%", marginRight: "5px" }} />
          <Cameras defaultCamera={8} style={{ width: "50%", marginLeft: "5px" }} />
        </div>
      </div>
    )
  }
}
export default Science
