import React, { Component } from "react"
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
      </div>
    )
  }
}

export default ControlCenter
