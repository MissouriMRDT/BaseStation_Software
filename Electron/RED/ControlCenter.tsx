import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import RAM from "../RAM/RAM"
import RON from "../RON/RON"
import GPS from "./components/GPS"
import Log from "./components/Log"
import NewWindowComponent from "../Core/Window"
import Spectrometer from "../RAM/components/Spectrometer"

interface IProps {}

interface IState {
  openRON: boolean
  openRAM: boolean
}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      openRON: false,
      openRAM: false,
    }
  }

  render(): JSX.Element {
    return (
      <div>
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.openRON && (
            <NewWindowComponent
              name="RON"
              onClose={() => this.setState({ openRON: false })}
            >
              <RON />
            </NewWindowComponent>
          )
        }
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.openRAM && (
            <NewWindowComponent
              name="RAM"
              onClose={() => this.setState({ openRAM: false })}
            >
              <RAM />
            </NewWindowComponent>
          )
        }
        <Log />
        <GPS />
        <Spectrometer />
        <button
          type="button"
          onClick={rovecomm.resubscribe}
          style={{ width: "100px" }}
        >
          Resubscribe All
        </button>
        <button type="button" onClick={() => this.setState({ openRON: true })}>
          Open Rover Overview of Network
        </button>
        <button type="button" onClick={() => this.setState({ openRAM: true })}>
          Open Rover Attachment Manager
        </button>
      </div>
    )
  }
}

export default ControlCenter
