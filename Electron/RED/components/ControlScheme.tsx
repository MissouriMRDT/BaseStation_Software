import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "block",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
}
const label: CSS.Properties = {
  marginTop: "-10px",
  position: "relative",
  top: "24px",
  left: "3px",
  fontFamily: "arial",
  fontSize: "16px",
  zIndex: 2,
  color: "white",
  gridRowStart: "2 & {}",
  grid: "repeat(2, 28px) /auto-flow dense",
}

const roAndBtnContainer: CSS.Properties = {
  // stands for "Readout and Button Container"; shortened for sanity
  display: "grid",
  width: "auto",
  gridTemplateColumns: "69px 1fr 69px 1fr",
  marginLeft: "2px",
  marginTop: "2px",
  marginBottom: "2px",
}

const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  backgroundColor: "#30ff00", // green
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "4px",
  paddingLeft: "3px",
  paddingRight: "3px",
  paddingBottom: "4px",
  marginRight: "2px",
}

interface IProps {
  defaultState: false
}

interface IState {
  currentState: boolean
  motorLF: number
}

class ControlScheme extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
      currentState: this.props.defaultState,
      motorLF: 0.0,
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>ControlScheme</div>
        <div style={container}>
          <div style={readoutDisplay}>
            <div style={readoutDisplay}>
                <span>Drive</span>
            </div>
            <ToggleButton>
                {({ on, toggle }) => (
                  <button type="button" onClick={toggle}>
                    {on ? "On" : "Off"}
                  </button>
                )}
            </ToggleButton>
            <div style={readoutDisplay}>
                <span>Main Gimbal</span>
            </div>
            <ToggleButton>
                {({ on, toggle }) => (
                  <button type="button" onClick={toggle}>
                    {on ? "On" : "Off"}
                  </button>
                )}
            </ToggleButton>
          </div>
        </div>
      </div>
    )
  }
}

class ToggleButton extends ControlScheme {
  state = {
    on: false,
  }

  toggle = () => {
    this.setState({
      on: !this.state.on,
    })
  }

  render() {
    const { children } = this.props
    return children({
      on: this.state.on,
      toggle: this.toggle,
    })
  }
}

export default ControlScheme
