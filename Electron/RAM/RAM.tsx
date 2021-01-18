import React, { Component } from "react"
import CSS from "csstype"
import ControlMultipliers from "./components/ControlMultipliers"
import IK from "./components/IK"
import Angular from "./components/angular"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"
import Geneva from "./components/Geneva"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"

const RON: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexWrap: "wrap",
  height: "100vh",
  alignContent: "flex-start",
}

interface IProps {}

interface IState {}

class RoverAttachmentManager extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={RON}>
        <Angular />
        <ControlMultipliers />
        <Geneva />
        <IK />
        <SensorData />
        <SensorGraphs />
        <Spectrometer />
        <SpectrometerViewer />
      </div>
    )
  }
}
export default RoverAttachmentManager
