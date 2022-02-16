import React, { Component } from "react"
import CSS from "csstype"
import fs from "fs"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { ColorStyleConverter } from "../../Core/ColorConverter"
import { BitmaskUnpack } from "../../Core/BitmaskUnpack"
import path from "path"

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
const textPad: CSS.Properties = {
  paddingLeft: "10px",
  paddingRight: "10px",
}
const mainContainer: CSS.Properties = {
  display: "flex",
  flexWrap: "wrap",
  flexDirection: "column",
  flexGrow: 1,
  fontFamily: "arial",
  borderTopWidth: "28px",
  borderColor: "#990000",
  borderBottomWidth: "2px",
  borderStyle: "solid",
  padding: "5px",
  fontSize: "10px",
  lineHeight: "10px",
}
const row: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 2,
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
  flexGrow: 2,
}
const readout: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 1,
  justifyContent: "space-between",
  fontFamily: "arial",
}
const btnArray: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "105px 130px 105px",
  justifyContent: "center",
  gridTemplateRows: "25px",
}
const cellReadoutContainer: CSS.Properties = {
  display: "grid",
  gridTemplateColumns: "auto auto auto auto",
}
const btnStyle: CSS.Properties = {
  width: "70px",
  cursor: "pointer",
}
let RovecommManifest: any = {}
const FILEPATH = path.join(__dirname, "../assets/RovecommManifest.json")
if (fs.existsSync(FILEPATH)) {
  RovecommManifest = JSON.parse(fs.readFileSync(FILEPATH).toString()).RovecommManifest
}

// The specific function of turnOffReboot() originates from how the control boards
// on the rover are programmed. If turnOffReboot() gets passed "0", the rover turns off.
// If "time" is any positive number, the rover just powercycles for that many seconds.
// It's been determined that 5 seconds is long enough for a standard powercycle as any longer
// is mostly redundent and would cut into the allowed time during competition, and any shorter
// is probably not enough time. ¯\_(ツ)_/¯
function turnOffReboot(time: number): void {
  rovecomm.sendCommand("BMSStop", [time])
}

interface IProps {
  style?: CSS.Properties
}

interface IState {
  boardTelemetry: any
  batteryTelemetry: any
}

