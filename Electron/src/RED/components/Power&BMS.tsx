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

const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
const steerMotors = ["Steering LF", "Steering LR", "Steering RF", "Steering RR"]
const actBus = ["Gimbal", "multimedia", "Auxiliary"]
const logicBus = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Nav", "Cameras", "Extra"]
const thirtyVBus = ["12V", "Comms", "Auxiliary", "Drive"]
const twelveVBus = ["Gimbal", "Multimedia", "Autonomy", "Logic"]
const cells = ["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"]
const vacuum = ["Vacuum"]
const totalPackCurrent = ["TotalPackCurrent"]
const totalPackVoltage = ["TotalPackVoltage"]
const battTempMeas = ["Temp"]

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

    rovecomm.on("MotorBusCurrent", (data: number[]) => this.boardListenHandler(data, motors, false))
    rovecomm.on("MotorBusEnabled", (data: number[]) => this.boardListenHandler(data, motors, true))
    rovecomm.on("SteeringMotorCurrents", (data: number[]) => this.boardListenHandler(data, steerMotors, false)) // potential removal
    rovecomm.on("12VActBusEnabled", (data: number[]) => this.boardListenHandler(data, actBus, true))
    rovecomm.on("12VLogicBusEnabled", (data: number[]) => this.boardListenHandler(data, logicBus, true))
    rovecomm.on("12VBusCurrent", (data: number[]) => this.boardListenHandler(data, twelveVBus, false))
    rovecomm.on("ThirtyVEnabled", (data: number[]) => this.boardListenHandler(data, thirtyVBus, false))
    rovecomm.on("30VBusCurrent", (data: number[]) => this.boardListenHandler(data, thirtyVBus, false))
    rovecomm.on("VacuumEnabled", (data: number[]) => this.boardListenHandler(data, vacuum, true))
    rovecomm.on("VacuumCurrent", (data: number[]) => this.boardListenHandler(data, vacuum, false))
    rovecomm.on("PackI_Meas", (data: number) => this.batteryListenHandler(data, totalPackCurrent))
    rovecomm.on("PackV_Meas", (data: number) => this.batteryListenHandler(data, totalPackVoltage))
    rovecomm.on("CellV_Meas", (data: number) => this.batteryListenHandler(data, cells))
    rovecomm.on("Temp_Meas", (data: number) => this.batteryListenHandler(data, battTempMeas))
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
    this.setState({ boardTelemetry })
  }

  batteryListenHandler(data: number, partList: string[]): void {
    const { batteryTelemetry } = this.state
    for (let i = 0; i < partList.length; i++) {
      batteryTelemetry[partList[i]].value = data[i]
    }
    this.setState({ batteryTelemetry })
  }

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
      () => this.packCommand(bus)
    )
  }

  allMotorToggle(button: boolean): void {
    let { boardTelemetry } = this.state
    if (button) {
      for (let i = 0; i < motors.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [motors[i]]: {
            ...boardTelemetry[motors[i]],
            enabled: true,
          },
        }
      }
    } else {
      for (let i = 0; i < motors.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [motors[i]]: {
            ...boardTelemetry[motors[i]],
            enabled: false,
          },
        }
      }
    }
    this.setState({ boardTelemetry }, () => this.packCommand(motors[0]))
  }

  packCommand(bus: string): void {
    if (motors.includes(bus)) {
      let newBitMask = ""
      motors.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("MotorBusEnable", [parseInt(newBitMask, 2)])
    }
    if (actBus.includes(bus)) {
      let newBitMask = ""
      actBus.forEach(part => {
        newBitMask += this.state.boardTelemetry[part].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VActBusEnable", [parseInt(newBitMask, 2)])
    }
    if (logicBus.includes(bus)) {
      let newBitMask = ""
      logicBus.forEach(board => {
        newBitMask += this.state.boardTelemetry[board].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VLogicBusEnable", [parseInt(newBitMask, 2)])
    }
    if (thirtyVBus.includes(bus)) {
      let newBitMask = ""
      thirtyVBus.forEach(lane => {
        newBitMask += this.state.boardTelemetry[lane].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("30VBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus === "Vacuum") {
      const newBitMask = this.state.boardTelemetry.Vacuum.enabled ? "1" : "0"
      rovecomm.sendCommand("VacuumEnable", [parseInt(newBitMask, 2)])
    }
  }

  render(): JSX.Element {
    return (
      <div>
        <div style={label}>Power and BMS</div>
        <div style={mainContainer}>
          <div style={{ ...row, width: "100%" }}>
            <div
              /* first column of buttons and readouts */
              style={{ ...column, flexGrow: 1 }}
            >
              {[
                "Drive LF",
                "Drive LR",
                "Drive RF",
                "Drive RR",
                "Steering LF", // doesn't actually have enable
                "Steering LR", // doesn't actually have enable
                "Steering RF", // doesn't actually have enable
                "Steering RR", // doesn't actually have enable
                "Spare Motor",
              ].map(motor => {
                /* this and following map functions work to make the button onClick's, titles,
              and electrical current details all callable by the same respective "name" or "ID"
              stored in the above array */
                return (
                  <div key={motor} style={row}>
                    {"enabled" in this.state.boardTelemetry[motor] ? (
                      <button type="button" onClick={() => this.buttonToggle(motor)} style={btnStyle}>
                        {this.state.boardTelemetry[motor].enabled ? "Enabled" : "Disabled"}
                      </button>
                    ) : (
                      <div style={{ width: "70px", backgroundColor: "hsl(0, 0%, 90%)" }} />
                    )}
                    <div style={ColorStyleConverter(this.state.boardTelemetry[motor].value, 0, 7, 15, 120, 0, readout)}>
                      <h3 style={textPad}>{motor}</h3>
                      <h3 style={textPad}>
                        {this.state.boardTelemetry[motor].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          maximumFractionDigits: 1,
                          minimumIntegerDigits: 2,
                        })}
                        &nbsp;A
                      </h3>
                    </div>
                  </div>
                )
              })}
            </div>
            <div
              style={{ ...column, flexGrow: 1 }}
              /* second column of buttons and readouts */
            >
              {[
                "Gimbal",
                "Multimedia",
                "Autonomy",
                "Logic", // doesn't actually have enable
                "12V",
                "Comms",
                "Auxiliary",
                "Drive",
                "Vacuum",
              ].map(part => {
                return (
                  <div key={part} style={row}>
                    {"enabled" in this.state.boardTelemetry[part] ? (
                      <button type="button" onClick={() => this.buttonToggle(part)} style={btnStyle}>
                        {this.state.boardTelemetry[part].enabled ? "Enabled" : "Disabled"}
                      </button>
                    ) : (
                      <div style={{ width: "70px", backgroundColor: "hsl(0, 0%, 90%)" }} />
                    )}
                    <div style={ColorStyleConverter(this.state.boardTelemetry[part].value, 0, 7, 15, 120, 0, readout)}>
                      <h3 style={textPad}>{part}</h3>
                      <h3 style={textPad}>
                        {this.state.boardTelemetry[part].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          maximumFractionDigits: 1,
                          minimumIntegerDigits: 2,
                        })}
                        &nbsp;A
                      </h3>
                    </div>
                  </div>
                )
              })}
            </div>
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
                {this.state.batteryTelemetry.TotalPackCurrent.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                &nbsp;A
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
                {this.state.batteryTelemetry.TotalPackVoltage.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  maximumFractionDigits: 2,
                  minimumIntegerDigits: 2,
                })}
                &nbsp;V
              </h3>
            </div>
          </div>
          <div style={{ ...row, width: "100%" }}>
            <div style={{ ...cellReadoutContainer, width: "100%" }}>
              {["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"].map(cell => {
                return (
                  <div
                    key={cell}
                    style={ColorStyleConverter(this.state.batteryTelemetry[cell].value, 2.5, 3.1, 4.2, 0, 120, readout)}
                  >
                    <h3 style={textPad}>{cell}</h3>
                    <h3 style={textPad}>
                      {this.state.batteryTelemetry[cell].value.toLocaleString(undefined, {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                        minimumIntegerDigits: 1,
                      })}
                      &nbsp;V
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
