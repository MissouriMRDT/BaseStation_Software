import React, { Component } from "react"

import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"
import Geneva from "./components/Geneva"
import SensorData from "./components/SensorData"
import SensorGraphs from "./components/SensorGraphs"
import CSS from "csstype"

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
        <button onClick={() => console.log("RAM clicked")} type="button">
          Sample RAM module
        </button>
      </div>
    )
  }
}
export default RoverAttachmentManager