class Power extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    const { Power, BMS } = RovecommManifest
    const boardTelemetry = {}
    const batteryTelemetry = {}
    console.log(Object.keys(Power.Commands))
    Object.keys(Power.Commands).forEach((Bus: string, index: any) => {
      Power.Commands[Bus].comments.split(", ").forEach((component: any) => {
        boardTelemetry[component] = { enabled: true, value: 0 }
      })
    })
    Object.keys(BMS.Telemetry).forEach((meas_group: string, index: any) => {
      batteryTelemetry[meas_group] = {}
      BMS.Telemetry[meas_group].comments.split(", ").forEach((element: any) => {
        batteryTelemetry[meas_group][element] = { value: 0 }
      })
    })
    this.state = {
      boardTelemetry,
      batteryTelemetry,
    }
    console.log(this.state.batteryTelemetry)
    this.boardListenHandler = this.boardListenHandler.bind(this)
    this.batteryListenHandler = this.batteryListenHandler.bind(this)
    /*
    rovecomm.on("MotorBusCurrent", (data: number[]) => this.boardListenHandler(data, MOTORS, false))
    rovecomm.on("MotorBusEnabled", (data: number[]) => this.boardListenHandler(data, MOTORS, true))
    rovecomm.on("SteeringMotorCurrents", (data: number[]) => this.boardListenHandler(data, STEERMOTORS, false))
    rovecomm.on("12VActBusEnabled", (data: number[]) => this.boardListenHandler(data, ACTBUS, true))
    rovecomm.on("12VLogicBusEnabled", (data: number[]) => this.boardListenHandler(data, LOGICBUS, true))
    rovecomm.on("12VBusCurrent", (data: number[]) => this.boardListenHandler(data, TWELVE_V_BUS, false))
    rovecomm.on("ThirtyVEnabled", (data: number[]) => this.boardListenHandler(data, THIRTY_V_BUS, true))
    rovecomm.on("30VBusCurrent", (data: number[]) => this.boardListenHandler(data, THIRTY_V_BUS, false))
    rovecomm.on("VacuumEnabled", (data: number[]) => this.boardListenHandler(data, VACUUM, true))
    rovecomm.on("VacuumCurrent", (data: number[]) => this.boardListenHandler(data, VACUUM, false))
    rovecomm.on("PackI_Meas", (data: number) => this.batteryListenHandler(data, TOTALPACKCURRENT))
    rovecomm.on("PackV_Meas", (data: number) => this.batteryListenHandler(data, TOTALPACKVOLTAGE))
    rovecomm.on("Temp_Meas", (data: number) => this.batteryListenHandler(data, BATTERY_TEMP)) */
  }

  boardListenHandler(data: number[], partList: string[], isEnabler: boolean): void {
    const { boardTelemetry } = this.state
    if (isEnabler) {
      const bitmask = BitmaskUnpack(data[0], partList.length)
      for (let i = 0; i < partList.length; i++) {
        boardTelemetry[partList[i]].enabled = Boolean(Number(bitmask[i]))
      }
    } else {
      for (let i = 0; i < partList.length; i++) {
        boardTelemetry[partList[i]].value = data[i]
      }
    }
    // The setState is kept until the end because of how the priority of state changes are handled.
    // This reason remains the same for all the functions with a setState at the end.
    this.setState({ boardTelemetry })
  }

  batteryListenHandler(data: number, partList: string[]): void {
    const { batteryTelemetry } = this.state
    for (let i = 0; i < partList.length; i++) {
      batteryTelemetry[partList[i]].value = data[i]
    }
    this.setState({ batteryTelemetry })
  }

  // except for the all motor buttons, buttonToggle() is called every time an enable/disable
  // button is pressed. When called, it's passed the name of a bus/device defined in the global
  // lists, and sets the state for that item to the opposite it was prior to the function call.
  // The ellipses function as the "spreading apart" operator that makes it so ONLY the specified
  // states get changed.
  buttonToggle(bus: string): void {
    this.setState(
      {
        boardTelemetry: {
          ...this.state.boardTelemetry,
          [bus]: {
            ...this.state.boardTelemetry[bus],
            enabled: !this.state.boardTelemetry[bus].enabled,
          },
        },
      }
      // after the state of a given device is changed, packCommand() gets called to send the command(s)
      // corresponding with the toggled device to the rover, making it so only relevent commands are sent
      // () => this.packCommand(bus)
    )
  }
  /*
  allMotorToggle(button: boolean): void {
    let { boardTelemetry } = this.state
    // to simplify things, the all motor toggle was split into two buttons: an all enabled and an all disable.
    // Both buttons will pass a boolean to allMotorToggle() where it goes through each item in the constant
    // list and sets all their "enabled" states to true. Same if allMotorToggle() is passed false except it
    // sets all the states to false. Then the state changes all get thrown to the actual setState() and the
    // command is packed and sent.
    if (button) {
      for (let i = 0; i < MOTORS.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [MOTORS[i]]: {
            ...boardTelemetry[MOTORS[i]],
            enabled: true,
          },
        }
      }
    } else {
      for (let i = 0; i < MOTORS.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [MOTORS[i]]: {
            ...boardTelemetry[MOTORS[i]],
            enabled: false,
          },
        }
      }
    }
    // MOTORS[0] is used used mostly arbitrarily. allMotorToggle() changes the
    // state of all the motors at the same time, but even if only one motor had
    // changed state, the state of ALL the motors are sent as a SINGLE packet
    // by packCommand(), according to the groups defined by roveComm.
    this.setState({ boardTelemetry }, () => this.packCommand(MOTORS[0]))
  }

  packCommand(bus: string): void {
    // pack command will be passed the object whose button got pressed (ie "Drive LF"
    // or "Vacuum", etc.). It then gets checked which list that object is a part of.
    // Multiple lists have objects the exist in multiple lists, hence only if's and
    // not if-else's. After an object's "home list" is determined, a new bitmask string
    // is made where each character corresponds to the "enabled" state of each object
    // in the "home list." After the bitmask's string is constructed, it gets turned
    // from a binary string into an integer and sent as a command to roveComm.
    if (MOTORS.includes(bus)) {
      let newBitMask = ""
      MOTORS.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("MotorBusEnable", [parseInt(newBitMask, 2)])
    }
    if (ACTBUS.includes(bus)) {
      let newBitMask = ""
      ACTBUS.forEach(part => {
        newBitMask += this.state.boardTelemetry[part].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VActBusEnable", [parseInt(newBitMask, 2)])
    }
    if (LOGICBUS.includes(bus)) {
      let newBitMask = ""
      LOGICBUS.forEach(board => {
        newBitMask += this.state.boardTelemetry[board].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VLogicBusEnable", [parseInt(newBitMask, 2)])
    }
    if (THIRTY_V_BUS.includes(bus)) {
      let newBitMask = ""
      THIRTY_V_BUS.forEach(lane => {
        newBitMask += this.state.boardTelemetry[lane].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("30VBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus === "Vacuum") {
      const newBitMask = this.state.boardTelemetry[bus].enabled ? "1" : "0"
      rovecomm.sendCommand("VacuumEnable", [parseInt(newBitMask, 2)])
    }
  }
*/
  render(): JSX.Element {
    return (
      <div style={this.props.style}>
        <div style={label}>Power and BMS</div>
        <div style={mainContainer}>
            <div style={{ ...column, height: "401px", flexWrap:"wrap", margin: "1px" }}>
              {Object.keys(this.state.boardTelemetry).map((part: any) => {
                const { enabled, value } = this.state.boardTelemetry[part]
                return (
                  <div key={undefined} style={{ ...row }}>
                    <button type="button" onClick={() => this.buttonToggle(enabled)} style={btnStyle}>
                      {enabled ? "Enabled" : "Disabled"}
                    </button>
                    <div style={ColorStyleConverter(value, 0, 7, 15, 120, 0, readout)}>
                      <h3 style={textPad}>{part}</h3>
                      <h3 style={textPad}>
                        {`${value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          maximumFractionDigits: 1,
                          minimumIntegerDigits: 2,
                        })} A`}
                      </h3>
                    </div>
                  </div>
                )
              })}
          </div>
          <div style={{ ...row, ...btnArray, gridTemplateColumns: "auto auto" }}>
            <button type="button" onClick={() => {}} style={{ cursor: "pointer" }}>
              Enable All Motors
            </button>
            <button type="button" onClick={() => {}} style={{ cursor: "pointer" }}>
              Disable All Motors
            </button>
            <button type="button" onClick={() => turnOffReboot(5)} style={{ cursor: "pointer" }}>
              REBOOT
            </button>
            <button type="button" onClick={() => turnOffReboot(0)} style={{ cursor: "pointer" }}>
              SHUT DOWN
            </button>
          </div>
          <h3 style={{ alignSelf: "center", fontSize: "14px", fontFamily: "arial" }}>
            -------------------------------------------
          </h3>
          <div style={{ ...row, width: "100%" }}>
            <div style={ColorStyleConverter(this.state.batteryTelemetry.Temp_Meas.value, 30, 75, 115, 120, 0, readout)}>
              <h3 style={textPad}>Battery Temperature</h3>
              <h3 style={textPad}>
                {this.state.batteryTelemetry.Temp_Meas.Temperature.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                °
              </h3>
            </div>
            <div
              style={ColorStyleConverter(
                this.state.batteryTelemetry.PackI_Meas["Total Current"].value,
                0,
                15,
                20,
                120,
                0,
                readout
              )}
            >
              <h3 style={textPad}>Total Pack Current</h3>
              <h3 style={textPad}>
                {`${(this.state.batteryTelemetry.PackI_Meas["Total Current"].value / 1000).toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })} A`}
              </h3>
            </div>
            <div
              style={ColorStyleConverter(
                this.state.batteryTelemetry.PackV_Meas["Pack Voltage"].value,
                22,
                28,
                33,
                0,
                120,
                readout
              )}
            >
              <h3 style={textPad}>Total Pack Voltage</h3>
              <h3 style={textPad}>
                {`${(this.state.batteryTelemetry.PackV_Meas["Pack Voltage"].value / 1000).toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 2,
                  minimumIntegerDigits: 2,
                })} V`}
              </h3>
            </div>
          </div>
          <div style={{ ...row, width: "100%" }}>
            <div style={{ ...cellReadoutContainer, width: "100%" }}>
              {Object.keys(this.state.batteryTelemetry.CellV_Meas).map(cell => {
                const { value } = this.state.batteryTelemetry.CellV_Meas[cell]
                return (
                  <div key={cell} style={ColorStyleConverter(value, 2.5, 3.1, 4.2, 0, 120, readout)}>
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>
                      {`${(value / 1000).toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                        minimumIntegerDigits: 1,
                      })} V`}
                    </h3>
                  </div>
                )
              })}
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default Power
