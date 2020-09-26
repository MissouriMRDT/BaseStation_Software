import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 1,
  color: "white",
}

interface IProps {}

interface IState {
  isRunning: boolean
}

class Timer extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      isRunning: false,
    }
  }

  toggle = (): void => {
    this.setState(previousState => ({
      isRunning: !previousState.isRunning,
    }))
  }

  render(): JSX.Element {
    return (
      <div>
        <p style={label}>Timer</p>
      </div>
    )
  }
}

export default Timer
