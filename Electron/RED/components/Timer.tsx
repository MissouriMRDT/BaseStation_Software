import React, { Component } from "react"
import CSS from "csstype"

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
  seconds: number
}

class Timer extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      isRunning: false,
      seconds: 10,
    }
  }

  componentDidMount(): void {
    this.timerID = setInterval(() => this.tick(), 1000)
  }

  componentWillUnmount(): void {
    clearInterval(this.timerID)
  }

  tick = (): void => {
    if (this.state.isRunning) {
      this.setState(previousState => ({
        seconds: previousState.seconds - 1,
      }))
    }
  }

  toggle = (): void => {
    this.setState(previousState => ({
      isRunning: !previousState.isRunning,
    }))
  }

  reset = (): void => {
    this.setState({
      isRunning: false,
      seconds: 10,
    })
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Timer</div>
        <div>
          <p>{this.state.seconds}</p>
          <div>
            <button onClick={this.toggle} type="button">
              Toggle
            </button>
            <button onClick={this.reset} type="button">
              Reset
            </button>
          </div>
        </div>
      </div>
    )
  }
}

export default Timer
