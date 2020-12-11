import React, { Component, ReactNode, useState } from "react"
import { render } from "react-dom"
import Gamepad from 'react-gamepad'
import CSS from "csstype"
import { CONTROLLERINPUT } from "../../Core/ControllerInput/ControllerInput"
//import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
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
  scheme: string,
  DeadZone: number,
}

export var input = {}

class ControlScheme extends Component<IProps, IState> {

  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      scheme: "TankDrive",
      DeadZone: 0.15,
    }
    window.addEventListener("gamepaddisconnected", function(e) {
      console.log("Gamepad disconnected from index %d: %s",
        e.gamepad.index, e.gamepad.id,
        //console.log(e)
        );
    });
    this.controller()
  }

  controller(): void{
    setInterval(() => {
      if(navigator.getGamepads()[0] != null)
      {
        for (const button in CONTROLLERINPUT[this.state.scheme]){
          if (CONTROLLERINPUT[this.state.scheme][button].buttonType == "button"){
            input[button] = navigator.getGamepads()[0]?.buttons[CONTROLLERINPUT[this.state.scheme][button].buttonIndex].value
          }
          else{
            input[button] = navigator.getGamepads()[0]?.axes[CONTROLLERINPUT[this.state.scheme][button].buttonIndex]
            if (input[button] >= -(this.state.DeadZone) && input[button] <= this.state.DeadZone){
              input[button] = 0.0
            }
          }
        }
        console.log(input)
      }
    }, 100)
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
          {Object.keys(CONTROLLERINPUT).map(scheme => {
              return(
              <option value={scheme} >{scheme}</option>
              )
            })}
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
    if(this.state.on){
      this.controller()
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
