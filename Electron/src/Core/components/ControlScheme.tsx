// eslint-disable-next-line max-classes-per-file
import React, { Component } from "react"
import CSS from "csstype"
import path from "path"
import fs from "fs"

/*
  make a file in rover drive that has this and for flight stick and link to it
  button buttonIndex (for xbox controller):
    0: A
    1: B
    2: X
    3: Y
    4: LB
    5: RB
    6: RT
    7: LT
    8: back
    9: start
    10: left stick click
    11: right stick click
    12: d up
    13: d down
    14: d left
    15: d right
    16: (unsure possibly home button)
  joystick buttonIndex
    0: left stick left/right
    1: left stick up/down
    2: right stick left/right
    3: right stick up/down
*/

const container: CSS.Properties = {
  display: "flex",
  fontFamily: "arial",
  borderTopWidth: "30px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  flexWrap: "wrap",
  flexDirection: "column",
  height: "calc(100% - 40px)",
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

// eslint-disable-next-line import/no-mutable-exports
export let controllerInputs: any = {}
const filepath = path.join(__dirname, "../assets/ControllerInput.json")
let CONTROLLERINPUT: any = {}
if (fs.existsSync(filepath)) {
  CONTROLLERINPUT = JSON.parse(fs.readFileSync(filepath).toString())
}

function controller(passedScheme: any, pos: any): any {
  controllerInputs = {}
  return setInterval(() => {
    // if navigator.getGampads()[pos] == flight stick
    let index: number
    let DeadZone = 0.05 // xbox one controller
    const controllerList = []
    for (let i = 0; i < 4; i++) {
      if (navigator.getGamepads()[i] != null) {
        controllerList.push(navigator.getGamepads()[i]?.id)
      }
    }

    const FlightStickIndex = controllerList.findIndex(element => {
      if (element && element.indexOf("Logitech Extreme 3D") >= 0) return true
      else return false
    })

    switch (pos) {
      case "Xbox 1":
        index = FlightStickIndex !== -1 && FlightStickIndex <= 0 ? 1 : 0
        break
      case "Xbox 2":
        index = FlightStickIndex !== -1 && FlightStickIndex <= 1 ? 2 : 1
        break
      case "Xbox 3":
        index = FlightStickIndex !== -1 && FlightStickIndex <= 2 ? 3 : 2
        break
      case "Flight Stick": // Logitech Extreme 3D
        index = FlightStickIndex
        DeadZone = 0.1
        break
      default:
        return
    }
    if (navigator.getGamepads()[index] != null && passedScheme !== "") {
      for (const button in CONTROLLERINPUT[passedScheme].bindings) {
        if (CONTROLLERINPUT[passedScheme].bindings[button].buttonType === "button") {
          controllerInputs[button] = navigator.getGamepads()[index]?.buttons[
            CONTROLLERINPUT[passedScheme].bindings[button].buttonIndex
          ].value
        } else {
          controllerInputs[button] = navigator.getGamepads()[index]?.axes[
            CONTROLLERINPUT[passedScheme].bindings[button].buttonIndex
          ]
          if (controllerInputs[button] >= -DeadZone && controllerInputs[button] <= DeadZone) {
            controllerInputs[button] = 0.0
          } else {
            controllerInputs[button] *= -1
          }
        }
      }
    }
  }, 50)
}

interface IProps {
  style?: CSS.Properties
  configs: string[]
}

interface IState {
  functionality: any
}

class ControlScheme extends Component<IProps, IState> {
  constructor(props: Readonly<IProps>) {
    super(props)
    this.state = {
      functionality: {
        Drive: {
          toggled: "Off",
          scheme: "TankDrive",
          controller: "Xbox 1",
          interval: null,
        },
        MainGimbal: {
          toggled: "Off",
          scheme: "Gimbal",
          controller: "Xbox 2",
          interval: null,
        },
        // Science and Arm are set to Xbox 3 so that they do not conflict with gimbal or drive as they might be on the same page
        Science: {
          toggled: "Off",
          scheme: "ScienceControls",
          controller: "Xbox 3",
          interval: null,
        },
        Arm: {
          toggled: "Off",
          scheme: "ArmControls",
          controller: "Xbox 3",
          interval: null,
        },
      },
    }
    this.schemeChange = this.schemeChange.bind(this)
    // detects if a controller disconnects
    window.addEventListener("gamepaddisconnected", function (e) {
      console.log("Gamepad disconnected from index %d: %s", e.gamepad.index, e.gamepad.id)
    })
  }
  // takes in the controllers scheme and the position in the array of controllers to determin which controller it is

  controllerChange(event: { target: { value: string } }, config: string): void {
    controllerInputs = {}
    let defaultScheme = ""
    for (const scheme in CONTROLLERINPUT) {
      if (
        CONTROLLERINPUT[scheme].config === config &&
        event.target.value.indexOf(CONTROLLERINPUT[scheme].controller) >= 0
      ) {
        defaultScheme = scheme
        break
      }
    }
    this.setState(
      {
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            controller: event.target.value,
            scheme: defaultScheme,
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
                interval: controller(
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
    controllerInputs = {}
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
                interval: controller(
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
    controllerInputs = {}
    // if toggling on
    if (this.state.functionality[config].toggled === "Off") {
      this.setState({
        functionality: {
          ...this.state.functionality,
          [config]: {
            ...this.state.functionality[config],
            toggled: "On",
            interval: controller(this.state.functionality[config].scheme, this.state.functionality[config].controller),
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
          {this.props.configs.map(config => {
            return (
              <div key={config} style={row}>
                <div style={readoutDisplay}>{config}</div>
                <select
                  value={this.state.functionality[config].controller}
                  onChange={e => this.controllerChange(e, config)}
                  style={{ flex: 1 }}
                >
                  {["Xbox 1", "Xbox 2", "Xbox 3", "Flight Stick"].map(controllerSelect => {
                    return (
                      <option value={controllerSelect} key={controllerSelect}>
                        {controllerSelect}
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
                    if (
                      CONTROLLERINPUT[scheme].config === config &&
                      this.state.functionality[config].controller.indexOf(CONTROLLERINPUT[scheme].controller) >= 0
                    )
                      return (
                        <option value={scheme} key={scheme}>
                          {scheme}
                        </option>
                      )
                    else return null
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
