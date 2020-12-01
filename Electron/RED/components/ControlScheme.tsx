import React, { Component, ReactNode, useState } from "react"
import { render } from "react-dom";
import CSS from "csstype"
import { useGamepads } from 'react-gamepads'
import GamepadController from "./GamepadController"


//import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
// import { Packet } from "../../Core/RoveProtocol/Packet"

const h1Style: CSS.Properties = {
  fontFamily: "arial",
  fontSize: "12px",
}
const container: CSS.Properties = {
  display: "grid",
  fontFamily: "arial",
  width: "640px",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  grid: "repeat(1, 28px) / auto-flow dense",

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

const readoutDisplay: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto",
  fontSize: "14px",
  justifyContent: "space-between",
  fontFamily: "arial",
  paddingTop: "4px",
  paddingLeft: "3px",
  paddingRight: "3px",
  paddingBottom: "4px",
  marginRight: "2px",
  grid: "repeat(2, 28px) /auto-flow dense",
  }

interface IProps {
  children: ReactNode,
}

interface IState {}

export const Switch = () => {
  const [open, toggle] = useToggle();
  const [open2, toggle2] = useToggle();
  return (
    <>
      <p>Testing toggle 1: {`${open}`}</p>
      <p>Testing toggle 2: {`${open2}`}</p>
      <button
        onClick={() => {
          toggle();
        }}
      >
        Toggle 1
      </button>
      <button
        onClick={() => {
          toggle2();
        }}
      >
        Toggle 2
      </button>
    </>
  );
};

export const useToggle = (initialMode = false) => {
  const [open, setOpen] = useState(initialMode);
  const toggle = () => setOpen(!open);
  return [open, toggle, setOpen] as const;
};

class ControlScheme extends Component<IProps, IState> {
  constructor(props: any) {
    super(props)
    this.state = {
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>ControlScheme</div>
        <div style={container}>
          <switch />
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
              {({ on, toggle }) => (
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

