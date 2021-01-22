// eslint-disable-next-line max-classes-per-file
import React, { Component, ReactNode, useState } from "react"
import CSS from "csstype"
import { CONTROLLERINPUT } from "../../Core/ControllerInput/ControllerInput"

const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  borderTopWidth: "30px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexWrap: "wrap",
  flexDirection: "column",
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
  fontSize: "16px",
  fontFamily: "arial",
  padding: "6px 3px 3px 4px",
  marginRight: "2px",
  flex: 1,
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  margin: "2px",
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  DeadZone: number
  functionality: any
}

export var input = {}

class ControlScheme extends Component<IProps, IState> {
  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      DeadZone: 0.15,
      functionality: {
        Drive: {
          toggled: "Off",
          scheme: "TankDrive",
          controller: "Xbox 1",
          interval: null,
        },
        MainGimbal: {
          toggled: "Off",
          scheme: "ArmControls",
          controller: "Xbox 2",
          interval: null,
        },
      },
      // controllerInput: {drive: {controller, setInterval, scheme}, Gimbal: {this.controller, setInterval, scheme}, ...},
    }
    this.schemeChange = this.schemeChange.bind(this)

    window.addEventListener("gamepaddisconnected", function (e) {
      console.log("Gamepad disconnected from index %d: %s", e.gamepad.index, e.gamepad.id)
    })
  }
  // takes in the controllers scheme and the position in the array of controllers to determin which controller it is

  controller(passedScheme: any, pos: any): any {
    input = {}
    return setInterval(() => {
      // if navigator.getGampads()[pos] == flight stick
      let index: number
      console.log(pos, this.state)
      switch (pos) {
        case "Xbox 1":
          index = 0
          break
        case "Xbox 2":
          index = 1
          break
        case "Xbox 3":
          index = 2
          break
        case "Flight Stick":
          index = 3
          break
        default:
          return
      }

      if (navigator.getGamepads()[index] != null) {
        for (const button in CONTROLLERINPUT[passedScheme]) {
          if (CONTROLLERINPUT[passedScheme][button].buttonType === "button") {
            input[button] = navigator.getGamepads()[index]?.buttons[
              CONTROLLERINPUT[passedScheme][button].buttonIndex
            ].value
          } else {
            input[button] = navigator.getGamepads()[index]?.axes[CONTROLLERINPUT[passedScheme][button].buttonIndex]
            if (input[button] >= -this.state.DeadZone && input[button] <= this.state.DeadZone) {
              input[button] = 0.0
            }
          }
        }
        console.log(input)
      }
    }, 100)
  }

  controllerChange(event: { target: { value: string } }, config: string): void {
    this.setState(
      {
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            controller: event.target.value,
            interval: clearInterval(this.state.functionality[config].interval),
          },
        },
      },
      () => {
        if (this.state.functionality[config].toggled === "On") {
          this.setState({
            functionality: {
              ...this.state.functionality,
              [config]: {
                ...this.state.functionality[config],
                interval: this.controller(
                  this.state.functionality[config].scheme,
                  this.state.functionality[config].controller
                ),
              },
            },
          })
        }
      }
    )
  }

  schemeChange(event: { target: { value: string } }, config: string): void {
    this.setState(
      {
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            scheme: event.target.value,
            interval: clearInterval(this.state.functionality[config].interval),
          },
        },
      },
      () => {
        if (this.state.functionality[config].toggled === "On") {
          this.setState({
            functionality: {
              ...this.state.functionality,
              [config]: {
                ...this.state.functionality[config],
                interval: this.controller(
                  this.state.functionality[config].scheme,
                  this.state.functionality[config].controller
                ),
              },
            },
          })
        }
      }
    )
  }

  buttonToggle(config: string): void {
    input = {}
    // if toggling on
    if (this.state.functionality[config].toggled === "Off") {
      this.setState({
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            toggled: "On",
            interval: this.controller(
              this.state.functionality[config].scheme,
              this.state.functionality[config].controller
            ),
          },
        },
      })
    }
    // if toggling off
    else if (this.state.functionality[config].toggled === "On") {
      this.setState({
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            toggled: "Off",
            interval: clearInterval(this.state.functionality[config].interval),
          },
        },
      })
    }
  }

  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Control Scheme</div>
        <div style={container}>
          {/* [{name: Drive}].map({return(this.state.controllerInput[name].controller = value)})["Xbox 1", "Xbox 2", "Xbox 3", "Flight Stick"] */}
          {["Drive", "MainGimbal"].map(config => {
            return (
              <div key={config} style={row}>
                <div style={readoutDisplay}>{config}</div>
                <select
                  value={this.state.functionality[config].controller}
                  onChange={e => this.controllerChange(e, config)}
                  style={{ flex: 1 }}
                >
                  {["Xbox 1", "Xbox 2", "Xbox 3", "Flight Stick"].map(controller => {
                    return (
                      <option value={controller} key={controller}>
                        {controller}
                      </option>
                    )
                  })}
                </select>
                <select
                  value={this.state.functionality[config].scheme}
                  onChange={e => this.schemeChange(e, config)}
                  style={{ flex: 1 }}
                >
                  {Object.keys(CONTROLLERINPUT).map(scheme => {
                    return (
                      <option value={scheme} key={scheme}>
                        {scheme}
                      </option>
                    )
                  })}
                </select>
                <button type="button" onClick={() => this.buttonToggle(config)}>
                  {this.state.functionality[config].toggled}
                </button>
              </div>
            )
          })}
        </div>
      </div>
    )
  }
}

export default ControlScheme
