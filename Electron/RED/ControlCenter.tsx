import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"

const ControlStyle: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexWrap: "wrap",
  height: "100vh",
  alignContent: "flex-start",
}

interface IProps {}

interface IState {}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div style={ControlStyle}>
        <Log />
        <GPS />
        <Spectrometer />
        <SpectrometerViewer />
      </div>
    )
  }
}

export default ControlCenter
