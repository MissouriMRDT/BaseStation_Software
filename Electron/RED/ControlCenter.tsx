import React, { Component } from "react"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import NewWindowComponent from "../Core/Window"
import RoverOverviewOfNetwork from "../RON/RON"
import RoverAttachmentManager from "../RAM/RAM"

interface IProps {}

interface IState {
  ronOpen: boolean
  ramOpen: boolean
}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      ronOpen: false,
      ramOpen: false,
    }
  }

  render(): JSX.Element {
    return (
      <div>
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.ronOpen && (
            <NewWindowComponent onClose={() => this.setState({ ronOpen: false })} name="RON">
              <RoverOverviewOfNetwork />
            </NewWindowComponent>
          )
        }
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.ramOpen && (
            <NewWindowComponent onClose={() => this.setState({ ramOpen: false })} name="RAM">
              <RoverAttachmentManager />
            </NewWindowComponent>
          )
        }
        <Log />
        <GPS />
        <button type="button" onClick={rovecomm.resubscribe} style={{ width: "100px" }}>
          Resubscribe All
        </button>
        <button type="button" onClick={() => this.setState({ ronOpen: true })}>
          Open Rover Overview of Network
        </button>
        <button type="button" onClick={() => this.setState({ ramOpen: true })}>
          Open Rover Attachment Manager
        </button>
      </div>
    )
  }
}
/*
      <div>
        <Log />
        <GPS />
        <button
          type="button"
          onClick={rovecomm.resubscribe}
          style={{ width: "100px" }}
        >
          Resubscribe All
        </button>
        <button
          type="button"
          onClick={rovecomm.resubscribe}
          style={{ width: "100px" }}
        >
          Resubscribe All
        </button>
        <button
          type="button"
          onClick={rovecomm.resubscribe}
          style={{ width: "100px" }}
        >
          Resubscribe All
        </button>
      </div> */

export default ControlCenter
