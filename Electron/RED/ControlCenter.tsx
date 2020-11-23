import React, { Component } from "react"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"
import NewWindowComponent from "./components/Window"

interface IProps {}

interface IState {
  isNewWindow: boolean
}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      isNewWindow: false,
    }
  }

  render(): JSX.Element {
    return (
      <div>
        {
          // onClose will be fired when the new window is closed
          // everything inside NewWindowComponent is considered props.children and will be
          // displayed in a new window
          this.state.isNewWindow === false && (
            <NewWindowComponent>
              onClose={() => this.setState({ isNewWindow: false })}
              <h2>This will display in a new window</h2>
            </NewWindowComponent>
          )
        }
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
        <button
          type="button"
          onClick={() => this.setState({ isNewWindow: true })}
        >
          Open in new window
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
