// eslint-disable-next-line max-classes-per-file
import React, { Component, ReactNode, useState } from "react"
import { render } from "react-dom"
import Gamepad from 'react-gamepad'
import CSS from "csstype"
import { CONTROLLERINPUT } from "../../Core/ControllerInput/ControllerInput"
// import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
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

interface IProps {}

interface IState {
  scheme: string,
  scheme2: string,
  DeadZone: number,
}

export var input = {}

class ControlScheme extends Component<IProps, IState> {

  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      scheme: "TankDrive",
      scheme2: "ArmControls",
      DeadZone: 0.15,
      // controllerInput: {drive: {controller, setInterval, scheme}, gimbal: {controller, setInterval, scheme}, ...}
    }
    this.schemeChange = this.schemeChange.bind(this)
    window.addEventListener("gamepaddisconnected", function(e) {
      console.log("Gamepad disconnected from index %d: %s",
        e.gamepad.index, e.gamepad.id,
        );
    });
  }

  controller(pos: any, ): void{
    if(pos != null){
      setInterval(() => {
          if(navigator.getGamepads()[pos] != null)
          {
            for (const button in CONTROLLERINPUT[this.state.scheme]){
              if (CONTROLLERINPUT[this.state.scheme][button].buttonType == "button"){
                input[button] = navigator.getGamepads()[pos]?.buttons[CONTROLLERINPUT[this.state.scheme][button].buttonIndex].value
              }
              else{
                input[button] = navigator.getGamepads()[pos]?.axes[CONTROLLERINPUT[this.state.scheme][button].buttonIndex]
                if (input[button] >= -(this.state.DeadZone) && input[button] <= this.state.DeadZone){
                  input[button] = 0.0
                }
              }
            }
            //console.log(navigator.getGamepads()[pos]?.id, input)
          }
      }, 100)
    }
  }

  schemeChange(e: any) {
    this.setState({scheme: e.target.value})
    console.log(this.state.scheme)
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>ControlScheme</div>
        <div style={container}>
          {/* [{name: Drive}].map({return(this.state.controllerInput[name].controller = value)}) */}
          <div style={readoutDisplay}>
            Drive
          </div>
          <select>
            <option value="Xbox 1">Xbox 1</option>
            <option selected value="Xbox 2">Xbox 2</option>
          </select>
          <select value={this.state.scheme} onChange={this.schemeChange}>
            {Object.keys(CONTROLLERINPUT).map(scheme => {
              return(
              <option value={scheme} >{scheme}</option>
              )
            })}
          </select>
          <ToggleButton>
            {({ on, toggle,  }) => (
              <button type="button" onClick={toggle}>
                {on ? "On" : "Off"}
                {on ? this.controller(0) : null}
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
          {Object.keys(CONTROLLERINPUT).map(scheme2 => {
              return(
              <option value={scheme2} >{scheme2}</option>
              )
            })}
          </select>
          <ToggleButton>
              {({ on, toggle}) => (
                <button type="button" onClick={toggle}>
                  {on ? "On" : "Off"}
                  {on ? this.controller(1) : null}
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
    //using this to test how clicking the button interacts with this part in case it helps with preventing it from running
    if(this.state.on){
      console.log("OFF: ")
      window.clearInterval(0)
    }
    else
    {
      console.log("ON: ")
      {this.controller}
    }

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
