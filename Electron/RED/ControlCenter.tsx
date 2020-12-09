import React, { Component } from "react"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import Spectrometer from "./components/Spectrometer"
import SpectrometerViewer from "./components/SpectrometerViewer"

interface IProps {}

interface IState {}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
  }

  render(): JSX.Element {
    return (
      <div>
        <Log />
        <GPS />
        <Spectrometer />
        <SpectrometerViewer />
        <button
          type="button"
          onClick={rovecomm.resubscribe}
          style={{ width: "100px" }}
        >
          Resubscribe All
        </button>
      </div>
    )
  }
}

export default ControlCenter
