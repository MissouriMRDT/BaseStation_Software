import React, { Component, ReactNode, useState } from "react"
import { render } from "react-dom"
import Gamepad from 'react-gamepad'
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { redBright } from "chalk"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "30px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexWrap: "wrap",
  flexDirection: "row",
  gridAutoFlow: "column",
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
}

const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "12px",
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "6px",
  paddingLeft: "3px",
  paddingRight: "3px",
  paddingBottom: "4px",
  marginRight: "2px",
}

interface IProps {
}

interface IState {
}

class ControlScheme extends Component<IProps, IState> {

  constructor(props: Readonly<IProps>) {
    super(props)

    this.state = {
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>ControlScheme</div>
        <div style={container}>
          <div style={readoutDisplay}>
                Drive
            </div>
            <select>
              <option selected value="Xbox 1">Xbox 1</option>
              <option value="Xbox 2">Xbox 2</option>
            </select>
            <select>
              <option selected value="Tank Drive">Tank Drive</option>
              <option value="Xbox Gimbal">Xbox Gimbal</option>
            </select>
            <ToggleButton>
              {({ on, toggle }) => (
                <button type="button" onClick={toggle}>
                  {on ? "On" : "Off"}
                </button>
            )}
            </ToggleButton>
            <div style={readoutDisplay}>
              Main Gimbal
            </div>
            <select>
              <option value="Xbox 1">Xbox 1</option>
              <option selected value="Xbox 2">Xbox 2</option>
            </select>
            <select>
              <option value="Tank Drive">Tank Drive</option>
              <option selected value="Xbox Gimbal">Xbox Gimbal</option>
            </select>
            <ToggleButton>
                {({ on, toggle}) => (
                  <button type="button" onClick={toggle}>
                    {on ? "On" : "Off"}
                  </button>
                )}
            </ToggleButton>
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
