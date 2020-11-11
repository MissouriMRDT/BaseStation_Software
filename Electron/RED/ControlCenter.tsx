import React, { Component } from "react"
import { rovecomm } from "../Core/RoveProtocol/Rovecomm"
import GPS from "./components/GPS"
import Log from "./components/Log"

interface IProps {}

interface IState {}

class ControlCenter extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {}
    console.log("here")
  }

  render(): JSX.Element {
    return (
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
      </div>
    )
  }
}

export default ControlCenter
