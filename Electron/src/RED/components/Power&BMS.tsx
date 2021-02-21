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
  justifyContent: "space-around",
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
  gridTemplateColumns: "auto auto auto",
  justifyContent: "center",
  lineHeight: "11px",
}
const cellReadoutContainer: CSS.Properties = {
  display: "grid",
  color: "black",
  gridTemplateColumns: "auto auto auto auto",
}

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
    this.motorBusCurrents = this.motorBusCurrents.bind(this)
    this.motorBusEnabled = this.motorBusEnabled.bind(this)
    this.steeringMotorCurrents = this.steeringMotorCurrents.bind(this)
    this.twelveVActBusEnabled = this.twelveVActBusEnabled.bind(this)
    this.twelveVLogicBusEnabled = this.twelveVLogicBusEnabled.bind(this)
    this.twelveVBusCurrent = this.twelveVBusCurrent.bind(this)
    this.thirtyVBusEnabled = this.thirtyVBusEnabled.bind(this)
    this.thirtyVBusCurrents = this.thirtyVBusCurrents.bind(this)
    this.vacuumEnabled = this.vacuumEnabled.bind(this)
    this.vacuumCurrent = this.vacuumCurrent.bind(this)
    this.packCurrentMeas = this.packCurrentMeas.bind(this)
    this.packVoltageMeas = this.packVoltageMeas.bind(this)
    this.cellsVoltMeas = this.cellsVoltMeas.bind(this)
    this.battTempMeas = this.battTempMeas.bind(this)

    rovecomm.on("MotorBusCurrent", (data: number[]) => this.motorBusCurrents(data))
    rovecomm.on("MotorBusEnabled", (data: number[]) => this.motorBusEnabled(data))
    rovecomm.on("SteeringMotorCurrents", (data: number[]) => this.steeringMotorCurrents(data)) // potential removal
    rovecomm.on("12VActBusEnabled", (data: number[]) => this.twelveVActBusEnabled(data))
    rovecomm.on("12VLogicBusEnabled", (data: number[]) => this.twelveVLogicBusEnabled(data))
    rovecomm.on("12VBusCurrent", (data: number[]) => this.twelveVBusCurrent(data))
    rovecomm.on("ThirtyVEnabled", (data: number[]) => this.thirtyVBusEnabled(data))
    rovecomm.on("30VBusCurrent", (data: number[]) => this.thirtyVBusCurrents(data))
    rovecomm.on("VacuumEnabled", (data: number[]) => this.vacuumEnabled(data))
    rovecomm.on("VacuumCurrent", (data: number[]) => this.vacuumCurrent(data))
    rovecomm.on("PackI_Meas", (data: number) => this.packCurrentMeas(data))
    rovecomm.on("PackV_Meas", (data: number) => this.packVoltageMeas(data))
    rovecomm.on("CellV_Meas", (data: number) => this.cellsVoltMeas(data))
    rovecomm.on("Temp_Meas", (data: number) => this.battTempMeas(data))
  }

  motorBusEnabled(data: number[]): void {
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
    const bitmask = BitmaskUnpack(data[0], motors)
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].enabled = Boolean(Number(bitmask[i]))
    }
    this.setState({ boardTelemetry })
  }

  motorBusCurrents(data: number[]): void {
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  steeringMotorCurrents(data: number[]): void {
    const motors = ["Steering LF", "Steering LR", "Steering RF", "Steering RR"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVActBusEnabled(data: number[]): void {
    const peripherals = ["Gimbal", "Multimedia", "Auxiliary"]
    const bitmask = BitmaskUnpack(data[0], peripherals)
    const { boardTelemetry } = this.state
    for (let i = 0; i < peripherals.length; i++) {
      boardTelemetry[peripherals[i]].enabled = Boolean(Number(bitmask[i]))
    }
    this.setState({ boardTelemetry })
  }

  twelveVLogicBusEnabled(data: number[]): void {
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Nav", "Cameras", "Extra"]
    const bitmask = BitmaskUnpack(data[0], boards)
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = Boolean(Number(bitmask[i]))
    }
    this.setState({ boardTelemetry })
  }

  twelveVBusCurrent(data: number[]): void {
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Logic"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusEnabled(data: number[]): void {
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const bitmask = BitmaskUnpack(data[0], boards)
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = Boolean(Number(bitmask[i]))
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusCurrents(data: number[]): void {
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  vacuumEnabled(data: number[]): void {
    const { boardTelemetry } = this.state
    boardTelemetry.Vacuum.enabled = Boolean(Number(data[0]))
    this.setState({ boardTelemetry })
  }

  vacuumCurrent(data: number[]): void {
    const { boardTelemetry } = this.state
    // eslint-disable-next-line prefer-destructuring
    boardTelemetry.Vacuum.value = data[0]
    this.setState({ boardTelemetry })
  }

  packCurrentMeas(data: number): void {
    const { batteryTelemetry } = this.state
    // eslint-disable-next-line prefer-destructuring
    batteryTelemetry.TotalPackCurrent.value = data[0]
    this.setState({ batteryTelemetry })
  }

  packVoltageMeas(data: number): void {
    const { batteryTelemetry } = this.state
    // eslint-disable-next-line prefer-destructuring
    batteryTelemetry.TotalPackVoltage.value = data[0]
    this.setState({ batteryTelemetry })
  }

  cellsVoltMeas(data: number): void {
    const cells = ["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"]
    const { batteryTelemetry } = this.state
    for (let i = 0; i < cells.length; i++) {
      batteryTelemetry[cells[i]].value = data[i]
    }
    this.setState({ batteryTelemetry })
  }

  battTempMeas(data: number): void {
    const { batteryTelemetry } = this.state
    // eslint-disable-next-line prefer-destructuring
    batteryTelemetry.Temp.value = data[0]
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
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
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
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
    const actBus = ["Gimbal", "Multimedia", "Auxiliary"]
    const logicBus = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Nav", "Cameras", "Extra"]
    const thirtyVBus = ["12V", "Comms", "Auxiliary", "Drive"]
    if (motors.includes(bus)) {
      let newBitMask = ""
      motors.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("MotorBusEnable", [parseInt(newBitMask, 2)])
    }
    if (actBus.includes(bus)) {
      let newBitMask = ""
      actBus.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VActBusEnable", [parseInt(newBitMask, 2)])
    }
    if (logicBus.includes(bus)) {
      let newBitMask = ""
      logicBus.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("12VLogicBusEnable", [parseInt(newBitMask, 2)])
    }
    if (thirtyVBus.includes(bus)) {
      let newBitMask = ""
      thirtyVBus.forEach(motor => {
        newBitMask += this.state.boardTelemetry[motor].enabled ? "1" : "0"
      })
      rovecomm.sendCommand("30VBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus === "Vacuum") {
      const newBitMask = this.state.boardTelemetry.Vacuum.enabled ? "1" : "0"
      rovecomm.sendCommand("vacuumEnable", [parseInt(newBitMask, 2)])
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
                      <button type="button" onClick={() => this.buttonToggle(motor)} style={{ width: "30%" }}>
                        {this.state.boardTelemetry[motor].enabled ? "Enabled" : "Disabled"}
                      </button>
                    ) : (
                      <div style={{ width: "30%", backgroundColor: "hsl(0, 0%, 90%)" }} />
                    )}
                    <div style={ColorStyleConverter(this.state.boardTelemetry[motor].value, 0, 7, 15, 120, 0, readout)}>
                      <h3 style={textPad}>{motor}</h3>
                      <h3 style={textPad}>
                        {this.state.boardTelemetry[motor].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          minimumIntegerDigits: 2,
                          maximumFractionDigits: 2,
                        })}
                        A
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
                      <button type="button" onClick={() => this.buttonToggle(part)} style={{ width: "30%" }}>
                        {this.state.boardTelemetry[part].enabled ? "Enabled" : "Disabled"}
                      </button>
                    ) : (
                      <div style={{ width: "30%", backgroundColor: "hsl(0, 0%, 90%)" }} />
                    )}
                    <div style={ColorStyleConverter(this.state.boardTelemetry[part].value, 0, 7, 15, 120, 0, readout)}>
                      <h3 style={textPad}>{part}</h3>
                      <h3 style={textPad}>
                        {this.state.boardTelemetry[part].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          minimumIntegerDigits: 2,
                          maximumFractionDigits: 1,
                        })}
                        A
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
                <button type="button" key={peripheral} onClick={() => this.buttonToggle(peripheral)}>
                  {this.state.boardTelemetry[peripheral].enabled ? `${peripheral} Enabled` : `${peripheral} Disabled`}
                </button>
              )
            })}
          </div>
          <div style={{ ...row, ...btnArray, gridTemplateColumns: "auto auto" }}>
            <button type="button" onClick={() => this.allMotorToggle(true)}>
              Enable All Motors
            </button>
            <button type="button" onClick={() => this.allMotorToggle(false)}>
              Disable All Motors
            </button>
            <button type="button" onClick={() => turnOffReboot(5)}>
              REBOOT
            </button>
            <button type="button" onClick={() => turnOffReboot(0)}>
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
                  minimumIntegerDigits: 2,
                  maximumFractionDigits: 1,
                })}
                A
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
                  minimumIntegerDigits: 2,
                  maximumFractionDigits: 2,
                })}
                V
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
                        minimumIntegerDigits: 1,
                        maximumFractionDigits: 2,
                      })}
                      V
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
