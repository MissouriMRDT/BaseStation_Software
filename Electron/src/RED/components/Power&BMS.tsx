import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"
import { ColorStyleConverter } from "../../Core/ColorConverter"
import { BitmaskUnpack } from "../../Core/BitmaskUnpack"

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
  flexDirection: "column",
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
}
const column: CSS.Properties = {
  display: "flex",
  flexDirection: "column",
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

const MOTORS = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
const STEERMOTORS = ["Steering LF", "Steering LR", "Steering RF", "Steering RR"]
const ACTBUS = ["Gimbal", "Multimedia", "Auxiliary"]
const LOGICBUS = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Nav", "Cameras", "Extra"]
const THIRTY_V_BUS = ["12V", "Comms", "Auxiliary", "Drive"]
const TWELVE_V_BUS = ["Gimbal", "Multimedia", "Autonomy", "Logic"]
const CELLS = ["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"]
const VACUUM = ["Vacuum"]
const TOTALPACKCURRENT = ["TotalPackCurrent"]
const TOTALPACKVOLTAGE = ["TotalPackVoltage"]
const BATTERY_TEMP = ["Temp"]

// The specific function of turnOffReboot() originates from how the control boards
// on the rover are programmed. If turnOffReboot() gets passed "0", the rover turns off.
// If "time" is any positive number, the rover just powercycles for that many seconds.
// It's been determined that 5 seconds is long enough for a standard powercycle as any longer
// is mostly redundent and would cut into the allowed time during competition, and any shorter
// is probably not enough time. ¯\_(ツ)_/¯
function turnOffReboot(time: number): void {
  rovecomm.sendCommand("BMSStop", [time])
}

interface IProps {}

interface IState {
  boardTelemetry: any
  batteryTelemetry: any
}

class Power extends Component<IProps, IState> {
  constructor(props: IProps) {
    super(props)
    this.state = {
      boardTelemetry: {
        "Drive LF": { enabled: false, value: 0 },
        "Drive LR": { enabled: false, value: 0 },
        "Drive RF": { enabled: false, value: 0 },
        "Drive RR": { enabled: false, value: 0 },
        "Steering LF": { value: 0 },
        "Steering LR": { value: 0 },
        "Steering RF": { value: 0 },
        "Steering RR": { value: 0 },
        "Spare Motor": { enabled: false, value: 0 },
        Gimbal: { enabled: false, value: 0 },
        Multimedia: { enabled: false, value: 0 },
        Autonomy: { enabled: false, value: 0 },
        Logic: { value: 0 },
        "12V": { enabled: false, value: 0 },
        Comms: { enabled: false, value: 0 },
        Auxiliary: { enabled: false, value: 0 },
        Drive: { enabled: false, value: 0 },
        Vacuum: { enabled: false, value: 0 },
        Nav: { enabled: false },
        Cameras: { enabled: false },
        Extra: { enabled: false },
      },
      batteryTelemetry: {
        Temp: { value: 0 },
        TotalPackCurrent: { value: 0 },
        TotalPackVoltage: { value: 0 },
        "Cell 1": { value: 0 },
        "Cell 2": { value: 0 },
        "Cell 3": { value: 0 },
        "Cell 4": { value: 0 },
        "Cell 5": { value: 0 },
        "Cell 6": { value: 0 },
        "Cell 7": { value: 0 },
        "Cell 8": { value: 0 },
      },
    }
    this.boardListenHandler = this.boardListenHandler.bind(this)
    this.batteryListenHandler = this.batteryListenHandler.bind(this)

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
    rovecomm.on("CellV_Meas", (data: number) => this.batteryListenHandler(data, CELLS))
    rovecomm.on("Temp_Meas", (data: number) => this.batteryListenHandler(data, BATTERY_TEMP))
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
      },
      // after the state of a given device is changed, packCommand() gets called to send the command(s)
      // corresponding with the toggled device to the rover, making it so only relevent commands are sent
      () => this.packCommand(bus)
    )
  }

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

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Power and BMS</div>
        <div style={mainContainer}>
          <div style={{ ...row, width: "100%" }}>
            {[
              [...STEERMOTORS, ...MOTORS],
              ["Gimbal", "Multimedia", "Autonomy", "Logic", "12V", "Comms", "Auxiliary", "Drive", "Vacuum"],
            ].map(part => {
              return (
                <div key={undefined} style={{ ...column, flexGrow: 1 }}>
                  {part.map(motor => {
                    return (
                      <div key={motor} style={row}>
                        {"enabled" in this.state.boardTelemetry[motor] ? (
                          <button type="button" onClick={() => this.buttonToggle(motor)} style={btnStyle}>
                            {this.state.boardTelemetry[motor].enabled ? "Enabled" : "Disabled"}
                          </button>
                        ) : (
                          <div style={{ width: "70px", backgroundColor: "hsl(0, 0%, 90%)" }} />
                        )}
                        <div
                          style={ColorStyleConverter(this.state.boardTelemetry[motor].value, 0, 7, 15, 120, 0, readout)}
                        >
                          <h3 style={textPad}>{motor}</h3>
                          <h3 style={textPad}>
                            {`${this.state.boardTelemetry[motor].value.toLocaleString(undefined, {
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
              )
            })}
          </div>
          <div style={{ ...row, ...btnArray, marginTop: "2%" }}>
            {["Nav", "Cameras", "Extra"].map(peripheral => {
              return (
                <button
                  type="button"
                  key={peripheral}
                  onClick={() => this.buttonToggle(peripheral)}
                  style={{ cursor: "pointer" }}
                >
                  {this.state.boardTelemetry[peripheral].enabled ? `${peripheral} Enabled` : `${peripheral} Disabled`}
                </button>
              )
            })}
          </div>
          <div style={{ ...row, ...btnArray, gridTemplateColumns: "auto auto" }}>
            <button type="button" onClick={() => this.allMotorToggle(true)} style={{ cursor: "pointer" }}>
              Enable All Motors
            </button>
            <button type="button" onClick={() => this.allMotorToggle(false)} style={{ cursor: "pointer" }}>
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
            <div style={ColorStyleConverter(this.state.batteryTelemetry.Temp.value, 30, 75, 115, 120, 0, readout)}>
              <h3 style={textPad}>Battery Temperature</h3>
              <h3 style={textPad}>
                {this.state.batteryTelemetry.Temp.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                °
              </h3>
            </div>
            <div
              style={ColorStyleConverter(
                this.state.batteryTelemetry.TotalPackCurrent.value,
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
                {`${this.state.batteryTelemetry.TotalPackCurrent.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })} A`}
              </h3>
            </div>
            <div
              style={ColorStyleConverter(
                this.state.batteryTelemetry.TotalPackVoltage.value,
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
                {`${this.state.batteryTelemetry.TotalPackVoltage.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 2,
                  minimumIntegerDigits: 2,
                })} V`}
              </h3>
            </div>
          </div>
          <div style={{ ...row, width: "100%" }}>
            <div style={{ ...cellReadoutContainer, width: "100%" }}>
              {CELLS.map(cell => {
                return (
                  <div
                    key={cell}
                    style={ColorStyleConverter(this.state.batteryTelemetry[cell].value, 2.5, 3.1, 4.2, 0, 120, readout)}
                  >
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>
                      {`${this.state.batteryTelemetry[cell].value.toLocaleString(undefined, {
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
