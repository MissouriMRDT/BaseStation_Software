import React, { Component } from "react"
import CSS from "csstype"
import { rovecomm } from "../../Core/RoveProtocol/Rovecomm"

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
  margin: "5px",
}
const readout: CSS.Properties = {
  display: "flex",
  flexDirection: "row",
  flexGrow: 1,
  justifyContent: "space-between",
  fontFamily: "arial",
  margin: "0px 5px",
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
  marginLeft: "3px",
  marginBottom: "3px",
  marginRight: "3px",
}

function turnOffReboot(time: number): void {
  rovecomm.sendCommand("BMSStop", time)
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
    rovecomm.on("MotorBusCurrent", (data: number) => this.motorBusCurrents(data))
    rovecomm.on("MotorBusEnabled", (data: number) => this.motorBusEnabled(data))
    rovecomm.on("SteeringMotorCurrents", (data: number) => this.steeringMotorCurrents(data)) // potential removal
    rovecomm.on("12VActBusEnable", (data: number) => this.twelveVActBusEnable(data))
    rovecomm.on("12VLogicBusEnable", (data: number) => this.twelveVLogicBusEnable(data))
    rovecomm.on("30VBusEnabled", (data: number) => this.thirtyVBusEnabled(data))
    rovecomm.on("VacuumEnabled", (data: number) => this.vacuumEnabled(data))
    rovecomm.on("VacuumCurrent", (data: number) => this.vacuumCurrent(data))
    rovecomm.on("PackI_Meas", (data: number) => this.packCurrentMeas(data))
    rovecomm.on("PackV_Meas", (data: number) => this.packVoltageMeas(data))
    rovecomm.on("CellV_Meas", (data: number) => this.cellsVoltMeas(data))
    rovecomm.on("Temp_Meas", (data: number) => this.battTempMeas(data))
  }

  motorBusEnabled(data: number): void {
    const bitmask = data.toString(2)
    const motors = ["Drive LF", "Drive LR", "Drive FF", "Drive FR", "Spare Motor"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  motorBusCurrents(data: number): void {
    const motors = ["Drive LF", "Drive LR", "Drive FF", "Drive FR", "Spare Motor"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  steeringMotorCurrents(data: number): void {
    const motors = ["Steering LF", "Steering LR", "Steering RF", "Steering RR"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < motors.length; i++) {
      boardTelemetry[motors[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVActBusEnable(data: number): void {
    const bitmask = data.toString(2)
    const peripherals = ["Gimbal", "Multimedia", "Auxilliary"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < peripherals.length; i++) {
      boardTelemetry[peripherals[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVLogicBusEnable(data: number): void {
    const bitmask = data.toString(2)
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Navigation", "Cameras", "Extra"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  twelveVBusCurrent(data: number): void {
    const boards = ["Gimbal", "Multimedia", "Autonomy", "Logic"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusEnabled(data: number): void {
    const bitmask = data.toString(2)
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].enabled = bitmask[i]
    }
    this.setState({ boardTelemetry })
  }

  thirtyVBusCurrents(data: number): void {
    const boards = ["12V", "Comms", "Auxiliary", "Drive"]
    const { boardTelemetry } = this.state
    for (let i = 0; i < boards.length; i++) {
      boardTelemetry[boards[i]].value = data[i]
    }
    this.setState({ boardTelemetry })
  }

  vacuumEnabled(data: number): void {
    const { boardTelemetry } = this.state
    boardTelemetry.Vacuum.enabled = data
    this.setState({ boardTelemetry })
  }

  vacuumCurrent(data: number): void {
    const { boardTelemetry } = this.state
    boardTelemetry.Vacuum.value = data
    this.setState({ boardTelemetry })
  }

  packCurrentMeas(data: number): void {
    const { batteryTelemetry } = this.state
    batteryTelemetry.TotalPackCurrent.value = data
    this.setState({ batteryTelemetry })
  }

  packVoltageMeas(data: number): void {
    const { batteryTelemetry } = this.state
    batteryTelemetry.TotalPackVoltage.value = data
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
    batteryTelemetry.Temp.value = data
    this.setState({ batteryTelemetry })
  }

  buttonToggle(bus: string): void {
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
    const actBus = ["Gimbal", "Multimedia", "Auxilliary"]
    const logicBus = ["Gimbal", "Multimedia", "Autonomy", "Drive", "Navigation", "Cameras", "Extra"]
    const thirtyVBus = ["12V", "Comms", "Auxiliary", "Drive"]
    this.setState({
      boardTelemetry: {
        ...this.state.boardTelemetry,
        [bus]: {
          ...this.state.boardTelemetry[bus],
          enabled: !this.state.boardTelemetry[bus].enabled,
        },
      },
    })
    if (bus in motors) {
      const newBitMask = ""
      motors.forEach(motor => newBitMask.concat(this.state.boardTelemetry[motor].enabled ? "1" : "0"))
      rovecomm.sendCommand("MotorBusEnabled", [parseInt(newBitMask, 2)])
    }
    if (bus in actBus) {
      const newBitMask = ""
      actBus.forEach(motor => newBitMask.concat(this.state.boardTelemetry[motor].enabled ? "1" : "0"))
      rovecomm.sendCommand("12VActBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus in logicBus) {
      const newBitMask = ""
      logicBus.forEach(motor => newBitMask.concat(this.state.boardTelemetry[motor].enabled ? "1" : "0"))
      rovecomm.sendCommand("12VLogicBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus in thirtyVBus) {
      const newBitMask = ""
      thirtyVBus.forEach(motor => newBitMask.concat(this.state.boardTelemetry[motor].enabled ? "1" : "0"))
      rovecomm.sendCommand("30VBusEnable", [parseInt(newBitMask, 2)])
    }
    if (bus === "Vacuum") {
      const newBitMask = this.state.boardTelemetry.vacuum.enabled ? "1" : "0"
      rovecomm.sendCommand("vacuumEnabled", [parseInt(newBitMask, 2)])
    }
    // rovecomm.sendCommand("packetName", [dataArray])
  }

  allMotorToggle(button: boolean): void {
    const motors = ["Drive LF", "Drive LR", "Drive RF", "Drive RR", "Spare Motor"]
    let { boardTelemetry } = this.state
    let i
    if (button) {
      for (i = 0; i < motors.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [motors[i]]: {
            ...boardTelemetry[motors[i]],
            enabled: true,
          },
        }
      }
    } else {
      for (i = 0; i < motors.length; i++) {
        boardTelemetry = {
          ...boardTelemetry,
          [motors[i]]: {
            ...boardTelemetry[motors[i]],
            enabled: false,
          },
        }
      }
    }
    this.setState({ boardTelemetry })
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
                      <div style={{ width: "30%" }} />
                    )}
                    <div style={readout}>
                      <h3>{motor}</h3>
                      <h3>
                        {this.state.boardTelemetry[motor].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          minimumIntegerDigits: 2,
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
                      <div style={{ width: "30%" }} />
                    )}
                    <div style={readout}>
                      <h3>{part}</h3>
                      <h3>
                        {this.state.boardTelemetry[part].value.toLocaleString(undefined, {
                          minimumFractionDigits: 1,
                          minimumIntegerDigits: 2,
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
            <div style={readout}>
              <h3>Battery Temperature</h3>
              <h3>{this.state.batteryTelemetry.Temp.value}°</h3>
            </div>
            <div style={readout}>
              <h3>Total Pack Current</h3>
              <h3>
                {this.state.batteryTelemetry.TotalPackCurrent.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                A
              </h3>
            </div>
            <div style={readout}>
              <h3>Total Pack Voltage</h3>
              <h3>
                {this.state.batteryTelemetry.TotalPackVoltage.value.toLocaleString(undefined, {
                  minimumFractionDigits: 1,
                  minimumIntegerDigits: 2,
                })}
                A
              </h3>
            </div>
          </div>
          <div style={{ ...row, width: "100%" }}>
            <div style={{ ...cellReadoutContainer, width: "100%" }}>
              {["Cell 1", "Cell 2", "Cell 3", "Cell 4", "Cell 5", "Cell 6", "Cell 7", "Cell 8"].map(cell => {
                return (
                  <div key={cell} style={readout}>
                    <h3>{cell}</h3>
                    <h3>
                      {this.state.batteryTelemetry[cell].value.toLocaleString(undefined, {
                        minimumFractionDigits: 1,
                        minimumIntegerDigits: 2,
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
